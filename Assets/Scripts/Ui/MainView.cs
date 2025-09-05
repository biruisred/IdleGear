using UnityEngine;

namespace IdleGear.UI
{
    public class MainView : MonoBehaviour
    {
        [SerializeField] private VisibilityComponent homeVisibility;
        [SerializeField] private VisibilityComponent npcInteractionVisibility;
        [SerializeField] private float animationDuration = 0.2f;

        private void Start()
        {
            homeVisibility.SetVisible(true);
            npcInteractionVisibility.SetVisible(false);
        }

        public void SetInteractionView()
        {
            homeVisibility.SetVisible(false, animationDuration);
            npcInteractionVisibility.SetVisible(true, animationDuration);
        }

        public void CloseInteractionView()
        {
            homeVisibility.SetVisible(true, animationDuration);
            npcInteractionVisibility.SetVisible(false, animationDuration);
        }
    }
}