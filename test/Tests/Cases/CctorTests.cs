using Mono.Cecil;
using Mono.Cecil.Rocks;
using System.Linq;
using Xunit;

namespace Tests.Cases
{
    public class CctorTests
    {
        [Theory]
        [ClassData(typeof(AssemblyData))]
        public void AnyCctorTest(AssemblyDefinition assemblyDef, bool compositeAccessibility)
        {
            var pattern = "cctor(*)";
            var patternMatches = assemblyDef.FindMethods(pattern, compositeAccessibility).ToArray();
            var lambdaMatches = assemblyDef.FindMethods(md => md.IsConstructor && md.IsStatic).ToArray();

            Assert.NotEmpty(patternMatches);
            Assert.Equal(lambdaMatches, patternMatches);
        }

        [Theory]
        [ClassData(typeof(AssemblyData))]
        public void SpecificDeclaringTest(AssemblyDefinition assemblyDef, bool compositeAccessibility)
        {
            var expectedMethod = assemblyDef.RandomType(static td => !td.HasGenericParameters && !td.IsNested && td.GetStaticConstructor() != null).GetStaticConstructor();
            var pattern = $"cctor({expectedMethod.DeclaringType.FullName})";
            var patternMatches = assemblyDef.FindMethods(pattern, compositeAccessibility).ToArray();

            Assert.Single(patternMatches);
            Assert.Equal(expectedMethod, patternMatches[0]);
        }
    }
}
