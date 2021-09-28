# ISA_L.PInvoke

[![AppVeyor](https://ci.appveyor.com/api/projects/status/id3mrt9m4j2usal2/branch/main?svg=true)](https://ci.appveyor.com/project/Apollo3zehn/isa-l-pinvoke)
[![NuGet](https://img.shields.io/nuget/vpre/ISA_L.PInvoke.svg?label=Nuget)](https://www.nuget.org/packages/ISA_L.PInvoke)

This lib provides a very limited collection of P/Invoke signatures for Linux and Windows to enable working with [Intel Intelligent Storage Acceleration Library](https://github.com/intel/isa-l) using .NET.

Currently, only the following features are supported:
- De-compression - Fast inflate-compatible data compression ([example 1](https://github.com/Apollo3zehn/ISA-L.PInvoke/blob/main/tests/ISA-L.PInvoke.Tests/PInvokeTests.cs#L16), [example 2](https://github.com/intel/isa-l/blob/f980b366556d785ea7701a529c6d1c3b33d05502/programs/igzip_cli.c#L921-L944))

---
**NOTE**

Pull requests to expose more P/Invoke signatures or to improve already available functionality are warmly welcomed.

---

*based on: https://01.org/intel%C2%AE-storage-acceleration-library-open-source-version/documentation/isa-l-2.28-api-doc*