using BiruisredEngine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleGear.UI
{
    public class UIManager : MonoBehaviour
    {
        public string id;

        [Button]
        public void GenerateId()
        {
            id = IDGenerator.Generate();
        }
    }

    public class PageView : MonoBehaviour
    {
        
    }
}
