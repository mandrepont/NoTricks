namespace NoTricks.Data.Models {
    public class Account {
        public int Id { get; }
        public string EMail { get; set; }
        public string PasswordHash { get; set; }
    }
}