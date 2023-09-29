using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TheBugTracker.Extensions;
using TheBugTracker.Models;
using TheBugTracker.Services;
using TheBugTracker.Services.Interfaces;
using TheBugTracker.Models.Enums;

namespace TheBugTracker.Controllers
{
    public class HomeController : BTBaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBTProjectService _projectService;
        public HomeController(ILogger<HomeController> logger, IBTProjectService projectService)
        {
            _logger = logger;
            _projectService = projectService;
        }

        public IActionResult LandingPage()
        {
            return View();
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<JsonResult> GglProjectTickets()
        {

            List<Project> projects = await _projectService.GetAllProjectsByCompanyIdAsync(_companyId);

            List<object> chartData = new();
            chartData.Add(new object[] { "ProjectName", "TicketCount" });

            foreach (Project prj in projects)
            {
                chartData.Add(new object[] { prj.Name, prj.Tickets.Count() });
            }

            return Json(chartData);
        }

        [HttpPost]
        public async Task<JsonResult> GglProjectPriority()
        {
          

            List<Project> projects = await _projectService.GetAllProjectsByCompanyIdAsync(_companyId);

            List<object> chartData = new();
            chartData.Add(new object[] { "Priority", "Count" });


            foreach (string priority in Enum.GetNames(typeof(ProjectPriority)))
            {
                int priorityCount = (await _projectService.GetAllProjectsByPriorityAsync(_companyId, priority)).Count();
                chartData.Add(new object[] { priority, priorityCount });
            }

            return Json(chartData);
        }
    }
}