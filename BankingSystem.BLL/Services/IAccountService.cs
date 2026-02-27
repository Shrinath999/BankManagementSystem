using BankingSystem.Entities.Models;

namespace BankingSystem.BLL.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task CreateAccountAsync(Account account);
    }
}