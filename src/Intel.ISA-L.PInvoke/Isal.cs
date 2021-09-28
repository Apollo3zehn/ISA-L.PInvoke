using System;
using System.Runtime.InteropServices;
using System.Security;

namespace ISA_L.PInvoke
{
    public static class Isal
    {
        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.NATIVE_DLL_NAME)]
        public static extern unsafe int isal_inflate_init(IntPtr state);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.NATIVE_DLL_NAME)]
        public static extern unsafe int isal_inflate_reset(IntPtr state);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.NATIVE_DLL_NAME)]
        public static extern unsafe inflate_return_values isal_inflate(IntPtr state);
    }
}