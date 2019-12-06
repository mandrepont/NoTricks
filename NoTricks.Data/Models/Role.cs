namespace NoTricks.Data.Models {
    public class Role {
       public int Id { get; set; }
       public string Name { get; set; }
       public string Description { get; set; }

       public override bool Equals(object obj) {
           if (obj is Role role) {
               return Equals(role);
           }
           return base.Equals(obj);
       }

       private bool Equals(Role other) {
           return Id == other.Id && Name == other.Name && Description == other.Description;
       }

       public override int GetHashCode() {
           unchecked {
               var hashCode = Id;
               hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
               hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
               return hashCode;
           }
       }
    }
}