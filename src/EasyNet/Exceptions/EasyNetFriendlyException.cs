using System;
using System.Runtime.Serialization;

namespace EasyNet
{
    public class EasyNetFriendlyException : EasyNetException
    {
        public EasyNetFriendlyException(int code, string message):base(message)
        {
            Code = code;
        }

        public EasyNetFriendlyException(int code, string message, Exception innerException)
            : base(message, innerException)
        {
            Code = code;
        }

        public int Code { get; set; }
    }
}
