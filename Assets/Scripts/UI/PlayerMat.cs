using System;
using System.Collections.Generic;
using BiruisredEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGear.UI
{
    public class PlayerMat : MonoBehaviour
    {
        public enum Mode
        {
            None,
            Battle,
            Dialogue,
            Shop,
            Stat,
        }

        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private LayoutElement layoutElement;
        [SerializeField] private GameObject stat;
        [SerializeField] private float animationDuration = 0.2f;
        [SerializeField] private SerializedDictionary<Mode, LayoutSetting> layoutSettings = new();
        private void Start()
        {
            SetMode(Mode.None);
        }

        public void SetMode(Mode mode)
        {
            foreach (var (key, setting) in layoutSettings)
            {
                if (key == mode) continue;
                setting.visibilityComponents.ForEach(x => x.SetVisible(false, animationDuration));
            }
            if (layoutSettings.TryGetValue(mode, out var layoutSetting))
            {
                layoutSetting.visibilityComponents.ForEach(x => x.SetVisible(true, animationDuration));
            }
            SetContainerHigh(layoutSetting.preferredHeight, animationDuration);
        }

        private void SetContainerHigh(float value, float duration = 0, Action onComplete = null)
        {
            if (Mathf.Approximately(value, layoutElement.preferredHeight)) return;
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                layoutElement.preferredHeight = value;
                return;
            }
#endif
            layoutElement.DOKill();
            DOTween.To(() => layoutElement.preferredHeight, x => layoutElement.preferredHeight = x, value, duration)
                .SetId(layoutElement).OnComplete(() => onComplete?.Invoke());
        }

        [Title("Mode")]
        [Button]
        public void SetModeNone() => SetMode(Mode.None);

        [Button]
        public void SetModeBattle() => SetMode(Mode.Battle);

        [Button]
        public void SetModeDialogue() => SetMode(Mode.Dialogue);

        [Button]
        public void SetModeShop() => SetMode(Mode.Shop);
        [Button]
        public void SetModeStat() => SetMode(Mode.Stat);

        [Serializable]
        public class LayoutSetting
        {
            public List<VisibilityComponent> visibilityComponents = new();
            public float preferredHeight = 360;
        }
    }
}