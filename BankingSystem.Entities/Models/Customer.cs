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
        [Required]
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "Aadhaar must be 12 digits")]
        public string AadhaarNumber { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Enter valid PAN")]
        public string PanNumber { get; set; }

        public string? Phone { get; set; }

        public bool KYCStatus { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? ProfileImage { get; set; }

        public ICollection<Account>? Accounts { get; set; }
    }
}
