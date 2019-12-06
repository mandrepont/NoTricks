using System;

namespace NoTricks.Data.Models {
    public class SupplierManager {
        public int ProfileId { get; }
        public int AccountId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string PreferredName { get; }

        public string PreferredOrFirstName => string.IsNullOrWhiteSpace(PreferredName) ? FirstName : PreferredName; 
        
        public string PreferredFirstLastName => $"{PreferredOrFirstName} {LastName}";
        
        public string PreferredLastFirstName => $"{LastName}, {PreferredOrFirstName}";
    }
}