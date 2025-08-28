using BiruisredEngine;

namespace IdleGear.Core
{

    public readonly struct HealthChanged : IEvent
    {
        public HealthChanged(int currentAmount, int deltaAmount)
        {
            CurrentAmount = currentAmount;
            DeltaAmount = deltaAmount;
        }
        public readonly int CurrentAmount;
        public readonly int DeltaAmount;
    }
    
    public readonly struct EnergyChanged : IEvent
    {
        public EnergyChanged(int currentAmount, int deltaAmount)
        {
            CurrentAmount = currentAmount;
            DeltaAmount = deltaAmount;
        }
        public readonly int CurrentAmount;
        public readonly int DeltaAmount;
    }
    
    public readonly struct StaminaChanged : IEvent
    {
        public StaminaChanged(int currentAmount, int deltaAmount)
        {
            CurrentAmount = currentAmount;
            DeltaAmount = deltaAmount;
        }
        public readonly int CurrentAmount;
        public readonly int DeltaAmount;
    }
    
    public readonly struct LevelChanged : IEvent
    {
        public LevelChanged(int currentAmount, int deltaAmount)
        {
            CurrentAmount = currentAmount;
            DeltaAmount = deltaAmount;
        }
        public readonly int CurrentAmount;
        public readonly int DeltaAmount;
    }
    
    public readonly struct ExpChanged : IEvent
    {
        public ExpChanged(int currentAmount, int maxAmount, int deltaAmount)
        {
            MaxAmount = maxAmount;
            CurrentAmount = currentAmount;
            DeltaAmount = deltaAmount;
        }
        public readonly int CurrentAmount;
        public readonly int MaxAmount;
        public readonly int DeltaAmount;
    }


}