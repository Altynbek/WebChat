using System;

namespace WebChat.Classes.Exceptions
{
    [Serializable()]
    public sealed class UserNotFoundException : ApplicationException
    {
        public UserNotFoundException() : base() { }

        public UserNotFoundException(string message) : base(message) { }

        public UserNotFoundException(string message, Exception inner) : base(message, inner) { }

        private UserNotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}