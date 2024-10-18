using Cecil.AspectN;
using Mono.Cecil;
using System;
using System.Collections.Generic;

namespace Tests
{
    public static class MethodDefinitionExtensions
    {
        public static IEnumerable<MethodDefinition> FindMethods(this AssemblyDefinition assemblyDef, string pattern, bool compositeAccessibility = true)
        {
            var matcher = PatternParser.Parse(pattern);

            return assemblyDef.FindMethods(PatternMatch);

            bool PatternMatch(MethodDefinition methodDef)
            {
                var signature = SignatureParser.ParseMethod(methodDef, compositeAccessibility);
                return matcher.IsMatch(signature);
            }
        }

        public static IEnumerable<MethodDefinition> FindMethods(this AssemblyDefinition assemblyDef, Func<MethodDefinition, bool> predicate)
        {
            foreach (var typeDef in assemblyDef.MainModule.Types)
            {
                foreach (var methodDef in FindMethods(typeDef, predicate))
                {
                    yield return methodDef;
                }
            }
        }

        private static IEnumerable<MethodDefinition> FindMethods(TypeDefinition typeDef, Func<MethodDefinition, bool> predicate)
        {
            foreach (var methodDef in typeDef.Methods)
            {
                if (predicate(methodDef))
                {
                    yield return methodDef;
                }
            }

            foreach (var nestedTypeDef in typeDef.NestedTypes)
            {
                foreach (var method in FindMethods(nestedTypeDef, predicate))
                {
                    yield return method;
                }
            }
        }
    }
}
