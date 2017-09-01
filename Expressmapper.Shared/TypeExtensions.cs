using System;
using System.Reflection;

namespace ExpressMapper
{
    public static class TypeExtensions
    {
        public static Type GetInfo(this Type type)
        {
            return type;
        }

        public static ObjMemberTypes GetMemberType(this MemberInfo member)
        {
            if (member is FieldInfo)
                return ObjMemberTypes.Field;
            if (member is ConstructorInfo)
                return ObjMemberTypes.Constructor;
            if (member is PropertyInfo)
                return ObjMemberTypes.Property;
            if (member is EventInfo)
                return ObjMemberTypes.Event;
            if (member is MethodInfo)
                return ObjMemberTypes.Method;

            var typeInfo = member as Type;
            if (!typeInfo.IsPublic && !typeInfo.IsNotPublic)
                return ObjMemberTypes.NestedType;

            return ObjMemberTypes.TypeInfo;
        }
    }
}
