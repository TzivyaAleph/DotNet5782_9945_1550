using System;
using System.Runtime.Serialization;

namespace IBL
{
    namespace BO
    {
        [Serializable]
        public class InvalidInputException : Exception
        {
            public InvalidInputException() : base() { }
            public InvalidInputException(string message) : base(message) { }
            public InvalidInputException(string message, Exception innerException) : base(message, innerException) { }
            public override string ToString()
            {
                return Message;
            }
        }

        [Serializable]
        public class FailedToAddException : Exception
        {
            public FailedToAddException() : base() { }
            public FailedToAddException(string message) : base(message) { }
            public FailedToAddException(string message, Exception innerException) : base(message, innerException) { }
            public override string ToString()
            {
                return Message;
            }
        }

        [Serializable]
        internal class FailedToGetException : Exception
        {
            public FailedToGetException() : base() { }
            public FailedToGetException(string message) : base(message) { }
            public FailedToGetException(string message, Exception innerException) : base(message, innerException) { }
            public override string ToString()
            {
                return Message;
            }
        }

        [Serializable]
        internal class InputDoesNotExist : Exception
        {
            public InputDoesNotExist() : base() { }
            public InputDoesNotExist(string message) : base(message) { }
            public InputDoesNotExist(string message, Exception innerException) : base(message, innerException) { }
            public override string ToString()
            {
                return Message;
            }
        }

        [Serializable]
        internal class FailedToUpdateException : Exception
        {
            public FailedToUpdateException() : base() { }
            public FailedToUpdateException(string message) : base(message) { }
            public FailedToUpdateException(string message, Exception innerException) : base(message, innerException) { }
            public override string ToString()
            {
                return Message;
            }
        }
    }
}


