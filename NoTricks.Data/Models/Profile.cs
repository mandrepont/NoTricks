using System;

namespace NoTricks.Data.Models
{
    public class Profile
    {
        public int Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PreferredName { get; set; }
        public string Phone { get; set; }
        public DateTime? Birthday { get; set; }
        public int AddressId { get; set; }
        public int AccountId { get; set; }
    }
}