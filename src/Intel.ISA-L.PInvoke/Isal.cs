using System;
using System.Runtime.InteropServices;
using System.Security;

namespace ISA_L.PInvoke
{
    public static class Isal
    {
        //static Isal()
        //{
        //    // .NET Framework does not automatically load libraries from the native runtimes folder like .NET Core.
        //    // Therefore, if running .NET Framework, switch the current directory to the native runtime folder
        //    // before attempting to load the native HDF5 library.
        //    bool changedCurrentDir = false;
        //    var prevCurrentDir = Directory.GetCurrentDirectory();
        //    if (RuntimeInformation.FrameworkDescription.Contains("Framework"))
        //    {
        //        var dllDir = Path.Combine(prevCurrentDir, string.Format(IsalConstants.WindowsDLLPath, Environment.Is64BitProcess ? "64" : "86"));
        //        if (Directory.Exists(dllDir))
        //        {
        //            Directory.SetCurrentDirectory(dllDir);
        //            changedCurrentDir = true;
        //        }
        //    }

        //    try
        //    {
        //        Isal.blosc_init();
        //    }
        //    finally
        //    {
        //        if (changedCurrentDir)
        //            Directory.SetCurrentDirectory(prevCurrentDir);
        //    }
        //}

        [SuppressUnmanagedCodeSecurity]
        [DllImport(IsalConstants.NATIVE_DLL_NAME)]
        public static extern unsafe int isal_inflate_init(IntPtr state);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(IsalConstants.NATIVE_DLL_NAME)]
        public static extern unsafe inflate_return_values isal_inflate(IntPtr state);
    }
}