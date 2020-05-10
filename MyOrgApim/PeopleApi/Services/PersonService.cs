using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PeopleApi.Entities;
using PeopleApi.Models;
using StackExchange.Redis;

namespace PeopleApi.Services
{
    public class PersonService
    {
        private const string RedisPeopleKey = "people-key";
        private const int RedisDataExpireMinutes = 2;

        private readonly IConfiguration _configuration;

        public PersonService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IList<Person>> GetPeople()
        {
            return await GetRedisPeopleAsync();
        }

        public async Task<Person> GetPerson(string personId)
        {
            var people = await GetRedisPeopleAsync();
            return people.FirstOrDefault(x => x.Id == personId);
        }

        public async Task<Person> CreatePerson(PersonRequest newPerson)
        {
            var people = await GetRedisPeopleAsync();
            var person = new Person
            {
                FirstName = newPerson.FirstName,
                LastName = newPerson.LastName,
                Age = newPerson.Age,
                FavoriteColor = newPerson.FavoriteColor,
                Id = Guid.NewGuid().ToString()
            };

            people.Add(person);
            await SavePeopleAsync(people);

            return person;
        }

        public async Task<Person> UpdatePerson(string id, PersonRequest updatedPerson)
        {
            var people = await GetRedisPeopleAsync();
            var person = people.FirstOrDefault(x => x.Id == id);
            
            person.Age = updatedPerson.Age;
            person.FavoriteColor = updatedPerson.FavoriteColor;
            person.FirstName = updatedPerson.FirstName;
            person.LastName = updatedPerson.LastName;

            await SavePeopleAsync(people);
            return person;
        }

        public async Task DeletePerson(string id)
        {
            await SavePeopleAsync(
                (await GetRedisPeopleAsync()).Where(x => x.Id != id).ToList()
            );
        }

        public async Task<IList<Person>> GetPeopleByPrefix(string prefix)
        {
            var people = await GetPeople();
            return people.Where(x => x.FirstName.StartsWith(prefix)).ToList();
        }

        async Task<IList<Person>> GetRedisPeopleAsync()
        {
            var redis = ConnectionMultiplexer.Connect(_configuration["REDIS_HOSTNAME"]);
            var database = redis.GetDatabase();

            var peopleRaw = await database.StringGetAsync(RedisPeopleKey);
            if (string.IsNullOrEmpty(peopleRaw))
                return new List<Person>();

            return JsonConvert.DeserializeObject<IList<Person>>(peopleRaw);
        }

        async Task SavePeopleAsync(IList<Person> people)
        {
            var redis = ConnectionMultiplexer.Connect(_configuration["REDIS_HOSTNAME"]);
            var database = redis.GetDatabase();
            var rawString = JsonConvert.SerializeObject(people);

            await database.StringSetAsync(RedisPeopleKey, rawString, TimeSpan.FromMinutes(RedisDataExpireMinutes));
        }
    }
}