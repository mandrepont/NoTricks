namespace NoTricks.Data.Models {
    public class RoleMapping {
        public int Id { get; }
        public int RoleId { get; set; }
        public int AccountId { get; set; }
    }
}