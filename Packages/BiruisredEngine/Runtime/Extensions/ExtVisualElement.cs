using UnityEngine.UIElements;

namespace BiruisredEngine
{
    public static class ExtVisualElement
    {
        public static VisualElement WithClassName(this VisualElement visualElement, params string[] classNames)
        {
            foreach (var className in classNames)
            {
                visualElement.AddToClassList(className);
            }

            return visualElement;
        }
    }
}
