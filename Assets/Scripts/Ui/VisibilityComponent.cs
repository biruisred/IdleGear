using DG.Tweening;
using UnityEngine;

namespace IdleGear.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class VisibilityComponent : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        public bool visible = true;
        public bool disableWhenHidden = true;

        public void SetVisible(bool value, float durationInSeconds = 0)
        {
            if (visible == value) return;
            gameObject.SetActive(true);
            visible = value;
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                canvasGroup.alpha = value ? 1 : 0;
                gameObject.SetActive(!disableWhenHidden || value);
                return;
            }
#endif
            if (durationInSeconds <= 0)
            {
                canvasGroup.alpha = value ? 1 : 0;
                gameObject.SetActive(!disableWhenHidden || value);
                return;
            }

            canvasGroup.DOKill();
            canvasGroup.DOFade(value ? 1f : 0f, durationInSeconds)
                .OnComplete(() =>
                {
                    if (value) return;
                    gameObject.SetActive(!disableWhenHidden);
                });
        }

        private void OnValidate()
        {
            canvasGroup ??= GetComponent<CanvasGroup>();
            canvasGroup.alpha = visible ? 1 : 0;
            gameObject.SetActive(!disableWhenHidden || visible);
        }
    }
}