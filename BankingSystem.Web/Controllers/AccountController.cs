using Microsoft.AspNetCore.Mvc;
using BankingSystem.BLL.Services;
using BankingSystem.Entities.Models;
using BankingSystem.DAL.Repositorie;
using static BankingSystem.DAL.Repositorie.IGenericRepository;

namespace BankingSystem.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IGenericRepository<Customer> _customerRepo;

        public AccountController(IAccountService accountService,
                                 IGenericRepository<Customer> customerRepo)
        {
            _accountService = accountService;
            _customerRepo = customerRepo;
        }

        // GET: Account List
        public async Task<IActionResult> Index()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return View(accounts);
        }

        // GET: Create Account
        public async Task<IActionResult> Create()
        {
            ViewBag.Customers = await _customerRepo.GetAllAsync();
            return View();
        }

        // POST: Create Account
        [HttpPost]
        public async Task<IActionResult> Create(Account account)
        {
            try
            {
                await _accountService.CreateAccountAsync(account);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Customers = await _customerRepo.GetAllAsync();
                return View(account);
            }
        }
    }
}