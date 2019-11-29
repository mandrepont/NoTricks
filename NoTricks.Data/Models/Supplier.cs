﻿namespace NoTricks.Data.Models {
    public class Supplier {
        public int Id { get; }
        public string CompanyName { get; set; }
        public decimal Balance { get; set; }
        public int? ManagerId { get; set; }
        public int? AddressId { get; set; }
    }
}