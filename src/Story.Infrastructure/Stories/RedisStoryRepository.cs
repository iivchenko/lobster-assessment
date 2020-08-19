using Story.Application.Domain.Stories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Story.Infrastructure.Stories
{
    public sealed class RedisStoryRepository : IStoryRepository
    {
        public Task<Application.Domain.Stories.Story> Read(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Application.Domain.Stories.Story>> ReadAll()
        {
            throw new NotImplementedException();
        }

        public Task Update(Application.Domain.Stories.Story story)
        {
            throw new NotImplementedException();
        }
    }
}
