namespace BankingSystem.Entities.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string? TxnNumber { get; set; }   // Bank Transaction Number

        public int? FromAccountId { get; set; }
        public int? ToAccountId { get; set; }

        public decimal Amount { get; set; }

        public string TransactionType { get; set; }

        public string Status { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Account FromAccount { get; set; }
        public Account ToAccount { get; set; }

        public string? ExternalBankName { get; set; }
        public string? ExternalAccountNumber { get; set; }
        public string TransferType { get; set; }
        public string? ReceiverName { get; set; }

        public string? SenderName { get; set; }
    }
}
