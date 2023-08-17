using System.IO;
using Microsoft.DotNet.PlatformAbstractions;

namespace Nexus.Extensibility.Tests;

public class PInvokeFixture
{
    public PInvokeFixture()
    {
        var filePaths = Directory
            .EnumerateFiles("./runtimes/", "*isa-l.*", SearchOption.AllDirectories);
            
        foreach (var filePath in filePaths)
        {
            if (filePath.Contains(RuntimeEnvironment.RuntimeArchitecture))
                File.Copy(filePath, Path.GetFileName(filePath), true);
        };
    }
}