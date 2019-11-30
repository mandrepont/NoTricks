using System;

namespace NoTricks.Data.Models {
    public class SupplierManager {
        public int ProfileId { get; }
        public int AccountId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string PreferredName { get; }

        public string PreferredFirstLastName => String.IsNullOrWhiteSpace(PreferredName)
            ? $"{FirstName} {LastName}"
            : $"{PreferredName} {LastName}";
    }
}