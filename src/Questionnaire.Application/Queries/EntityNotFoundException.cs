using System;

namespace Questionnaire.Application.Queries
{
    [Serializable]
    public sealed class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(Guid id, string type)
            : base($"'{type}' entity with id: '{id}' was not found")
        {
            Id = id;
            Type = type;
        }

        public Guid Id { get; }

        public string Type { get; }
    }
}
