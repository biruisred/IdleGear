using UnityEngine;

namespace BiruisredEngine
{
    public static class ExtColor
    {
        public static Color WithAlpha(this Color color, float a) {
            var col = color;
            col.a = a;
            return col;
        }
    }
}