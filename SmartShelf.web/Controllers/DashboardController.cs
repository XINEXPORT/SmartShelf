using Microsoft.AspNetCore.Mvc;
using SmartShelf.web.Services;

namespace SmartShelf.web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _dashboardService;

        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public IActionResult GetDashboard()
        {
            var dashboard = _dashboardService.GetDashboard();
            return Ok(dashboard);
        }
    }
}