using System;

namespace NoTricks.Data.Models.Report {
    public class AccountCreatedCount {
        public AccountCreatedCount() { }

        public AccountCreatedCount(DateTime createdDate, int count) {
            CreatedDate = createdDate;
            Count = count;
        }

        public DateTime CreatedDate { get; }
        public int Count { get; }
    }
}
