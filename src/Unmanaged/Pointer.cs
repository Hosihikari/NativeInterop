﻿using System.Runtime.InteropServices;

namespace Hosihikari.NativeInterop.Unmanaged;

public unsafe struct Pointer<T> where T : class, ICppInstance<T>
{
    private nint _ptr;

    public readonly T Target => T.ConstructInstance(_ptr, false, false);

    private Pointer(nint ptr) => _ptr = ptr;

    public static implicit operator Pointer<T>(T ins) => new(ins.Pointer);

    public static explicit operator T(Pointer<T> ptr) => ptr.Target;

    public static explicit operator Pointer<T>(nint ptr) => new(ptr);

    public static implicit operator nint(Pointer<T> ptr) => ptr._ptr;

    public readonly Pointer<U> As<U>() where U : class, ICppInstance<U> => (Pointer<U>)_ptr;

    public void AddOffset(int offset) => _ptr += offset;

    public readonly void Delete()
    {
        if (_ptr == nint.Zero)
        {
            return;
        }
        T.DestructInstance(_ptr);
        NativeMemory.Free(_ptr.ToPointer());
    }
}
