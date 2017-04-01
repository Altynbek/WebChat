using System;

namespace WebChat.Classes.Exceptions
{
    [Serializable()]
    public sealed class UserContactNotFoundException : ApplicationException
    {
        public UserContactNotFoundException() : base() { }

        public UserContactNotFoundException(string message) : base(message) { }

        public UserContactNotFoundException(string message, Exception inner) : base(message, inner) { }

        private UserContactNotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}