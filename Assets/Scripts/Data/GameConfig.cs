using UnityEngine;

namespace IdleGear.Data
{
    
    [CreateAssetMenu(menuName = "Config/Game Config", fileName = "Game Config", order = 0)]
    public class GameConfig : ScriptableObject
    {
        public DataConfig DataConfig => dataConfig;
        [SerializeField] private DataConfig dataConfig;
    }
}
