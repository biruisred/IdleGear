using System.Collections.Generic;
using System.Linq;
using UnityEditor;

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
    }
}
#endif