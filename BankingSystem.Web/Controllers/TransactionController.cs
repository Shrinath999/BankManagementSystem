using Microsoft.AspNetCore.Mvc;
using BankingSystem.BLL.Services;
using BankingSystem.DAL.Repositorie;  
using BankingSystem.Entities.Models;
using static BankingSystem.DAL.Repositorie.IGenericRepository;
using Microsoft.AspNetCore.Authorization;

namespace BankingSystem.Web.Controllers
{
    [Authorize(Roles = "Admin,Teller")]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IGenericRepository<Account> _accountRepo;

        public TransactionController(
            ITransactionService transactionService,
            IGenericRepository<Account> accountRepo)
        {
            _transactionService = transactionService;
            _accountRepo = accountRepo;
        }

        // GET: Deposit Page
        public async Task<IActionResult> Deposit()
        {
            ViewBag.Accounts = await _accountRepo.GetAllAsync();
            return View();
        }

        // POST: Deposit
        [HttpPost]
        public async Task<IActionResult> Deposit(int accountId, decimal amount)
        {
            try
            {
                await _transactionService.DepositAsync(accountId, amount);
                return RedirectToAction("Deposit");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Accounts = await _accountRepo.GetAllAsync();
                return View();
            }
        }

        // GET: Withdraw Page
        public async Task<IActionResult> Withdraw()
        {
            ViewBag.Accounts = await _accountRepo.GetAllAsync();
            return View();
        }

        // POST: Withdraw
        [HttpPost]
        public async Task<IActionResult> Withdraw(int accountId, decimal amount)
        {
            try
            {
                await _transactionService.WithdrawAsync(accountId, amount);
                return RedirectToAction("Withdraw");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Accounts = await _accountRepo.GetAllAsync();
                return View();
            }
        }


        // GET: Transfer Page
        public async Task<IActionResult> Transfer()
        {
            ViewBag.Accounts = await _accountRepo.GetAllAsync();
            return View();
        }

        // POST: Transfer
        [HttpPost]
        public async Task<IActionResult> Transfer(int fromAccountId, int toAccountId, decimal amount)
        {
            try
            {
                await _transactionService.TransferAsync(fromAccountId, toAccountId, amount);
                return RedirectToAction("Transfer");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Accounts = await _accountRepo.GetAllAsync();
                return View();
            }
        }
        // GET: Statement
        public async Task<IActionResult> Statement(int? accountId, DateTime? fromDate, DateTime? toDate)
        {
            ViewBag.Accounts = await _accountRepo.GetAllAsync();

            if (accountId == null)
                return View();

            var statement = await _transactionService.GetAccountStatementAsync(accountId.Value);

            if (fromDate.HasValue)
            {
                statement = statement.Where(t => t.CreatedDate >= fromDate.Value.Date);
            }

            if (toDate.HasValue)
            {
                var endDate = toDate.Value.Date.AddDays(1).AddTicks(-1);
                statement = statement.Where(t => t.CreatedDate <= endDate);
            }

            statement = statement.OrderBy(t => t.CreatedDate);

            
            var account = await _accountRepo.GetByIdAsync(accountId.Value);

            ViewBag.SelectedAccount = accountId.Value;
            ViewBag.CurrentBalance = account.Balance;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;

            return View(statement);
        }
        public async Task<IActionResult> ExportStatement(int accountId, DateTime? fromDate, DateTime? toDate)
        {
            var statement = await _transactionService.GetAccountStatementAsync(accountId);

            if (fromDate.HasValue)
                statement = statement.Where(t => t.CreatedDate >= fromDate.Value.Date);

            if (toDate.HasValue)
            {
                var endDate = toDate.Value.Date.AddDays(1).AddTicks(-1);
                statement = statement.Where(t => t.CreatedDate <= endDate);
            }

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Date,Type,Debit,Credit");

            foreach (var txn in statement)
            {
                decimal debit = txn.FromAccountId == accountId ? txn.Amount : 0;
                decimal credit = txn.ToAccountId == accountId ? txn.Amount : 0;

                sb.AppendLine($"{txn.CreatedDate:dd-MM-yyyy HH:mm},{txn.TransactionType},{debit},{credit}");
            }

            return File(
                System.Text.Encoding.UTF8.GetBytes(sb.ToString()),
                "text/csv",
                "AccountStatement.csv");
        }
    }
}