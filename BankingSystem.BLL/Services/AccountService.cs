using BankingSystem.DAL.Repositorie;

using BankingSystem.Entities.Models;
using static BankingSystem.DAL.Repositorie.IGenericRepository;

namespace BankingSystem.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IGenericRepository<Account> _accountRepo;

        public AccountService(IGenericRepository<Account> accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _accountRepo.GetAllAsync();
        }

        public async Task CreateAccountAsync(Account account)
        {
            // Business Rule – Minimum Balance
            if (account.AccountType == "Savings" && account.Balance < 1000)
                throw new Exception("Minimum balance for Savings account is 1000");

            if (account.AccountType == "Current" && account.Balance < 5000)
                throw new Exception("Minimum balance for Current account is 5000");

            // Generate Account Number
            account.AccountNumber = GenerateAccountNumber();

            account.CreatedDate = DateTime.Now;

            await _accountRepo.InsertAsync(account);
            await _accountRepo.SaveAsync();
        }

        private string GenerateAccountNumber()
        {
            var random = new Random();
            return random.Next(100000000, 999999999).ToString();
        }
    }
}