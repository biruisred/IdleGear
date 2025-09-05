using UnityEngine;

namespace IdleGear.UI
{
    public class SaveArea : MonoBehaviour
    {
        [SerializeField] private RectTransform rect;
        private void Awake() {
            rect = GetComponent<RectTransform>();
        }

        private void OnRectTransformDimensionsChange() {
            var safeArea = Screen.safeArea;
            var minAnchor = safeArea.position;
            var maxAnchor = minAnchor + safeArea.size;
            minAnchor.x /= Screen.width;
            minAnchor.y /= Screen.height;

            maxAnchor.x /= Screen.width;
            maxAnchor.y /= Screen.height;

            rect.anchorMin = minAnchor;
            rect.anchorMax = maxAnchor;
        }
    }
}
