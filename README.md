# ISA_L.PInvoke

[![AppVeyor](https://ci.appveyor.com/api/projects/status/j3ci6s0ebf43ypfa/branch/main?svg=true)](https://ci.appveyor.com/project/Apollo3zehn/isa-l-pinvoke)
[![NuGet](https://img.shields.io/nuget/vpre/Intrinsics.ISA-L.PInvoke.svg?label=Nuget)](https://www.nuget.org/packages/Intrinsics.ISA-L.PInvoke)

This lib provides a very limited collection of P/Invoke signatures for Linux and Windows to enable working with [Intel Intelligent Storage Acceleration Library](https://github.com/intel/isa-l) using .NET.

Currently, only the following features are supported:
- Compression - Fast deflate-compatible data compression ([example 1](TBD), [example 2](https://github.com/intel/isa-l/blob/e53db8563180712ec5f1759ec9d52b844c86fa30/programs/igzip_cli.c#L754-L779))
- De-compression - Fast inflate-compatible data compression ([example 1](https://github.com/Apollo3zehn/ISA-L.PInvoke/blob/main/tests/ISA-L.PInvoke.Tests/PInvokeTests.cs#L15), [example 2](https://github.com/intel/isa-l/blob/e53db8563180712ec5f1759ec9d52b844c86fa30/programs/igzip_cli.c#L921-L944))

---
**NOTE**

Pull requests to expose more P/Invoke signatures or to improve already available functionality are warmly welcomed.

---

further links:
- https://ieeexplore.ieee.org/document/8712745
- https://github.com/intel/isa-l
- https://01.org/intel%C2%AE-storage-acceleration-library-open-source-version/documentation/isa-l-2.28-api-doc*