using Microsoft.AspNetCore.Mvc;
using BankingSystem.BLL.Services;
using Microsoft.AspNetCore.Authorization;

namespace BankingSystem.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalCustomers = await _dashboardService.GetTotalCustomersAsync();
            ViewBag.TotalAccounts = await _dashboardService.GetTotalAccountsAsync();
            ViewBag.TotalTransactions = await _dashboardService.GetTotalTransactionsAsync();
            ViewBag.TotalBalance = await _dashboardService.GetTotalBalanceAsync();

            return View();
        }
    }
}