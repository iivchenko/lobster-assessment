using System.Collections.Generic;
using System.Threading.Tasks;

namespace Questionnaire.Application.Domain.Common
{
    public interface IRepository<TAggregate, TId> where TAggregate 
        : IAggregateRoot<TId>
    {

        Task<int> ReadCount();

        Task<IEnumerable<TAggregate>> ReadAll(int skip, int take);

        Task<TAggregate> Read(TId id);

        Task Create(TAggregate aggregate);
    }
}
