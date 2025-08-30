using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace BiruisredEngine
{
#if UNITY_EDITOR
    /// <summary>
    /// Extension methods for <see cref="SerializedObject"/> and <see cref="SerializedProperty"/>
    /// to simplify getting and setting values through reflection in the Unity Editor.
    /// </summary>
    public static class ExtensionSerializedProperty
    {
        /// <summary>
        /// Regex to detect array element indices in property paths (e.g. "myList[0]").
        /// </summary>
        private static readonly Regex _rgx = new(@"\[\d+\]", RegexOptions.Compiled);

        /// <summary>
        /// Sets the value of a field/property on a <see cref="ScriptableObject"/> and applies the changes.
        /// </summary>
        public static void SetValue<T>(this ScriptableObject so, string name, T value)
        {
            var serializeObject = new SerializedObject(so);
            serializeObject.SetValue(name, value);
        }

        /// <summary>
        /// Sets the value of a field/property on a <see cref="ScriptableObject"/> 
        /// without applying the serialized changes immediately.
        /// </summary>
        public static void SetValueWithoutApply<T>(this ScriptableObject so, string name, T value)
        {
            var serializeObject = new SerializedObject(so);
            serializeObject.SetValueWithoutApply(name, value);
        }

        /// <summary>
        /// Gets the value of a field/property from a <see cref="ScriptableObject"/>.
        /// </summary>
        public static T GetValue<T>(this ScriptableObject so, string name)
        {
            var serializeObject = new SerializedObject(so);
            return serializeObject.GetValue<T>(name);
        }

        /// <summary>
        /// Sets a serialized field/property value and applies changes.
        /// </summary>
        public static void SetValue<T>(this SerializedObject serializedObject, string name, T value)
        {
            SetValueWithoutApply(serializedObject, name, value);
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Sets a serialized field/property value without applying changes immediately.
        /// </summary>
        public static void SetValueWithoutApply<T>(this SerializedObject serializedObject, string name, T value)
        {
            var prop = serializedObject.FindProperty(name);
            if (prop == null)
            {
                Debug.LogError($"Property {name} not found on {serializedObject.targetObject.GetType()}");
                return;
            }

            prop.SetValue(value);
        }

        /// <summary>
        /// Gets a serialized field/property value from a <see cref="SerializedObject"/>.
        /// </summary>
        public static T GetValue<T>(this SerializedObject serializedObject, string name)
        {
            var prop = serializedObject.FindProperty(name);
            if (prop == null)
            {
                Debug.LogError($"Property {name} not found on {serializedObject.targetObject.GetType()}");
                return default;
            }

            return prop.GetValue<T>();
        }

        /// <summary>
        /// Gets the underlying C# value of a <see cref="SerializedProperty"/> using reflection.
        /// </summary>
        public static T GetValue<T>(this SerializedProperty property)
        {
            object obj = property.serializedObject.targetObject;
            string path = property.propertyPath.Replace(".Array.data", "");
            string[] fieldStructure = path.Split('.');

            // Traverse through nested fields (including arrays/lists)
            for (int i = 0; i < fieldStructure.Length; i++)
            {
                if (fieldStructure[i].Contains("["))
                {
                    int index = Convert.ToInt32(new string(fieldStructure[i].Where(char.IsDigit).ToArray()));
                    obj = GetFieldValueWithIndex(_rgx.Replace(fieldStructure[i], ""), obj, index);
                }
                else
                {
                    obj = GetFieldValue(fieldStructure[i], obj);
                }
            }

            return (T)obj;
        }

        /// <summary>
        /// Sets the underlying C# value of a <see cref="SerializedProperty"/> using reflection.
        /// </summary>
        public static bool SetValue<T>(this SerializedProperty property, T value)
        {
            object obj = property.serializedObject.targetObject;
            string path = property.propertyPath.Replace(".Array.data", "");
            string[] fieldStructure = path.Split('.');

            // Traverse until the parent field of the last property
            for (int i = 0; i < fieldStructure.Length - 1; i++)
            {
                if (fieldStructure[i].Contains("["))
                {
                    int index = Convert.ToInt32(new string(fieldStructure[i].Where(char.IsDigit).ToArray()));
                    obj = GetFieldValueWithIndex(_rgx.Replace(fieldStructure[i], ""), obj, index);
                }
                else
                {
                    obj = GetFieldValue(fieldStructure[i], obj);
                }
            }

            // Handle last field (supports array/list index or normal field)
            string fieldName = fieldStructure.Last();
            if (fieldName.Contains("["))
            {
                int index = Convert.ToInt32(new string(fieldName.Where(char.IsDigit).ToArray()));
                return SetFieldValueWithIndex(_rgx.Replace(fieldName, ""), obj, index, value);
            }
            else
            {
                return SetFieldValue(fieldName, obj, value);
            }
        }

        /// <summary>
        /// Gets the value of a field from an object via reflection.
        /// </summary>
        private static object GetFieldValue(string fieldName, object obj,
            BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                    BindingFlags.NonPublic)
        {
            var fieldInfos = obj.GetType().GetFieldInfosIncludingBaseClasses(bindings);
            var field = fieldInfos.FirstOrDefault(x => x.Name == fieldName);

            return field != null ? field.GetValue(obj) : null;
        }

        /// <summary>
        /// Gets the value of a field that contains an array or list at a specific index.
        /// </summary>
        private static object GetFieldValueWithIndex(string fieldName, object obj, int index,
            BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                    BindingFlags.NonPublic)
        {
            var fieldInfos = obj.GetType().GetFieldInfosIncludingBaseClasses(bindings);
            var field = fieldInfos.FirstOrDefault(x => x.Name == fieldName);

            if (field != null)
            {
                object list = field.GetValue(obj);
                if (list.GetType().IsArray)
                {
                    return ((object[])list)[index];
                }
                else if (list is IEnumerable)
                {
                    return ((IList)list)[index];
                }
            }

            return null;
        }

        /// <summary>
        /// Sets the value of a field on an object via reflection.
        /// </summary>
        public static bool SetFieldValue(string fieldName, object obj, object value, bool includeAllBases = false,
            BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                    BindingFlags.NonPublic)
        {
            var fieldInfos = obj.GetType().GetFieldInfosIncludingBaseClasses(bindings);
            var field = fieldInfos.FirstOrDefault(x => x.Name == fieldName);

            if (field != null)
            {
                field.SetValue(obj, value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the value of a list/array field element at a specific index.
        /// </summary>
        public static bool SetFieldValueWithIndex(string fieldName, object obj, int index, object value,
            bool includeAllBases = false,
            BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                    BindingFlags.NonPublic)
        {
            var fieldInfos = obj.GetType().GetFieldInfosIncludingBaseClasses(bindings);
            var field = fieldInfos.FirstOrDefault(x => x.Name == fieldName);

            if (field == null) return false;
            object list = field.GetValue(obj);

            if (list.GetType().IsArray)
            {
                ((object[])list)[index] = value;
                return true;
            }

            if (list is not IEnumerable) return false;
            ((IList)list)[index] = value;
            return true;
        }
    }
#endif
}