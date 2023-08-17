namespace ISA_L.PInvoke
{
    public static class Constants
    {
        public const string NATIVE_DLL_NAME = "isa-l";

        public const int SIZE_OF_INFLATE_HUFF_CODE_LARGE = (1 << ISAL_DECODE_LONG_BITS) * 4 + ISAL_HUFF_CODE_LARGE_LONG_ALIGNED * 2;
        public const int SIZE_OF_INFLATE_HUFF_CODE_SMALL = (1 << ISAL_DECODE_SHORT_BITS) * 2 + ISAL_HUFF_CODE_SMALL_LONG_ALIGNED * 2;
        public const int HUFF_CODE_OFFSET = SIZE_OF_INFLATE_HUFF_CODE_LARGE + SIZE_OF_INFLATE_HUFF_CODE_SMALL;

        /* Deflate Compression Standard Defines */
        public const int IGZIP_K = 1024;
        public const int ISAL_DEF_MAX_HDR_SIZE = 328;
        public const int ISAL_DEF_HIST_SIZE = 32 * IGZIP_K;
        public const int ISAL_DEF_MAX_MATCH = 258;

        public const int ISAL_DEF_LIT_SYMBOLS = 257;
        public const int ISAL_DEF_LEN_SYMBOLS = 29;
        public const int ISAL_DEF_DIST_SYMBOLS = 30;
        public const int ISAL_DEF_LIT_LEN_SYMBOLS = ISAL_DEF_LIT_SYMBOLS + ISAL_DEF_LEN_SYMBOLS;

        public const int ISAL_LOOK_AHEAD = (ISAL_DEF_MAX_MATCH + 31) & ~31;        

        /* Deflate Implementation Specific Defines */
        public const int IGZIP_HASH8K_HASH_SIZE = 8 * IGZIP_K;
        public const int IGZIP_HASH_HIST_SIZE = IGZIP_HIST_SIZE;
        public const int IGZIP_HASH_MAP_HASH_SIZE = IGZIP_HIST_SIZE;

        public const int IGZIP_LVL0_HASH_SIZE = 8 * IGZIP_K;
        public const int IGZIP_LVL1_HASH_SIZE = IGZIP_HASH8K_HASH_SIZE;
        public const int IGZIP_LVL2_HASH_SIZE = IGZIP_HASH_HIST_SIZE;
        public const int IGZIP_LVL3_HASH_SIZE = IGZIP_HASH_MAP_HASH_SIZE;

        public const int IGZIP_DIST_TABLE_SIZE = 2;
        public const int IGZIP_DECODE_OFFSET = 0;
        public const int IGZIP_LEN_TABLE_SIZE = 256;
        public const int IGZIP_LIT_TABLE_SIZE = ISAL_DEF_LIT_SYMBOLS;

        /* Inflate Implementation Specific Defines */
        public const int IGZIP_HIST_SIZE = ISAL_DEF_HIST_SIZE;

        public const int ISAL_DECODE_LONG_BITS = 12;
        public const int ISAL_DECODE_SHORT_BITS = 10;

        /* Compression structures */
        public const int ISAL_DEF_MIN_LEVEL = 0;
        public const int ISAL_DEF_MAX_LEVEL = 3;

        public const int ISAL_DEF_LVL0_REQ = 0;
        public const int ISAL_DEF_LVL1_REQ = 4 * IGZIP_K + 2 * IGZIP_LVL1_HASH_SIZE;
        public const int ISAL_DEF_LVL1_TOKEN_SIZE = 4;
        public const int ISAL_DEF_LVL2_REQ = 4 * IGZIP_K + 2 * IGZIP_LVL2_HASH_SIZE;
        public const int ISAL_DEF_LVL2_TOKEN_SIZE = 4;
        public const int ISAL_DEF_LVL3_REQ = 4 * IGZIP_K + 4 * 4 * IGZIP_K + 2 * IGZIP_LVL3_HASH_SIZE;
        public const int ISAL_DEF_LVL3_TOKEN_SIZE = 4;

        public const int ISAL_DEF_LVL0_LARGE = ISAL_DEF_LVL0_REQ;
        public const int ISAL_DEF_LVL0_DEFAULT = ISAL_DEF_LVL0_REQ;

        public const int ISAL_DEF_LVL1_LARGE = ISAL_DEF_LVL1_REQ + ISAL_DEF_LVL1_TOKEN_SIZE * 64 * IGZIP_K;
        public const int ISAL_DEF_LVL1_DEFAULT = ISAL_DEF_LVL1_LARGE;

        public const int ISAL_DEF_LVL2_LARGE = ISAL_DEF_LVL2_REQ + ISAL_DEF_LVL2_TOKEN_SIZE * 64 * IGZIP_K;
        public const int ISAL_DEF_LVL2_DEFAULT = ISAL_DEF_LVL2_LARGE;

        public const int ISAL_DEF_LVL3_LARGE = ISAL_DEF_LVL3_REQ + ISAL_DEF_LVL3_TOKEN_SIZE * 64 * IGZIP_K;
        public const int ISAL_DEF_LVL3_DEFAULT = ISAL_DEF_LVL3_LARGE;

        /* Inflate structures */
        public const int ISAL_L_REM = 21 - ISAL_DECODE_LONG_BITS;
        public const int ISAL_S_REM = 15 - ISAL_DECODE_SHORT_BITS;

        public const int ISAL_L_DUP = (1 << ISAL_L_REM) - (ISAL_L_REM + 1);
        public const int ISAL_S_DUP = (1 << ISAL_S_REM) - (ISAL_S_REM + 1);

        public const int ISAL_L_UNUSED = (1 << ISAL_L_REM) - (1 << (ISAL_L_REM / 2)) - (1 << ((ISAL_L_REM + 1) / 2)) + 1;
        public const int ISAL_S_UNUSED = (1 << ISAL_S_REM) - (1 << ((ISAL_S_REM) / 2)) - (1 << ((ISAL_S_REM + 1) / 2)) + 1;

        public const int ISAL_L_SIZE = ISAL_DEF_LIT_LEN_SYMBOLS + ISAL_L_DUP + ISAL_L_UNUSED;
        public const int ISAL_S_SIZE = ISAL_DEF_DIST_SYMBOLS + ISAL_S_DUP + ISAL_S_UNUSED;

        public const int ISAL_HUFF_CODE_LARGE_LONG_ALIGNED = ISAL_L_SIZE + (-ISAL_L_SIZE & 0xf);
        public const int ISAL_HUFF_CODE_SMALL_LONG_ALIGNED = ISAL_S_SIZE + (-ISAL_S_SIZE & 0xf);
    }
}