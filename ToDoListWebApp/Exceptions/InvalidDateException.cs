using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListWebApp.Exceptions
{
    public class InvalidDateException : Exception
    {
        public InvalidDateException()
        {
        }

        public InvalidDateException(string message) : base(message)
        {
        }

        public InvalidDateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidDateException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
