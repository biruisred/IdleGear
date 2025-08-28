using BiruisredEngine;

namespace IdleGear.Core
{
    public readonly struct AnnouncementMessage : IEvent
    {
        public AnnouncementMessage(string message)
        {
            Message = message;
        }
        public readonly string Message;
    }
    
    public readonly struct GlobalMessage : IEvent
    {
        public GlobalMessage(string message)
        {
            Message = message;
        }
        public readonly string Message;
    }

}