namespace NoTricks.Data.Models.Report {
    public class AccountStatusCount {
        public AccountStatusCount() { }

        public AccountStatusCount(AccountStatus status, int count) {
            Status = status;
            Count = count;
        }

        public AccountStatus Status { get; }
        public int Count { get; }
    }
}
