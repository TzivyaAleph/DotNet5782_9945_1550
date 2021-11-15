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
}