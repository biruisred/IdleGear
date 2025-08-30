using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.Pool;

namespace BiruisredEngine
{
    public static class ExtReflection
    {
        public static List<Type> GetAllType(Type type)
        {
            var allType = new List<Type>();
            IterateTypeInAppDomainAssemblies(x =>
            {
                if (x.IsSubclassOf(type)) allType.Add(x);
            });
            return allType;
        }

        public static List<Type> GetAllType<T>()
        {
            return GetAllType(typeof(T));
        }

        public static FieldInfo[] GetFieldInfosIncludingBaseClasses(this Type type, BindingFlags bindingFlags)
        {
            return type.GetFieldInfosIncludingBaseClasses(bindingFlags, typeof(object));
        }

        public static FieldInfo[] GetFieldInfosIncludingBaseClasses(this Type type, BindingFlags bindingFlags,
            Type maxIterate)
        {
            var fields1 = type.GetFields(bindingFlags);
            if (type.BaseType == maxIterate)
                return fields1;
            var targetType = type;
            var comparer = new FieldInfoComparer();
            var source = new HashSet<FieldInfo>(fields1, comparer);
            for (; targetType != maxIterate; targetType = targetType.BaseType)
            {
                var fields2 = targetType.GetFields(bindingFlags);
                source.UnionWith(fields2);
            }

            return source.ToArray();
        }

        public static bool TryGetCustomAttribute<T>(this MemberInfo memberInfo, out T attribute)
            where T : Attribute
        {
            attribute = null;
            if (memberInfo.GetCustomAttribute<T>() == null) return false;
            attribute = memberInfo.GetCustomAttribute<T>();
            return true;
        }

        public static Dictionary<MethodInfo, T> GetStaticMethodWithAttribute<T>() where T : Attribute
        {
            var result = new Dictionary<MethodInfo, T>();
            IterateMethodInAppDomainAssemblies(method =>
            {
                if (method.TryGetCustomAttribute<T>(out var attribute))
                {
                    result.Add(method, attribute);
                }
            });
            return result;
        }

        public static Type[] GetTypeWithAttribute<T>() where T : Attribute
        {
            var list = GenericPool<List<Type>>.Get();
            list.Clear();

            IterateTypeInAppDomainAssemblies(x =>
            {
                if (x.TryGetCustomAttribute<T>(out _)) list.Add(x);
            });
            var result = list.ToArray();

            list.Clear();
            GenericPool<List<Type>>.Release(list);
            return result;
        }

        public static void IterateTypeInAppDomainAssemblies(Action<Type> action)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                var length = types.Length;
                for (var i = 0; i < length; ++i)
                {
                    action.Invoke(types[i]);
                }
            }
        }

        public static void IterateType(this AppDomain appDomain, Action<Type> action)
        {
            var assemblies = appDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                var length = types.Length;
                for (var i = 0; i < length; ++i)
                {
                    action.Invoke(types[i]);
                }
            }
        }

        public static void IterateType<T>(this AppDomain appDomain, Action<Type> action)
        {
            var assemblies = appDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                var length = types.Length;
                for (var i = 0; i < length; ++i)
                {
                    if (!types[i].IsSubclassOf(typeof(T))) continue;
                    action.Invoke(types[i]);
                }
            }
        }

        public static void IterateType<T>(this AppDomain appDomain, Action<int, Type> action)
        {
            var assemblies = appDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                var length = types.Length;
                for (var i = 0; i < length; ++i)
                {
                    if (!types[i].IsSubclassOf(typeof(T))) continue;
                    action.Invoke(i, types[i]);
                }
            }
        }

        public static void IterateMethodInAppDomainAssemblies(Action<MethodInfo> action)
        {
            IterateTypeInAppDomainAssemblies(type =>
            {
                var methods = type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                var lenght = methods.Length;
                for (var i = 0; i < lenght; i++)
                {
                    action.Invoke(methods[i]);
                }
            });
        }

        public class FieldInfoComparer : IEqualityComparer<FieldInfo>
        {
            public bool Equals(FieldInfo x, FieldInfo y) =>
                y != null && x != null && x.DeclaringType == y.DeclaringType && x.Name == y.Name;
            public int GetHashCode(FieldInfo obj) => obj.Name.GetHashCode() ^ obj.DeclaringType.GetHashCode();
        }
    }
}

