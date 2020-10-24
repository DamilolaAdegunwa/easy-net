using System;
using System.Runtime.Serialization;

namespace EasyNet
{
    public class EasyNetException : Exception
    {
        public EasyNetException()
        {

        }

        public EasyNetException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        public EasyNetException(string message)
            : base(message)
        {

        }

        public EasyNetException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
