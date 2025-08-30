using System;
using BiruisredEngine;
using UnityEngine;

namespace IdleGear.Data
{
    [CreateAssetMenu(fileName = "Place", menuName = "Data/Place")]
    public class Place : ScriptableObjectID
    {
        public PlaceSpot[] Spots => spots;
        [SerializeField] private PlaceSpot[] spots = Array.Empty<PlaceSpot>();
    }
}
