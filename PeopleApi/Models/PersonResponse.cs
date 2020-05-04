using System.Text.Json.Serialization;
using PeopleApi.Entities;

namespace PeopleApi.Models
{
    public class PersonResponse
    {
        [JsonIgnore]
        public string FirstName { get; set; }

        [JsonIgnore]
        public string LastName { get; set; }

        [JsonPropertyName("name")]
        public string FullName => $"{FirstName} {LastName}";

        public int Age { get; set; }
        public string FavoriteColor { get; set; }
        public string Id { get; set; }
    }
}