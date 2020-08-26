using System;

namespace Story.Application.Domain.Common
{
    public class DomainException : Exception
    {
        public DomainException(string message)
          : base(message)
        {
        }
    }
}
