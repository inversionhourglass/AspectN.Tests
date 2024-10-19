using Cecil.AspectN;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public static class MethodDefinitionExtensions
    {
        private static readonly Random _Random = new();

        public static TypeDefinition RandomType(this AssemblyDefinition assemblyDef, Func<TypeDefinition, bool>? predicate = null)
        {
            var types = assemblyDef.MainModule.Types.ToArray();
            if (predicate != null)
            {
                types = types.Where(predicate).ToArray();
            }
            var index = _Random.Next(types.Length);
            return types[index];
        }

        public static MethodDefinition RandomMethod(this AssemblyDefinition assemblyDef)
        {
            var typeDef = assemblyDef.RandomType();
            return typeDef.RandomMethod();
        }

        public static MethodDefinition RandomMethod(this TypeDefinition typeDef, Func<MethodDefinition, bool>? predicate = null)
        {
            var methods = typeDef.Methods.ToArray();
            if (predicate != null)
            {
                methods = methods.Where(predicate).ToArray();
            }
            var index = _Random.Next(methods.Length);
            return methods[index];
        }

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
