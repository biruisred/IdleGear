using BiruisredEngine;
using UnityEngine;

namespace IdleGear.UI
{
    public class NPCInteractionView : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI dialogueText;
        
        private readonly TypingText _typingText = new(0.01f);

        private void OnEnable()
        {
            var text =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ";
            StopAllCoroutines();
            StartCoroutine(_typingText.Play(text, value =>
            {
                dialogueText.text = value;
            }, value =>
            {
                dialogueText.text = value;
            }));

        }

        public void SkipDialogue()
        {
            _typingText.Pass(true);
        }
    }
}