using System;

namespace WebChat.Classes.Exceptions
{
    [Serializable()]
    public sealed class MessageNotFoundException : ApplicationException
    {
        public MessageNotFoundException() : base() { }

        public MessageNotFoundException(string message) : base(message) { }

        public MessageNotFoundException(string message, Exception inner) : base(message, inner) { }

        private MessageNotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}