using Mono.Cecil;
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
            var pattern = "cctor(*(..))";
            var patternMatches = assemblyDef.FindMethods(pattern, compositeAccessibility).ToArray();
            var lambdaMatches = assemblyDef.FindMethods(md => md.IsConstructor && md.IsStatic).ToArray();

            Assert.NotEmpty(patternMatches);
            Assert.Equal(lambdaMatches, patternMatches);
        }
    }
}
