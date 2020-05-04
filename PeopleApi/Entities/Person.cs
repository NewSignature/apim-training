namespace PeopleApi.Entities
{
    public class Person
    {
        public virtual string Id { get; set;  }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        public int Age { get; set; }
        public string FavoriteColor { get; set; }
    }
}