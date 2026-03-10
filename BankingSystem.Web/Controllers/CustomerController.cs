using Microsoft.AspNetCore.Mvc;
using BankingSystem.BLL.Services;
using BankingSystem.Entities.Models;
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

        // Customer List
        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return View(customers);
        }

        // Create Customer
        public IActionResult Create()
        {
            return View();
        }

        // Delete Customer
        public async Task<IActionResult> Delete(int id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return RedirectToAction("Index");
        }

        // GET Edit
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // POST Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Customer customer, IFormFile ImageFile)
        {
            if (ImageFile != null)
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/customers");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                customer.ProfileImage = fileName;
            }

            await _customerService.UpdateCustomerAsync(customer);

            return RedirectToAction("Index");
        }
    }
}