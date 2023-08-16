using System.Runtime.InteropServices;

namespace ISA_L.PInvoke
{
    #region Inflate

    // get offset of struct member:
    // printf("offset: %ld\n", (long)offsetof(struct isal_zstate, total_in_start));

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

        [FieldOffset(40)]                                               public inflate_huff_code_large lit_huff_code; // Structure for decoding lit/len symbols.
        [FieldOffset(40 + Constants.SIZE_OF_INFLATE_HUFF_CODE_LARGE)]   public inflate_huff_code_small dist_huff_code; // Structure for decoding dist symbols

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
        [FieldOffset(92 + Constants.HUFF_CODE_OFFSET)] public short wrapper_flag;
        [FieldOffset(94 + Constants.HUFF_CODE_OFFSET)] public short tmp_in_size; // Number of bytes in tmp_in_buffer
        [FieldOffset(96 + Constants.HUFF_CODE_OFFSET)] public int tmp_out_valid; // Number of bytes in tmp_out_buffer
        [FieldOffset(100 + Constants.HUFF_CODE_OFFSET)] public int tmp_out_processed; // Number of bytes processed in tmp_out_buffer
        [FieldOffset(104 + Constants.HUFF_CODE_OFFSET)]                                   public fixed byte tmp_in_buffer[Constants.ISAL_DEF_MAX_HDR_SIZE]; // Temporary buffer containing data from the input stream.
        [FieldOffset(104 + Constants.HUFF_CODE_OFFSET + Constants.ISAL_DEF_MAX_HDR_SIZE)] public fixed byte tmp_out_buffer[2 * Constants.ISAL_DEF_HIST_SIZE + Constants.ISAL_LOOK_AHEAD]; // Temporary buffer containing data from the output stream.
    }

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct inflate_huff_code_large
    {
        [FieldOffset(0)]                                    public fixed uint short_code_lookup[1 << Constants.ISAL_DECODE_LONG_BITS];
        [FieldOffset(1 << (Constants.ISAL_DECODE_LONG_BITS + 2))] public fixed ushort long_code_lookup[Constants.ISAL_HUFF_CODE_LARGE_LONG_ALIGNED];
    };

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct inflate_huff_code_small
    {
        [FieldOffset(0)]                                public fixed ushort short_code_lookup[1 << (Constants.ISAL_DECODE_SHORT_BITS)];
        [FieldOffset(1 << (Constants.ISAL_DECODE_SHORT_BITS + 1))] public fixed ushort long_code_lookup[Constants.ISAL_HUFF_CODE_SMALL_LONG_ALIGNED];
    };

    public enum isal_block_state : int
    {
        ISAL_BLOCK_NEW_HDR = 0,     /* Just starting a new block */
        ISAL_BLOCK_HDR = 1,         /* In the middle of reading in a block header */
        ISAL_BLOCK_TYPE0 = 2,       /* Decoding a type 0 block */
        ISAL_BLOCK_CODED = 3,       /* Decoding a huffman coded block */
        ISAL_BLOCK_INPUT_DONE = 4,  /* Decompression of input is completed */
        ISAL_BLOCK_FINISH = 5,      /* Decompression of input is completed and all data has been flushed to output */
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

    #endregion

    #region Deflate

    public enum isal_zstate_state : int
    {
        ZSTATE_NEW_HDR,             /* Header to be written */
        ZSTATE_HDR,	                /* Header state */
        ZSTATE_CREATE_HDR,          /* Header to be created */
        ZSTATE_BODY,	            /* Body state */
        ZSTATE_FLUSH_READ_BUFFER,   /* Flush buffer */
        ZSTATE_FLUSH_ICF_BUFFER,
        ZSTATE_TYPE0_HDR,           /* Type0 block header to be written */
        ZSTATE_TYPE0_BODY,          /* Type0 block body to be written */
        ZSTATE_SYNC_FLUSH,          /* Write sync flush block */
        ZSTATE_FLUSH_WRITE_BUFFER,  /* Flush bitbuf */
        ZSTATE_TRL,	                /* Trailer state */
        ZSTATE_END,	                /* End state */
        ZSTATE_TMP_NEW_HDR,         /* Temporary Header to be written */
        ZSTATE_TMP_HDR,	            /* Temporary Header state */
        ZSTATE_TMP_CREATE_HDR,      /* Temporary Header to be created state */
        ZSTATE_TMP_BODY,	        /* Temporary Body state */
        ZSTATE_TMP_FLUSH_READ_BUFFER, /* Flush buffer */
        ZSTATE_TMP_FLUSH_ICF_BUFFER,
        ZSTATE_TMP_TYPE0_HDR,       /* Temporary Type0 block header to be written */
        ZSTATE_TMP_TYPE0_BODY,      /* Temporary Type0 block body to be written */
        ZSTATE_TMP_SYNC_FLUSH,      /* Write sync flush block */
        ZSTATE_TMP_FLUSH_WRITE_BUFFER, /* Flush bitbuf */
        ZSTATE_TMP_TRL,	            /* Temporary Trailer state */
        ZSTATE_TMP_END	            /* Temporary End state */
    };

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct BitBuf2 
    {
        [FieldOffset(0)] public ulong m_bits;       /* bits in the bit buffer */
        [FieldOffset(8)] public uint m_bit_count;   /* number of valid bits in the bit buffer */
        [FieldOffset(16)] public byte* m_out_buf;	/* current index of buffer to write to */
        [FieldOffset(24)] public byte* m_out_end;	/* end of buffer to write to */
        [FieldOffset(32)] public byte* m_out_start;	/* start of buffer to write to */
    };

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct isal_zstate 
    {
        [FieldOffset(0)] public uint total_in_start; /* Not used, may be replaced with something else */
        [FieldOffset(4)] public uint block_next; /* Start of current deflate block in the input */
        [FieldOffset(8)] public uint block_end; /* End of current deflate block in the input */
        [FieldOffset(12)] public uint dist_mask; /* Distance mask used. */
        [FieldOffset(16)] public uint hash_mask;
        [FieldOffset(20)] public isal_zstate_state state; /* Current state in processing the data stream */
        [FieldOffset(24)] public BitBuf2 bitbuf; /* Bit Buffer */
        [FieldOffset(64)] public uint crc; /* Current checksum without finalize step if any (adler) */
        [FieldOffset(68)] public byte has_wrap_hdr; /* keeps track of wrapper header */
        [FieldOffset(69)] public byte has_eob_hdr; /* keeps track of eob hdr (with BFINAL set) */
        [FieldOffset(70)] public byte has_eob; /* keeps track of eob on the last deflate block */
        [FieldOffset(71)] public byte has_hist; /* flag to track if there is match history */
        [FieldOffset(72)] public ushort has_level_buf_init; /* flag to track if user supplied memory has been initialized. */
        [FieldOffset(76)] public uint count; /* used for partial header/trailer writes */
        [FieldOffset(80)] public fixed byte tmp_out_buff[16]; /* temporary array */
        [FieldOffset(96)] public uint tmp_out_start; /* temporary variable */
        [FieldOffset(100)] public uint tmp_out_end; /* temporary variable */
        [FieldOffset(104)] public uint b_bytes_valid; /* number of valid bytes in buffer */
        [FieldOffset(108)] public uint b_bytes_processed; /* number of bytes processed in buffer */
        [FieldOffset(112)] public fixed byte buffer[2 * Constants.IGZIP_HIST_SIZE + Constants.ISAL_LOOK_AHEAD];	/* Internal buffer */

        /* Stream should be setup such that the head is cache aligned*/
        [FieldOffset(112 + 2 * Constants.IGZIP_HIST_SIZE + Constants.ISAL_LOOK_AHEAD)] public fixed ushort head[Constants.IGZIP_LVL0_HASH_SIZE];	/* Hash array */
    }

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct isal_hufftables 
    {
        [FieldOffset(0)] public fixed byte deflate_hdr[Constants.ISAL_DEF_MAX_HDR_SIZE]; /* deflate huffman tree header */
        [FieldOffset(328)] public uint deflate_hdr_count; /* Number of whole bytes in deflate_huff_hdr */
        [FieldOffset(332)] public uint deflate_hdr_extra_bits; /* Number of bits in the partial byte in header */
        [FieldOffset(336)] public fixed uint dist_table[Constants.IGZIP_DIST_TABLE_SIZE]; /* bits 4:0 are the code length, bits 31:5 are the code */
        [FieldOffset(344)] public fixed uint len_table[Constants.IGZIP_LEN_TABLE_SIZE]; /* bits 4:0 are the code length, bits 31:5 are the code */
        [FieldOffset(1368)] public fixed ushort lit_table[Constants.IGZIP_LIT_TABLE_SIZE]; /* literal code */
        [FieldOffset(1882)] public fixed byte lit_table_sizes[Constants.IGZIP_LIT_TABLE_SIZE]; /* literal code length */
        [FieldOffset(2140)] public fixed ushort dcodes[30 - Constants.IGZIP_DECODE_OFFSET]; /* distance code */
        [FieldOffset(2200)] public fixed byte dcodes_sizes[30 - Constants.IGZIP_DECODE_OFFSET]; /* distance code length */
    };

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct isal_zstream 
    {
        [FieldOffset(0)] public byte* next_in;                  /* Next input byte */
        [FieldOffset(8)] public uint avail_in;                  /* number of bytes available at next_in */
        [FieldOffset(12)] public uint total_in;	                /* total number of bytes read so far */

        [FieldOffset(16)] public byte* next_out;                /* Next output byte */
        [FieldOffset(24)] public uint avail_out;                /* number of bytes available at next_out */
        [FieldOffset(28)] public uint total_out;                /* total number of bytes written so far */

        [FieldOffset(32)] public isal_hufftables* hufftables;   /* Huffman encoding used when compressing */
        [FieldOffset(40)] public uint level;                    /* Compression level to use */
        [FieldOffset(44)] public uint level_buf_size;           /* Size of level_buf */
        [FieldOffset(48)] public byte* level_buf;               /* User allocated buffer required for different compression levels */
        [FieldOffset(56)] public ushort end_of_stream;          /* non-zero if this is the last input buffer */
        [FieldOffset(58)] public ushort flush;                  /* Flush type can be NO_FLUSH, SYNC_FLUSH or FULL_FLUSH */
        [FieldOffset(60)] public ushort gzip_flag;              /* Indicate if gzip compression is to be performed */
        [FieldOffset(62)] public ushort hist_bits;              /* Log base 2 of maximum lookback distance, 0 is use default */
        [FieldOffset(64)] public isal_zstate internal_state;    /* Internal state for this stream */
    };

    #endregion
}