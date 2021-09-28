using System.Runtime.InteropServices;

namespace ISA_L.PInvoke
{
    // get total size: https://stackoverflow.com/questions/20979565/how-can-i-print-the-result-of-sizeof-at-compile-time-in-c

    // sizeof inflate_huff_code_large = 18912
    // sizeof inflate_huff_code_small = 2208
    // sizeof isal_block_state = 4

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct inflate_state
    {
        [FieldOffset(0)] public byte* next_out; // Next output Byte.
        [FieldOffset(8)] public uint avail_out; // Number of bytes available at next_out.
        [FieldOffset(12)] public uint total_out; // Total bytes written out so far.
        [FieldOffset(16)] public byte* next_in; // Next input byte.
        [FieldOffset(24)] public ulong read_in; // Bits buffered to handle unaligned streams.
        [FieldOffset(32)] public uint avail_in; // Number of bytes available at next_in.
        [FieldOffset(36)] public int read_in_length; // Bits in read_in.

        [FieldOffset(40)] public fixed byte lit_huff_code[18912]; // Structure for decoding lit/len symbols.
        [FieldOffset(40 + Constants.SIZE_OF_INFLATE_HUFF_CODE_LARGE)] public fixed byte dist_huff_code[2208]; // Structure for decoding dist symbols

        [FieldOffset(40 + Constants.HUFF_CODE_OFFSET)] public isal_block_state block_state; // Current decompression state.
        [FieldOffset(44 + Constants.HUFF_CODE_OFFSET)] public uint dict_length; // Length of dictionary used.
        [FieldOffset(48 + Constants.HUFF_CODE_OFFSET)] public uint bfinal; // Flag identifying final block.
        [FieldOffset(52 + Constants.HUFF_CODE_OFFSET)] public uint crc_flag; // Flag identifying whether to track of crc.
        [FieldOffset(56 + Constants.HUFF_CODE_OFFSET)] public uint crc; // Contains crc or adler32 of output if crc_flag is set.
        [FieldOffset(60 + Constants.HUFF_CODE_OFFSET)] public uint hist_bits; // Log base 2 of maximum lookback distance.

        [FieldOffset(64 + Constants.HUFF_CODE_OFFSET)] public int type0_block_len; // Length left to read of type 0 block when outbuffer overflow occurred
        [FieldOffset(68 + Constants.HUFF_CODE_OFFSET)] public int count; // Count of bytes remaining to be parsed
        [FieldOffset(72 + Constants.HUFF_CODE_OFFSET)] public uint dict_id;

        [FieldOffset(76 + Constants.HUFF_CODE_OFFSET)] public int write_overflow_lits;
        [FieldOffset(80 + Constants.HUFF_CODE_OFFSET)] public int write_overflow_len;
        [FieldOffset(84 + Constants.HUFF_CODE_OFFSET)] public int copy_overflow_length; // Length left to copy when outbuffer overflow occurred.
        [FieldOffset(88 + Constants.HUFF_CODE_OFFSET)] public int copy_overflow_distance; // Lookback distance when outbuffer overflow occurred.
        [FieldOffset(92)] public short wrapper_flag;
        [FieldOffset(94)] public short tmp_in_size; // Number of bytes in tmp_in_buffer
        [FieldOffset(96)] public int tmp_out_valid; // Number of bytes in tmp_out_buffer
        [FieldOffset(100)] public int tmp_out_processed; // Number of bytes processed in tmp_out_buffer
        [FieldOffset(104)] public fixed byte tmp_in_buffer[Constants.ISAL_DEF_MAX_HDR_SIZE]; // Temporary buffer containing data from the input stream.
        [FieldOffset(104 + Constants.ISAL_DEF_MAX_HDR_SIZE)] public fixed byte tmp_out_buffer[2 * Constants.ISAL_DEF_HIST_SIZE + Constants.ISAL_LOOK_AHEAD]; // Temporary buffer containing data from the output stream.
    }

    //[StructLayout(LayoutKind.Explicit)]
    //public unsafe struct inflate_huff_code_large
    //{
    //    [FieldOffset(0)] public fixed uint short_code_lookup [1 << (IsalConstants.ISAL_DECODE_LONG_BITS)];
    //    [FieldOffset(4)] public fixed ushort long_code_lookup [IsalConstants.ISAL_HUFF_CODE_LARGE_LONG_ALIGNED];
    //};

    //[StructLayout(LayoutKind.Explicit)]
    //public unsafe struct inflate_huff_code_small
    //{
    //    [FieldOffset(0)] public fixed ushort short_code_lookup[1 << (IsalConstants.ISAL_DECODE_SHORT_BITS)];
    //    [FieldOffset(2)] public fixed ushort long_code_lookup[IsalConstants.ISAL_HUFF_CODE_SMALL_LONG_ALIGNED];
    //};

    public enum isal_block_state : int
    {
        ISAL_BLOCK_NEW_HDR = 0, /* Just starting a new block */
        ISAL_BLOCK_HDR = 1,     /* In the middle of reading in a block header */
        ISAL_BLOCK_TYPE0 = 2,   /* Decoding a type 0 block */
        ISAL_BLOCK_CODED = 3,   /* Decoding a huffman coded block */
        ISAL_BLOCK_INPUT_DONE = 4,  /* Decompression of input is completed */
        ISAL_BLOCK_FINISH = 5,  /* Decompression of input is completed and all data has been flushed to output */
        ISAL_GZIP_EXTRA_LEN = 6,
        ISAL_GZIP_EXTRA = 7,
        ISAL_GZIP_NAME = 8,
        ISAL_GZIP_COMMENT = 9,
        ISAL_GZIP_HCRC = 10,
        ISAL_ZLIB_DICT = 11,
        ISAL_CHECKSUM_CHECK = 12
    };

    public enum inflate_return_values : int
    {
        ISAL_DECOMP_OK = 0,	        /* No errors encountered while decompressing */
        END_INPUT = 1,	            /* End of input reached */
        OUT_OVERFLOW = 2,	        /* End of output reached */
        NAME_OVERFLOW = 3,	        /* End of gzip name buffer reached */
        COMMENT_OVERFLOW = 4,	    /* End of gzip name buffer reached */
        EXTRA_OVERFLOW = 5,	        /* End of extra buffer reached */
        NEED_DICT = 6,              /* Stream needs a dictionary to continue */
        INVALID_BLOCK = -1,	        /* Invalid deflate block found */
        INVALID_SYMBOL = -2,	    /* Invalid deflate symbol found */
        INVALID_LOOKBACK = -3,	    /* Invalid lookback distance found */
        INVALID_WRAPPER = -4,       /* Invalid gzip/zlib wrapper found */
        UNSUPPORTED_METHOD = -5,    /* Gzip/zlib wrapper specifies unsupported compress method */
        INCORRECT_CHECKSUM = -6     /* Incorrect checksum found */
    }
}