using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    public class BLIdException : Exception
    {
        public BLIdException()
        {
        }

        public BLIdException(string message) : base(message)
        {
        }

        public BLIdException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BLIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class BLInvalidNumberException : Exception
    {
        public BLInvalidNumberException()
        {
        }

        public BLInvalidNumberException(string message) : base(message)
        {
        }

        public BLInvalidNumberException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BLInvalidNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class BLInvalidStringException : Exception
    {
        public BLInvalidStringException()
        {
        }

        public BLInvalidStringException(string message) : base(message)
        {
        }

        public BLInvalidStringException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BLInvalidStringException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class InvalidLocationException : Exception
    {
        public InvalidLocationException()
        {
        }

        public InvalidLocationException(string message) : base(message)
        {
        }

        public InvalidLocationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidLocationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}