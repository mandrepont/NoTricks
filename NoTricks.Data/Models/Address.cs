using NoTricks.Data.Repositories;

namespace NoTricks.Data.Models {
    public class Address {
        public int Id { get; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
