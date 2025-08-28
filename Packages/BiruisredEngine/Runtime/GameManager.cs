using UnityEngine;

namespace BiruisredEngine
{
    [DefaultExecutionOrder(-1000)]
    public abstract class GameManager : MonoBehaviour
    {
        protected virtual void Awake()
        {
            EVENT.Create();
        }

        protected void OnDestroy()
        {
            EVENT.Clear();
        }
    }
}