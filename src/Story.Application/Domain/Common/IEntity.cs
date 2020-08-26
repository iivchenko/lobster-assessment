namespace Story.Application.Domain.Common
{
    public interface IEntity<TId>
    {
        TId Id { get; }
    }
}
