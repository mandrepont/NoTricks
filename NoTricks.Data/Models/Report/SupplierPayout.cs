using System;

namespace NoTricks.Data.Models.Report {
    public class SupplierPayoutCount {
        
        public SupplierPayoutCount() { }

        public SupplierPayoutCount(DateTime payedAt, int count) {
            PayedAt = payedAt;
            Count = count;
        }

        public DateTime PayedAt { get; }
        public int Count { get; }
    }
    
    public class SupplierPayoutSum {
        
        public SupplierPayoutSum() { }

        public SupplierPayoutSum(DateTime payedAt, decimal sum) {
            PayedAt = payedAt;
            Sum = sum;
        }

        public DateTime PayedAt { get; }
        public decimal Sum { get; }
    }
}