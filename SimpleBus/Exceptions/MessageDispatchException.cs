using System;

namespace SimpleBus.Exceptions
{
    [Serializable]
    public class MessageDispatchException : BusException
    {
        public MessageDispatchException(string message)
            : base(message)
        {
        }
    }
}