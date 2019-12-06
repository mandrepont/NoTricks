using System;

namespace NoTricks.Data.Models {
    public class SupplierPayout {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PayedAt { get; set; }
        public int SupplierId { get; set; }
        public int StaffId { get; set; }
    }
    
    public class ExtraSupplierPayout : SupplierPayout{
        public string SupplierName { get; set; }
        public string StaffFirstName { get; set; }
        public string StaffLastName { get; set; }
        public string StaffPreferredName { get; set; }
        
        public string StaffPreferredOrFirstName => string.IsNullOrWhiteSpace(StaffPreferredName) ? StaffFirstName : StaffLastName; 
        
        public string StaffPreferredFirstLastName => $"{StaffPreferredOrFirstName} {StaffLastName}";
        
        public string StaffPreferredLastFirstName => $"{StaffLastName}, {StaffPreferredOrFirstName}";
    }
}