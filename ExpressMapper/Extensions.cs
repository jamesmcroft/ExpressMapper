namespace ExpressMapper
{
    using System;
    using System.Reflection;

    internal static class Extensions
    {
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
    }
}
