using System;
using System.Runtime.InteropServices;
using System.Security;

namespace ISA_L.PInvoke
{
    public static class ISAL
    {
        #region Inflate

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.NATIVE_DLL_NAME)]
        public static extern unsafe inflate_return_values isal_inflate(IntPtr state);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.NATIVE_DLL_NAME)]
        public static extern unsafe void isal_inflate_init(IntPtr state);

        #endregion

        #region Deflate

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.NATIVE_DLL_NAME)]
        public static extern unsafe void isal_deflate_init(IntPtr stream);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.NATIVE_DLL_NAME)]
        public static extern unsafe inflate_return_values isal_deflate(IntPtr stream);

        #endregion
    }
}