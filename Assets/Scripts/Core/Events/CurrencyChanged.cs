using BiruisredEngine;

namespace IdleGear.Core
{
    public readonly struct DiamondChanged : IEvent
    {
        public DiamondChanged(int currentAmount, int deltaAmount)
        {
            CurrentAmount = currentAmount;
            DeltaAmount = deltaAmount;
        }
        public readonly int CurrentAmount;
        public readonly int DeltaAmount;
    }
    
    public readonly struct GoldChanged : IEvent
    {
        public GoldChanged(int currentAmount, int deltaAmount)
        {
            CurrentAmount = currentAmount;
            DeltaAmount = deltaAmount;
        }
        public readonly int CurrentAmount;
        public readonly int DeltaAmount;
    }
}
