using Hosihikari.NativeInterop.Layer;

namespace Hosihikari.NativeInterop.Hook;

public class HookInstance
{
    internal unsafe HookInstance(void* address, void* original)
    {
        _address = address;
        _original = original;
    }

    private unsafe void* _address;
    private unsafe void* _original;
    public unsafe void* Original
    {
        get
        {
            CheckActive();
            return _original;
        }
    }

    public HookResult Uninstall()
    {
        unsafe
        {
            CheckActive();
            HookResult result = LibHook.Unhook(_address);
            if (result is not HookResult.Success)
            {
                return result;
            }
            _address = null;
            _original = null;
            return result;
        }
    }

    private void CheckActive()
    {
        unsafe
        {
            if (_address is null || _original is null)
            {
                throw new InvalidOperationException(
                    "This hook is not active. Maybe already uninstalled."
                );
            }
        }
    }
}

