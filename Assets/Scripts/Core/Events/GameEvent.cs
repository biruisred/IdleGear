using BiruisredEngine;

namespace IdleGear.Core
{
    public readonly struct QuestComplete : IEvent
    {
        public QuestComplete( bool questId, bool success, int goldGain, int expGain, string message)
        {
            Success = success;
            GoldGain = goldGain;
            ExpGain = expGain;
            Message = message;
            QuestId = questId;
        }
        public readonly bool QuestId;
        public readonly bool Success;
        public readonly int GoldGain;
        public readonly int ExpGain;
        public readonly string Message;
        
    }
}