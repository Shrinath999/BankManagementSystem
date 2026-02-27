using BankingSystem.Entities.Models;

namespace BankingSystem.BLL.Services
{
    public interface ITransactionService
    {
        Task DepositAsync(int accountId, decimal amount);
        Task WithdrawAsync(int accountId, decimal amount);
        Task TransferAsync(int fromAccountId, int toAccountId, decimal amount);
        Task<IEnumerable<Transaction>> GetAccountStatementAsync(int accountId);
    }
}