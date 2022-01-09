using System;
using System.Runtime.Serialization;

namespace DO
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

    [Serializable]
    public class XMLFileLoadCreateException : Exception
    {
        public XMLFileLoadCreateException(string filePath) : base() { }
        public XMLFileLoadCreateException(string message, string v, Exception ex) : base(message) { }
        public XMLFileLoadCreateException(string message, Exception innerException, Exception ex) : base(message, innerException) { }
        public override string ToString()
        {
            return Message;
        }
    }

}