using Mono.Cecil;
using System;
using System.Linq;

namespace Tests
{
    public static class IsExtensions
    {
        public static bool Is(this TypeReference typeRef, Type type)
        {
            return typeRef.FullName == type.FullName;
        }

        public static bool IsAny(this TypeReference typeRef, params Type[] types)
        {
            return types.Any(typeRef.Is);
        }
    }
}
