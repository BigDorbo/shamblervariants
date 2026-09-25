using System;
using System.Reflection;
using HarmonyLib;

namespace ShamblerVariants
{
    public static class SVReflect
    {
        public static FieldInfo Field(Type type, string name)
        {
            FieldInfo field = AccessTools.Field(type, name);
            if (field == null)
            {
                throw new MissingFieldException(type.FullName, name);
            }
            return field;
        }
    }
}
