using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    public class BLInvalidInputException : Exception
    {
        public BLInvalidInputException()
        {
        }

        public BLInvalidInputException(string message) : base(message)
        {

        }

        public BLInvalidInputException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BLInvalidInputException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    internal class DronechargeException : Exception
    {
        public DronechargeException()
        {
        }

        public DronechargeException(string message) : base(message)
        {
        }

        public DronechargeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DronechargeException(SerializationInfo info, StreamingContext context) : base(info, context)
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

    [Serializable]
    public class BLParcelException : Exception
    {
        public BLParcelException()
        {
        }

        public BLParcelException(string message) : base(message)
        {
        }

        public BLParcelException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BLParcelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class FailedToAddException : Exception
    {
        public FailedToAddException()
        {
        }

        public FailedToAddException(string message) : base(message)
        {
        }

        public FailedToAddException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FailedToAddException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    internal class FailedToUpdateException : Exception
    {
        public FailedToUpdateException()
        {
        }

        public FailedToUpdateException(string message) : base(message)
        {
        }

        public FailedToUpdateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FailedToUpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}