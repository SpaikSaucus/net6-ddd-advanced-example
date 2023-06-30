using System;

namespace AuthorizationOperation.Domain.Exceptions
{
    public class CustomerDomainException : DomainException
    {
        public CustomerDomainException() { }

        public CustomerDomainException(string message) : base(message) { }

        public CustomerDomainException(string message, Exception exception) : base(message,exception) { }
    }
}
