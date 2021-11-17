using System;
using System.Runtime.Serialization;

namespace IDAL.DO
{
    [Serializable]
    public class UnvalidIDException : Exception
    {
        public UnvalidIDException()
        {
        }

        public UnvalidIDException(string message) : base(message)
        {
        }

        public UnvalidIDException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnvalidIDException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class ExistingObjectException : Exception
    {
        public ExistingObjectException()
        {
        }

        public ExistingObjectException(string message) : base(message)
        {
        }

        public ExistingObjectException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExistingObjectException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}