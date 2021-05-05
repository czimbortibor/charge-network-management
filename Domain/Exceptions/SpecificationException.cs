using System;

namespace Domain.Exceptions
{
    public class SpecificationException : Exception
    {
        public SpecificationException(string message)
            : base(message)
        {
        }
    }
}
