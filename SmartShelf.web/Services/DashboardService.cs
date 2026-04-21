using SmartShelf.web.DTOs.Dashboard;
using SmartShelf.web.Interfaces;

namespace SmartShelf.web.Services
{
    public class DashboardService
    {
        private readonly ISummaryService _summaryService;
        private readonly IAlertService _alertService;

        public DashboardService(ISummaryService summaryService, IAlertService alertService)
        {
            _summaryService = summaryService;
            _alertService = alertService;
        }

        public DashboardDto GetDashboard()
        {
            return new DashboardDto
            {
                Summary = _summaryService.GetSummary(),
                Inventory = new List<InventoryItemDto>(),
                Alerts = _alertService.GetAlerts(),
                Status = new DashboardStatusDto()
            };
        }
    }
}