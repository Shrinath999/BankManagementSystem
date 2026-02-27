using BankingSystem.DAL.Repositorie;
using BankingSystem.Entities.Models;
using static BankingSystem.DAL.Repositorie.IGenericRepository;

namespace BankingSystem.BLL.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IGenericRepository<Customer> _customerRepo;
        private readonly IGenericRepository<Account> _accountRepo;
        private readonly IGenericRepository<Transaction> _transactionRepo;

        public DashboardService(
            IGenericRepository<Customer> customerRepo,
            IGenericRepository<Account> accountRepo,
            IGenericRepository<Transaction> transactionRepo)
        {
            _customerRepo = customerRepo;
            _accountRepo = accountRepo;
            _transactionRepo = transactionRepo;
        }

        public async Task<int> GetTotalCustomersAsync()
        {
            var customers = await _customerRepo.GetAllAsync();
            return customers.Count();
        }

        public async Task<int> GetTotalAccountsAsync()
        {
            var accounts = await _accountRepo.GetAllAsync();
            return accounts.Count();
        }

        public async Task<int> GetTotalTransactionsAsync()
        {
            var transactions = await _transactionRepo.GetAllAsync();
            return transactions.Count();
        }

        public async Task<decimal> GetTotalBalanceAsync()
        {
            var accounts = await _accountRepo.GetAllAsync();
            return accounts.Sum(a => a.Balance);
        }
    }
}