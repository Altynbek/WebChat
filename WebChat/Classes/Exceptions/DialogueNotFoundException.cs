using System;

namespace WebChat.Classes.Exceptions
{
    [Serializable()]
    public sealed class DialogueNotFoundException : ApplicationException
    {
        public DialogueNotFoundException() : base() { }

        public DialogueNotFoundException(string message) : base(message) { }

        public DialogueNotFoundException(string message, Exception inner) : base(message, inner) { }

        private DialogueNotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}