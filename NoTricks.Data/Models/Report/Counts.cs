namespace NoTricks.Data.Models.Report {
    public class Counts {
        public int Accounts { get; set; }
        public int Suppliers { get; set; }
        public decimal PendingPayoutSum { get; set; }
        public decimal PayoutSum { get; set; }
    }
}