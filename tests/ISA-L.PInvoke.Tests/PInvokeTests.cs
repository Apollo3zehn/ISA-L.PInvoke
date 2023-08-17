using System;
using System.Buffers;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Nexus.Extensibility.Tests;
using Xunit;

namespace ISA_L.PInvoke.Tests;

public class PInvokeTests : IClassFixture<PInvokeFixture>
{
    [Fact]
    public unsafe void CanInflate()
    {
        // Arrange
        
        /* prepare inflate_state */
        var length = Unsafe.SizeOf<inflate_state>();
        var statePtr = Marshal.AllocHGlobal(length);

        try
        {
            var stateSpan = new Span<inflate_state>(statePtr.ToPointer(), 1);
            stateSpan.Clear();
            ref inflate_state state = ref stateSpan[0];

            /* prepare deflated data */
            var expected = Enumerable
                .Range(0, 100)
                .Select(value => (byte)value)
                .ToArray();

            var deflatedData = DeflateData(expected);

            /* prepare buffers */
            var source = deflatedData.AsSpan();
            var target = new byte[expected.Length];

            /* to simulate chunked reads */
            var chunkSize = 30;

            // Act
            ISAL.isal_inflate_init(statePtr);

            fixed (byte* _ptrIn = source, ptrOut = target)
            {
                byte* ptrIn = _ptrIn;

                state.next_out = ptrOut;
                state.avail_out = (uint)target.Length;

                do
                {
                    var currentSourceLength = Math.Min(chunkSize, source.Length);

                    if (state.avail_in == 0)
                    {
                        state.next_in = ptrIn;
                        state.avail_in = (uint)currentSourceLength;
                    }

                    var status = ISAL.isal_inflate(statePtr);

                    ptrIn = state.next_in;

                    if (status != inflate_return_values.ISAL_DECOMP_OK)
                        throw new Exception($"Error encountered while decompressing: {status}.");

                    source = source.Slice(currentSourceLength);
                }
                while (
                    state.block_state != isal_block_state.ISAL_BLOCK_FINISH &&
                    !(source.IsEmpty || state.avail_out == 0)
                );
            }

            // Assert
            Assert.Equal(isal_block_state.ISAL_BLOCK_FINISH, state.block_state);
            Assert.True(expected.SequenceEqual(target));
        }
        finally
        {
            Marshal.FreeHGlobal(statePtr);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public unsafe void CanDeflate(uint level)
    {
        // Arrange
        /* prepare isal_zstream */
        var length = Unsafe.SizeOf<isal_zstream>();
        var streamPtr = Marshal.AllocHGlobal(length);

        ISAL.isal_deflate_init(streamPtr);

        try
        {
            var streamSpan = new Span<isal_zstream>(streamPtr.ToPointer(), 1);
            streamSpan.Clear();
            ref isal_zstream stream = ref streamSpan[0];

            /* prepare inflated data */
            var inflatedData = Enumerable
                .Range(0, 100)
                .Select(value => (byte)value)
                .ToArray();

            /* prepare buffers */
            var source = inflatedData.AsSpan();
            var target = new byte[1000];

            /* to simulate chunked reads */
            var chunkSize = 30; 

            // Act
            ISAL.isal_deflate_init(streamPtr);

            var levelBufferSize = level switch
            {
                0 => Constants.ISAL_DEF_LVL0_DEFAULT,
                1 => Constants.ISAL_DEF_LVL1_DEFAULT,
                2 => Constants.ISAL_DEF_LVL2_DEFAULT,
                3 => Constants.ISAL_DEF_LVL3_DEFAULT,
                _ => throw new Exception($"The level {level} is not supported.")
            };

            using var memoryOwner = MemoryPool<byte>.Shared.Rent(levelBufferSize);
            var levelBuffer = memoryOwner.Memory.Span.Slice(0, levelBufferSize);

            fixed (byte* _ptrIn = source, ptrOut = target, ptrLevel = levelBuffer)
            {
                byte* ptrIn = _ptrIn;

                stream.level = level;
                stream.level_buf = ptrLevel;
                stream.level_buf_size = (uint)levelBufferSize;

                stream.next_out = ptrOut;
                stream.avail_out = (uint)target.Length;

                do
                {
                    var currentSourceLength = Math.Min(chunkSize, source.Length);

                    if (stream.avail_in == 0)
                    {
                        stream.next_in = ptrIn;
                        stream.avail_in = (uint)currentSourceLength;
                        stream.end_of_stream = (ushort)(currentSourceLength == source.Length ? 1 : 0);
                    }

                    var status = ISAL.isal_deflate(streamPtr);

                    ptrIn = stream.next_in;

                    if (status != inflate_return_values.ISAL_DECOMP_OK)
                        throw new Exception($"Error encountered while compressing: {status}.");

                    source = source.Slice(currentSourceLength);
                }
                while (
                    !(source.IsEmpty || stream.avail_out == 0)
                );

                var totalTargetLength = stream.next_out - ptrOut;

                // Assert
                Assert.NotEqual(0, totalTargetLength);

                /* deflate data */
                var slicedTarget = target.AsSpan(0..(int)totalTargetLength);
                var actual = InflateData(slicedTarget.ToArray());

                Assert.True(inflatedData.SequenceEqual(actual));
            }
        }
        finally
        {
            Marshal.FreeHGlobal(streamPtr);
        }
    }

    private byte[] DeflateData(byte[] data)
    {
        using var originalStream = new MemoryStream(data);
        using var compressedStream = new MemoryStream();

        using (var compressionStream = new DeflateStream(compressedStream, CompressionMode.Compress))
        {
            originalStream.CopyTo(compressionStream);
        }

        return compressedStream.ToArray();
    }

    private byte[] InflateData(byte[] data)
    {
        using var originalStream = new MemoryStream(data);
        using var decompressedStream = new MemoryStream();

        using (var decompressionStream = new DeflateStream(originalStream, CompressionMode.Decompress))
        {
            decompressionStream.CopyTo(decompressedStream);
        }

        return decompressedStream.ToArray();
    }
}