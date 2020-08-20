﻿using Newtonsoft.Json;
using StackExchange.Redis;
using Story.Application.Domain.Stories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Story.Infrastructure.Stories
{
    public sealed class RedisStoryRepository : IStoryRepository, IDisposable
    {
        private const string KeyPattern = "story:";

        private readonly ConnectionMultiplexer _redis;
        private readonly JsonSerializerSettings _settings;

        public RedisStoryRepository(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
        }

        public Task<Application.Domain.Stories.Story> Read(Guid id)
        {
            var value = _redis.GetDatabase().StringGet($"{KeyPattern}{id}");
            var story = JsonConvert.DeserializeObject<Application.Domain.Stories.Story>(value, _settings);

            return Task.FromResult(story);
        }

        public Task<IEnumerable<Application.Domain.Stories.Story>> ReadAll()
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var db = _redis.GetDatabase();

            var stories =
                 server
                 .Keys(pattern: $"{KeyPattern}*")
                 .Select(x => JsonConvert.DeserializeObject<Application.Domain.Stories.Story>(db.StringGet(x), _settings));

            return Task.FromResult(stories);
        }

        public Task Update(Application.Domain.Stories.Story story)
        {
            var db = _redis.GetDatabase();
            db.StringSet($"{KeyPattern}{story.Id}", JsonConvert.SerializeObject(story, _settings));

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _redis.Dispose();
        }
    }
}
