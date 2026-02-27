using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Entities.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        public string? Phone { get; set; }

        public bool KYCStatus { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<Account>? Accounts { get; set; }
    }
}
