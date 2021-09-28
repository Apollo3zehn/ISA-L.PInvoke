namespace ISA_L.PInvoke
{
    public static class Constants
    {
        public const string NATIVE_DLL_NAME = "isa-l";
        public const string WindowsDLLPath = @"runtimes\win-x{0}\native";

        public const int SIZE_OF_INFLATE_HUFF_CODE_LARGE = 18912;
        public const int SIZE_OF_INFLATE_HUFF_CODE_SMALL = 2208;
        public const int HUFF_CODE_OFFSET = Constants.SIZE_OF_INFLATE_HUFF_CODE_LARGE + Constants.SIZE_OF_INFLATE_HUFF_CODE_SMALL;

        /* Inflate Implementation Specific Defines */
        public const int ISAL_DECODE_LONG_BITS = 12;
        public const int ISAL_DECODE_SHORT_BITS = 10;

        /* Deflate Compression Standard Defines */
        public const int ISAL_DEF_HIST_SIZE = 32 * 1024;
        public const int ISAL_DEF_MAX_HDR_SIZE = 328;

        public const int ISAL_DEF_LIT_SYMBOLS = 257;
        public const int ISAL_DEF_LEN_SYMBOLS = 29;
        public const int ISAL_DEF_DIST_SYMBOLS = 30;
        public const int ISAL_DEF_LIT_LEN_SYMBOLS = Constants.ISAL_DEF_LIT_SYMBOLS + Constants.ISAL_DEF_LEN_SYMBOLS;

        public const int ISAL_LOOK_AHEAD = (258 + 31) & ~31;

        /* Inflate structures */
        public const int ISAL_L_REM = 21 - Constants.ISAL_DECODE_LONG_BITS;
        public const int ISAL_S_REM = 15 - Constants.ISAL_DECODE_SHORT_BITS;

        public const int ISAL_L_DUP = (1 << Constants.ISAL_L_REM) - (Constants.ISAL_L_REM + 1);
        public const int ISAL_S_DUP = (1 << Constants.ISAL_S_REM) - (Constants.ISAL_S_REM + 1);

        public const int ISAL_L_UNUSED = (1 << Constants.ISAL_L_REM) - (1 << (Constants.ISAL_L_REM / 2)) - (1 << ((Constants.ISAL_L_REM + 1) / 2)) + 1;
        public const int ISAL_S_UNUSED = (1 << Constants.ISAL_S_REM) - (1 << ((Constants.ISAL_S_REM) / 2)) - (1 << ((Constants.ISAL_S_REM + 1) / 2)) + 1;

        public const int ISAL_L_SIZE = Constants.ISAL_DEF_LIT_LEN_SYMBOLS + Constants.ISAL_L_DUP + Constants.ISAL_L_UNUSED;
        public const int ISAL_S_SIZE = Constants.ISAL_DEF_DIST_SYMBOLS + Constants.ISAL_S_DUP + Constants.ISAL_S_UNUSED;

        public const int ISAL_HUFF_CODE_LARGE_LONG_ALIGNED = Constants.ISAL_L_SIZE + (-Constants.ISAL_L_SIZE & 0xf);
        public const int ISAL_HUFF_CODE_SMALL_LONG_ALIGNED = Constants.ISAL_S_SIZE + (-Constants.ISAL_S_SIZE & 0xf);
    }
}