using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;
using RuntimeEnvironment = Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment;

namespace ISA_L.PInvoke.Tests
{
    public class PInvokeTests
    {
        [Fact]
        public unsafe void CanInflate()
        {
            // Arrange
            Directory.EnumerateFiles("./runtimes/", "*isa-l.*", SearchOption.AllDirectories).ToList().ForEach(filePath =>
            {
                if (filePath.Contains(RuntimeEnvironment.RuntimeArchitecture))
                {
                    File.Copy(filePath, Path.GetFileName(filePath), true);
                }
            });

            /* prepare deflated data */
            var expected = Enumerable
                .Range(0, 100)
                .Select(value => (byte)value)
                .ToArray();

            var deflatedData = this.GetDeflatedData(expected);

            /* prepare inflate_state */
            var length = Unsafe.SizeOf<inflate_state>();
            var state_ptr = Marshal.AllocHGlobal(Unsafe.SizeOf<inflate_state>());
            new Span<byte>(state_ptr.ToPointer(), length).Fill(0);
            var state = new Span<inflate_state>(state_ptr.ToPointer(), length);

            // Act
            var actual = new byte[expected.Length];

            ISAL.isal_inflate_init(state_ptr);

            var chunkSize = 30; /* to simulate buffered reads */
            var bufferIn = deflatedData.AsSpan();

            fixed (byte* ptrOut = actual)
            {
                while (!bufferIn.IsEmpty)
                {
                    fixed (byte* ptrIn = bufferIn)
                    {
                        if (state[0].avail_in == 0)
                        {
                            state[0].next_in = ptrIn;
                            state[0].avail_in = (uint)bufferIn.Length;
                        }

                        state[0].next_out = ptrOut;
                        state[0].avail_out = (uint)actual.Length;
                    }

                    var status = ISAL.isal_inflate(state_ptr);

                    if (status != inflate_return_values.ISAL_DECOMP_OK)
                        throw new Exception($"Error encountered while decompressing: {status}.");

                    bufferIn = bufferIn.Slice(Math.Min(chunkSize, bufferIn.Length));
                }
            }

            // Assert
            Assert.Equal(isal_block_state.ISAL_BLOCK_FINISH, state[0].block_state);
            Assert.True(actual.SequenceEqual(expected));
        }

        private byte[] GetDeflatedData(byte[] expected)
        {
            using var originalStream = new MemoryStream(expected);
            using var compressedStream = new MemoryStream();

            using (var compressionStream = new DeflateStream(compressedStream, CompressionMode.Compress))
            {
                originalStream.CopyTo(compressionStream);
            }

            return compressedStream.ToArray();
        }
    }
}