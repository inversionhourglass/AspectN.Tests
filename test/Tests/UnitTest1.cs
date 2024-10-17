using Mono.Cecil;
using System;
using Xunit;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var assembly = AssemblyDefinition.ReadAssembly("Lib1.dll");
        }
    }
}
