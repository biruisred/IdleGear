using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace BiruisredEngine
{
    public static class ExtEditor
    {
        /// <summary>
        /// Get all scenes in project
        /// </summary>
        /// <param name="folders">optional target folder e.g. Assets/Scenes</param>
        public static IEnumerable<SceneAsset> GetAllScenes(params string[] folders) {
            return AssetDatabase.FindAssets("t:Scene", folders).Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<SceneAsset>);
        }
        
        public static IEnumerable<ScriptableObject> LoadAllScriptableObject(Type type) {
            return AssetDatabase.FindAssets("t: " + type.Name)
                .Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<ScriptableObject>);
        }

        public static List<T> LoadAllScriptableObject<T>(params string[] folders) where T : ScriptableObject {
            return AssetDatabase.FindAssets("t: " + typeof(T).Name, folders).ToList()
                .Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<T>).ToList();
        }
    }
}
#endif