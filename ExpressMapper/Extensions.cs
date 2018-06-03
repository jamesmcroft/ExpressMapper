namespace ExpressMapper
{
    using System;
    using System.Linq;
    using System.Reflection;

    internal static class Extensions
    {
        public static ConstructorInfo GetConstructor(this TypeInfo typeInfo, params Type[] type)
        {
            return typeInfo?.DeclaredConstructors.FirstOrDefault(x => x.GetConstructor(type));
        }

        public static bool GetConstructor(this ConstructorInfo constructorInfo, params Type[] type)
        {
            if (constructorInfo != null)
            {
                ParameterInfo[] parameters = constructorInfo.GetParameters();
                if (parameters.Length == 0 && (type == null || type.Length == 0))
                {
                    return true;
                }

                if (type != null)
                {
                    if (parameters.Length != type.Length)
                    {
                        return false;
                    }

                    bool isConstructor = true;
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (!isConstructor)
                        {
                            break;
                        }

                        if (type[i] == parameters[i].ParameterType)
                        {
                            continue;
                        }

                        isConstructor = false;
                    }

                    return isConstructor;
                }
            }

            return false;
        }

        public static MethodInfo GetMethod(this TypeInfo typeInfo, string methodName, params Type[] type)
        {
            return typeInfo?.DeclaredMethods.FirstOrDefault(x => x.GetMethod(methodName, type));
        }

        public static bool GetMethod(this MethodInfo methodInfo, string methodName, params Type[] type)
        {
            if (methodInfo != null && methodInfo.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase))
            {
                ParameterInfo[] parameters = methodInfo.GetParameters();
                if (parameters.Length == 0 && (type == null || type.Length == 0))
                {
                    return true;
                }

                if (type != null)
                {
                    if (parameters.Length != type.Length)
                    {
                        return false;
                    }

                    bool isMethod = true;
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (!isMethod)
                        {
                            break;
                        }

                        if (type[i] == parameters[i].ParameterType)
                        {
                            continue;
                        }

                        isMethod = false;
                    }

                    return isMethod;
                }
            }

            return false;
        }
    }
}
