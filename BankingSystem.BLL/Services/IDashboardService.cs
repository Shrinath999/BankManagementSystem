using BankingSystem.Entities.Models;

namespace BankingSystem.BLL.Services
{
    public interface IDashboardService
    {
        Task<int> GetTotalCustomersAsync();
        Task<int> GetTotalAccountsAsync();
        Task<int> GetTotalTransactionsAsync();
        Task<decimal> GetTotalBalanceAsync();

        Task<IEnumerable<Transaction>> GetRecentTransactionsAsync();
    }
}