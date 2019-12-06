using System;

namespace NoTricks.Data.Models {
    public class SupplierPayout {
        public int Id { get; }
        public decimal Amount { get; set; }
        public DateTime PayedAt { get; set; }
        public int SupplierId { get; set; }
        public int StaffId { get; set; }
    }
}