using Mono.Cecil;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Cases
{
    public class SyntaxTests
    {
        [Theory]
        [ClassData(typeof(AssemblyData))]
        public void AsyncNullTest(AssemblyDefinition assemblyDef, bool compositeAccessibility)
        {
            var pattern = "execution(async null *(..))";
            var patternMatches = assemblyDef.FindMethods(pattern, compositeAccessibility).ToArray();
            var lambdaMatches = assemblyDef.FindMethods(md => md.ReturnType.IsAny(typeof(Task), typeof(ValueTask))).ToArray();

            Assert.Equal(lambdaMatches, patternMatches);
        }
    }
}
