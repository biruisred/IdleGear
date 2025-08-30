using System;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IdleGear.Data
{
    
    [CreateAssetMenu(menuName = "Config/Data Config", fileName = "Data Config", order = 0)]
    public class DataConfig : ScriptableObject
    {
        public Place[] Places => places;
        [SerializeField] private Place[] places = Array.Empty<Place>();

        
#if UNITY_EDITOR
        [Button]
        public void CollectAndResolve()
        {
            places = Resources.LoadAll<Place>("Data");
            
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }
#endif
    }
}
