using System;

namespace Questionnaire.Application.Domain.Common
{
    public class DomainException : Exception
    {
        public DomainException(string message)
          : base(message)
        {
        }
    }
}
