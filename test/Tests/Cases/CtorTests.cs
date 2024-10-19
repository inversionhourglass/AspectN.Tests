using Mono.Cecil;
using Mono.Cecil.Rocks;
using System.Linq;
using Xunit;

namespace Tests.Cases
{
    public class CtorTests
    {
        [Theory]
        [ClassData(typeof(AssemblyData))]
        public void AnyTest(AssemblyDefinition assemblyDef, bool compositeAccessibility)
        {
            var pattern = "ctor(*(..))";
            var patternMatches = assemblyDef.FindMethods(pattern, compositeAccessibility).ToArray();
            var lambdaMatches = assemblyDef.FindMethods(static md => md.IsConstructor && !md.IsStatic).ToArray();

            Assert.NotEmpty(patternMatches);
            Assert.Equal(lambdaMatches, patternMatches);
        }

        [Theory]
        [ClassData(typeof(AssemblyData))]
        public void NoneParasTest(AssemblyDefinition assemblyDef, bool compositeAccessibility)
        {
            var pattern = "ctor(*())";
            var patternMatches = assemblyDef.FindMethods(pattern, compositeAccessibility).ToArray();
            var lambdaMatches = assemblyDef.FindMethods(static md => md.IsConstructor && !md.IsStatic && md.Parameters.Count == 0).ToArray();

            Assert.NotEmpty(patternMatches);
            Assert.Equal(lambdaMatches, patternMatches);
        }

        [Theory]
        [ClassData(typeof(AssemblyData))]
        public void AnyP1Test(AssemblyDefinition assemblyDef, bool compositeAccessibility)
        {
            var pattern = "ctor(*(*))";
            var patternMatches = assemblyDef.FindMethods(pattern, compositeAccessibility).ToArray();
            var lambdaMatches = assemblyDef.FindMethods(static md => md.IsConstructor && !md.IsStatic && md.Parameters.Count == 1).ToArray();

            Assert.NotEmpty(patternMatches);
            Assert.Equal(lambdaMatches, patternMatches);
        }

        [Theory]
        [ClassData(typeof(AssemblyData))]
        public void AnyP2Test(AssemblyDefinition assemblyDef, bool compositeAccessibility)
        {
            var pattern = "ctor(*(*, *))";
            var patternMatches = assemblyDef.FindMethods(pattern, compositeAccessibility).ToArray();
            var lambdaMatches = assemblyDef.FindMethods(static md => md.IsConstructor && !md.IsStatic && md.Parameters.Count == 2).ToArray();

            Assert.NotEmpty(patternMatches);
            Assert.Equal(lambdaMatches, patternMatches);
        }

        [Theory]
        [ClassData(typeof(AssemblyData))]
        public void P1IntP2AnyTest(AssemblyDefinition assemblyDef, bool compositeAccessibility)
        {
            var pattern = "ctor(*(string, *))";
            var patternMatches = assemblyDef.FindMethods(pattern, compositeAccessibility).ToArray();
            var lambdaMatches = assemblyDef.FindMethods(static md => md.IsConstructor && !md.IsStatic && md.Parameters.Count == 2 && md.Parameters[0].ParameterType.Is(typeof(string))).ToArray();

            Assert.NotEmpty(patternMatches);
            var e1 = lambdaMatches.Except(patternMatches).ToArray();
            var e2 = patternMatches.Except(lambdaMatches).ToArray();
            Assert.Equal(lambdaMatches, patternMatches);
        }

        [Theory]
        [ClassData(typeof(AssemblyData))]
        public void NonGenericTest(AssemblyDefinition assemblyDef, bool compositeAccessibility)
        {
            var pattern = "ctor(*<!>(..))";
            var patternMatches = assemblyDef.FindMethods(pattern, compositeAccessibility).ToArray();
            var lambdaMatches = assemblyDef.FindMethods(static md => md.IsConstructor && !md.IsStatic && !md.DeclaringType.IsNested && !md.DeclaringType.HasGenericParameters).ToArray();

            Assert.NotEmpty(patternMatches);
            Assert.Equal(lambdaMatches, patternMatches);
        }

        [Theory]
        [ClassData(typeof(AssemblyData))]
        public void AnyG1Test(AssemblyDefinition assemblyDef, bool compositeAccessibility)
        {
            var pattern = "ctor(*<>(..))";
            var patternMatches = assemblyDef.FindMethods(pattern, compositeAccessibility).ToArray();
            var lambdaMatches = assemblyDef.FindMethods(static md => md.IsConstructor && !md.IsStatic && !md.DeclaringType.IsNested && md.DeclaringType.GenericParameters.Count == 1).ToArray();

            Assert.NotEmpty(patternMatches);
            Assert.Equal(lambdaMatches, patternMatches);
        }

        [Theory]
        [ClassData(typeof(AssemblyData))]
        public void AnyG2Test(AssemblyDefinition assemblyDef, bool compositeAccessibility)
        {
            var pattern = "ctor(*<,>(..))";
            var patternMatches = assemblyDef.FindMethods(pattern, compositeAccessibility).ToArray();
            var lambdaMatches = assemblyDef.FindMethods(static md => md.IsConstructor && !md.IsStatic && !md.DeclaringType.IsNested && md.DeclaringType.GenericParameters.Count == 2).ToArray();

            Assert.NotEmpty(patternMatches);
            Assert.Equal(lambdaMatches, patternMatches);
        }

        [Theory]
        [ClassData(typeof(AssemblyData))]
        public void SpecificDeclaringTest(AssemblyDefinition assemblyDef, bool compositeAccessibility)
        {
            var expectedMethod = assemblyDef.RandomType(static td => !td.HasGenericParameters && !td.IsNested && td.GetConstructors().Any(static x => !x.IsStatic && x.Parameters.Count == 0)).GetConstructors().First(static x => !x.IsStatic && x.Parameters.Count == 0);
            var pattern = $"ctor({expectedMethod.DeclaringType.FullName}<!>())";
            var patternMatches = assemblyDef.FindMethods(pattern, compositeAccessibility).ToArray();

            Assert.Single(patternMatches);
            Assert.Equal(expectedMethod, patternMatches[0]);
        }
    }
}
