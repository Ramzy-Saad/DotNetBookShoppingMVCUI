using BookShoppingMVCUI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingMVCUI.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class ReportsController : Controller
    {
        private readonly IReportRepository _reportRepository;

        public ReportsController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<ActionResult> TopFiveSellingBooks(DateTime? sDate=null, DateTime? eDate=null)
        {
            try
            {
                DateTime startDate = sDate ?? DateTime.UtcNow.AddDays(-30);
                DateTime endDate = eDate ?? DateTime.UtcNow;
                var topFiveSoldBooks = await _reportRepository.GetTopNSoldBooksAsync(startDate, endDate);
                var vm= new TopNSoldBooksVm(startDate, endDate, topFiveSoldBooks);
                return View(vm);
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
