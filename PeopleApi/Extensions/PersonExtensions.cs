using PeopleApi.Entities;
using PeopleApi.Models;

namespace PeopleApi.Extensions
{
    public static class PersonExtensions
    {
        public static PersonResponse AsPersonResponse(this Person person)
        {
            return new PersonResponse
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Age = person.Age,
                FavoriteColor = person.FavoriteColor
            };
        }
    }
}