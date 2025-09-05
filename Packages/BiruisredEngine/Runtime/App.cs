using System.Collections;
using UnityEngine;

namespace BiruisredEngine
{
    [DefaultExecutionOrder(-1000)]
    public abstract class App : MonoBehaviour
    {
        protected virtual void Awake() {
            SYSTEM.App = this;
            SYSTEM.Create();
        }

        protected virtual IEnumerator Start() {
            yield return SYSTEM.Initialize();
        }

        protected virtual void OnDestroy()
        {
            SYSTEM.Destroy();
        }
    }
}