using BiruisredEngine;
using UnityEngine;

namespace IdleGear
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI usernameText;
        [SerializeField] private TMPro.TextMeshProUGUI goldText;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            EVENT.AddListener<PlayerInit>(OnPlayerInit);
        }

        private void OnPlayerInit(PlayerInit evt)
        {
            usernameText.text = evt.PlayerName;
            goldText.text = evt.GoldAmount.ToString();
        }
    }
}
