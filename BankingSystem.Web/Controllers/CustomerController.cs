using Microsoft.AspNetCore.Mvc;
using BankingSystem.BLL.Services;
using BankingSystem.Entities.Models;
using BankingSystem.DAL.Repositorie;
using Microsoft.AspNetCore.Authorization;
namespace BankingSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: Customer List
        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return View(customers);
        }

        // GET: Create Customer
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create Customer
        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _customerService.CreateCustomerAsync(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }
    }
}
