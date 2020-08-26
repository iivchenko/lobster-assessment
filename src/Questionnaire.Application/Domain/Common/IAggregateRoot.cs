namespace Questionnaire.Application.Domain.Common
{
    public interface IAggregateRoot<TId>
    {
        TId Id { get; }
    }
}
