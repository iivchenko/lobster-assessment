using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Story.Application.Domain.Stories
{
    public interface IStoryRepository
    {
        Task<IEnumerable<Story>> ReadAll();

        Task<Story> Read(Guid id);

        Task Update(Story story);
    }
}
