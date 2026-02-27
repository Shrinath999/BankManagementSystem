namespace BankingSystem.Entities.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public int? FromAccountId { get; set; }
        public int? ToAccountId { get; set; }

        public decimal Amount { get; set; }

        public string TransactionType { get; set; }

        public string Status { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Account FromAccount { get; set; }
        public Account ToAccount { get; set; }
    }
}
