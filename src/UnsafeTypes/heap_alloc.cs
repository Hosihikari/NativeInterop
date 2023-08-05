﻿using Hosihikari.NativeInterop.LibLoader;

namespace Hosihikari.NativeInterop.UnsafeTypes;

public readonly unsafe ref struct heap_alloc<T> where T : unmanaged
{
    private static readonly ulong size = (ulong)sizeof(T);

    public static T* New(in T val)
    {
        T* ptr = (T*)LibNative.operator_new(size);
        *ptr = val;
        return ptr;
    }

    public static T* NewArray(ulong count, in T defaultVal = default)
    {
        T* ptr, ret = ptr = (T*)LibNative.operator_new(count * size);
        for (ulong i = 0; i < count; ++i)
        {
            *ptr = defaultVal;
            ++ptr;
        }
        return ptr;
    }

    public static ref T NewAsRef(in T val) => ref *New(val);

    public static void Delete(T* ptr)
    {
        if (ptr is null)
            return;

        LibNative.operator_delete(ptr);
    }

    public static void DeleteAsRef(ref T val)
    {
        fixed (T* ptr = &val)
        {
            Delete(ptr);
        }
    }
}
