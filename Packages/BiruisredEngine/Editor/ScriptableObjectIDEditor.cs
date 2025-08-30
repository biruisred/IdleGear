using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace BiruisredEngine.Editor
{
    [CustomEditor(typeof(ScriptableObjectID), true)]
    public class ScriptableObjectIDEditor : OdinEditor
    {
        protected override void OnEnable()
        {
            var scriptableObjectID = (ScriptableObjectID)target;
            var guid  = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(scriptableObjectID));
            if (guid != scriptableObjectID.ID)
            {
                scriptableObjectID.SetValue("id", guid);
                EditorUtility.SetDirty(scriptableObjectID);
                AssetDatabase.SaveAssetIfDirty(scriptableObjectID);
            }
            
            base.OnEnable();
        }
    }
}