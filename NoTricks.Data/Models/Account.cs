using System;

namespace NoTricks.Data.Models {
    public class Account {
        public int Id { get; }
        public string EMail { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public AccountStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }

    public enum AccountStatus {
        Disabled,
        Banned,
        PendingVerification,
        LockedOut,
        Ok
    }
}