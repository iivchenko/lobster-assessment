using Newtonsoft.Json;
using StackExchange.Redis;
using Questionnaire.Application.Domain.Common;
using Questionnaire.Application.Domain.Polls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questionnaire.Infrastructure.Polls
{
    public sealed class RedisPollRepository : IRepository<Poll, Guid>
    {
        private const string KeyPattern = "poll:";

        private readonly ConnectionMultiplexer _redis;
        private readonly JsonSerializerSettings _settings;

        public RedisPollRepository(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
        }

        public Task Create(Poll aggregate)
        {
            var db = _redis.GetDatabase();
            db.StringSet($"{KeyPattern}{aggregate.Id}", JsonConvert.SerializeObject(aggregate, _settings));

            return Task.CompletedTask;
        }

        public Task<Poll> Read(Guid id)
        {
            var value = _redis.GetDatabase().StringGet($"{KeyPattern}{id}");
            if (value.IsNull)
            {
                return Task.FromResult((Poll)null);
            }

            var poll = JsonConvert.DeserializeObject<Poll>(value, _settings);

            return Task.FromResult(poll);
        }

        public Task<IEnumerable<Poll>> ReadAll(int skip, int take)
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var db = _redis.GetDatabase();

            var polls =
                 server
                 .Keys(pattern: $"{KeyPattern}*")
                 .Skip(skip)
                 .Take(take)
                 .Select(x => JsonConvert.DeserializeObject<Poll>(db.StringGet(x), _settings));

            return Task.FromResult(polls);
        }

        public Task<int> ReadCount()
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());

            var count =
                 server
                 .Keys(pattern: $"{KeyPattern}*")
                 .Count();

            return Task.FromResult(count);
        }
    }
}
