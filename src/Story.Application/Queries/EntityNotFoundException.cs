using System;

namespace Story.Application.Queries
{
    [Serializable]
    public sealed class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(Guid id)
            : base($"Entity with id: {id} was not found")
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
