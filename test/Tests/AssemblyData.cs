using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class AssemblyData : IEnumerable<object[]>
    {
        private static AssemblyDefinition[] _AssemblyDefs;

        static AssemblyData()
        {
            _AssemblyDefs = ReadAssemblies("Abp", "Autofac", "FreeSql", "Newtonsoft.Json", "Polly", "Serilog", "StackExchange.Redis").ToArray();
        }

        private static IEnumerable<AssemblyDefinition> ReadAssemblies(params string[] names)
        {
            foreach (var name in names)
            {
                yield return AssemblyDefinition.ReadAssembly($"{name}.dll");
            }
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (var assemblyDef in _AssemblyDefs)
            {
                yield return [assemblyDef, true];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
