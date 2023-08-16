using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;
using RuntimeEnvironment = Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment;

namespace ISA_L.PInvoke.Tests;

public class PInvokeTests
{
    // [Fact]
    // public unsafe void CanInflate()
    // {
    //     // Arrange
    //     Directory.EnumerateFiles("./runtimes/", "*isa-l.*", SearchOption.AllDirectories).ToList().ForEach(filePath =>
    //     {
    //         if (filePath.Contains(RuntimeEnvironment.RuntimeArchitecture))
    //             File.Copy(filePath, Path.GetFileName(filePath), true);
    //     });

    //     /* prepare inflate_state */
    //     var length = Unsafe.SizeOf<inflate_state>();
    //     var statePtr = Marshal.AllocHGlobal(length);
    //     new Span<byte>(statePtr.ToPointer(), length).Fill(0);
    //     var stateSpan = new Span<inflate_state>(statePtr.ToPointer(), 1);
    //     ref inflate_state state = ref stateSpan[0];

    //     /* prepare deflated data */
    //     var expected = Enumerable
    //         .Range(0, 100)
    //         .Select(value => (byte)value)
    //         .ToArray();

    //     var deflatedData = DeflateData(expected);

    //     /* prepare buffers */
    //     var source = deflatedData.AsSpan();
    //     var target = new byte[expected.Length];

    //     /* to simulate chunked reads */
    //     var chunkSize = 30; 

    //     // Act
    //     ISAL.isal_inflate_init(statePtr);

    //     fixed (byte* _ptrIn = source, ptrOut = target)
    //     {
    //         byte* ptrIn = _ptrIn;

    //         state.next_out = ptrOut;
    //         state.avail_out = (uint)target.Length;

    //         do
    //         {
    //             var currentSourceLength = Math.Min(chunkSize, source.Length);

    //             if (state.avail_in == 0)
    //             {
    //                 state.next_in = ptrIn;
    //                 state.avail_in = (uint)currentSourceLength;
    //             }

    //             var status = ISAL.isal_inflate(statePtr);

    //             ptrIn = state.next_in;

    //             if (status != inflate_return_values.ISAL_DECOMP_OK)
    //                 throw new Exception($"Error encountered while decompressing: {status}.");

    //             source = source.Slice(currentSourceLength);
    //         }
    //         while (
    //             state.block_state != isal_block_state.ISAL_BLOCK_FINISH &&
    //             !(source.IsEmpty || state.avail_out == 0)
    //         );
    //     }

    //     // Assert
    //     Assert.Equal(isal_block_state.ISAL_BLOCK_FINISH, state.block_state);
    //     Assert.True(expected.SequenceEqual(target));
    // }

    [Fact]
    public unsafe void CanDeflate()
    {
        // Arrange
        Directory.EnumerateFiles("./runtimes/", "*isa-l.*", SearchOption.AllDirectories).ToList().ForEach(filePath =>
        {
            if (filePath.Contains(RuntimeEnvironment.RuntimeArchitecture))
                File.Copy(filePath, Path.GetFileName(filePath), true);
        });

        /* prepare inflate_state */
        var length = Unsafe.SizeOf<isal_zstream>();
        var streamPtr = Marshal.AllocHGlobal(length);
        new Span<byte>(streamPtr.ToPointer(), length).Fill(0);
        var streamSpan = new Span<isal_zstream>(streamPtr.ToPointer(), 1);
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

        // // Act
        ISAL.isal_deflate_init(streamPtr);

        stream.level = 1;

        fixed (byte* _ptrIn = source, ptrOut = target)
        {
            byte* ptrIn = _ptrIn;

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
            var actual = DeflateData(slicedTarget.ToArray());

            foreach (var item in slicedTarget)
            {
                File.AppendAllText("/home/vincent/Downloads/abc/abc.log", item + "\n");   
            }

            Assert.True(source.SequenceEqual(actual));
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
}