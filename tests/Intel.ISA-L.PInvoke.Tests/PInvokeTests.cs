using ISA_L.PInvoke;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;
using RuntimeEnvironment = Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment;

namespace Intel.ISA_L.PInvoke.Tests
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
            var nativeBytes = Marshal.AllocHGlobal(Unsafe.SizeOf<inflate_state>());
            var state = new Span<inflate_state>(nativeBytes.ToPointer(), length);

            // Act
            var actual = new byte[expected.Length];

            Isal.isal_inflate_init(nativeBytes);

            fixed (byte* ptrIn = deflatedData.AsSpan(), ptrOut = actual.AsSpan())
            {
                if (state[0].avail_in == 0)
                {
                    state[0].next_in = ptrIn;
                    state[0].avail_in = (uint)deflatedData.Length;
                }

                state[0].next_out = ptrOut;
                state[0].avail_out = (uint)actual.Length + 10;

                var status = Isal.isal_inflate(nativeBytes);

#warning state[0].next_out - state[0].avail_out = actual number of available bytes

                if (status != inflate_return_values.ISAL_DECOMP_OK)
                    throw new Exception($"Error encountered while decompressing: {status}.");
            }

            // Assert
            Assert.True(actual.SequenceEqual(expected));
        }

        public byte[] GetDeflatedData(byte[] expected)
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