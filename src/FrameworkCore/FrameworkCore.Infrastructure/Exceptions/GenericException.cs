using System;

namespace FrameworkCore.Infrastructure.Exceptions
{
    public class GenericException : Exception
    {
        public string ExceptionId { get; set; }

        public GenericException()
        {
        }

        public GenericException(string message)
            : base(message)
        {
        }

        public GenericException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
