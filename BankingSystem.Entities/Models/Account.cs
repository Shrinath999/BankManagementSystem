using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystem.Entities.Models
{
    public class Account
    {
        public int AccountId { get; set; }

        public int CustomerId { get; set; }

        [Required]
        public string AccountNumber { get; set; }

        [Required]
        public string AccountType { get; set; }

        public decimal Balance { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Customer Customer { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }
}
