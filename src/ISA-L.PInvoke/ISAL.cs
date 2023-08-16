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
        public static extern unsafe int isal_inflate_init(IntPtr state);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.NATIVE_DLL_NAME)]
        public static extern unsafe int isal_inflate_reset(IntPtr state);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.NATIVE_DLL_NAME)]
        public static extern unsafe inflate_return_values isal_inflate(IntPtr state);

        #endregion

        #region Deflate

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.NATIVE_DLL_NAME)]
        public static extern unsafe inflate_return_values isal_deflate(IntPtr stream);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.NATIVE_DLL_NAME)]
        public static extern unsafe int isal_deflate_reset(IntPtr stream);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(Constants.NATIVE_DLL_NAME)]
        public static extern unsafe int isal_deflate_init(IntPtr stream);

        #endregion
    }
}