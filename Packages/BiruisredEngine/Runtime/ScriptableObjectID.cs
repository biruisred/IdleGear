using Sirenix.OdinInspector;

namespace BiruisredEngine
{
    public abstract class ScriptableObjectID : SerializedScriptableObject
    {
        public string ID => _id;
        [ReadOnly]
        private string _id;
    }
}