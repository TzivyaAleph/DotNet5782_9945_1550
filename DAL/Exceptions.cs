using System;
using System.Runtime.Serialization;

namespace IDAL.DO
{
    [Serializable]
    public class UnvalidIDException : Exception
    {
        public UnvalidIDException() : base() { }
        public UnvalidIDException(string message) : base(message) { }
        public UnvalidIDException(string message, Exception innerException) : base(message, innerException) { }
        public override string ToString()
        {
            return Message;
        }
    }

    [Serializable]
    public class ExistingObjectException : Exception
    {
        public ExistingObjectException() : base() { }
        public ExistingObjectException(string message) : base(message) { }
        public ExistingObjectException(string message, Exception innerException) : base(message, innerException) { }
        public override string ToString()
        {
            return Message;
        }
    }
}