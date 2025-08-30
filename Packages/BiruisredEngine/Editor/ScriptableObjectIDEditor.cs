using UnityEditor;
using UnityEngine;

namespace BiruisredEngine.Editor
{
    [CustomEditor(typeof(ScriptableObjectID))]
    public class ScriptableObjectIDEditor : UnityEditor.Editor
    {
        
        
        private void OnEnable()
        {
            var scriptableObjectID = (ScriptableObjectID)target;
            Debug.Log(scriptableObjectID.GetValue<int>("_id"));            
        }
    }
}
