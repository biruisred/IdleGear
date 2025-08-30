using Sirenix.OdinInspector;
using UnityEngine;

namespace BiruisredEngine
{
    public abstract class ScriptableObjectID : SerializedScriptableObject
    {
        public string ID => id;
        [SerializeField] private string id;
    }
}