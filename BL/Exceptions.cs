﻿using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    public class InvalidInputException : Exception
    {
        public InvalidInputException()
        {
        }

        public InvalidInputException(string message) : base(message)
        {

        }

        public InvalidInputException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidInputException(SerializationInfo info, StreamingContext context) : base(info, context)
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
    internal class FailedToAddException : Exception
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
    internal class FailedToGetException : Exception
    {
        public FailedToGetException()
        {
        }

        public FailedToGetException(string message) : base(message)
        {
        }

        public FailedToGetException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FailedToGetException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FailedToGetException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    internal class InputDoesNotExist : Exception
    {
        public InputDoesNotExist()
        {
        }

        public InputDoesNotExist(string message) : base(message)
        {
        }

        public InputDoesNotExist(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InputDoesNotExist(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
