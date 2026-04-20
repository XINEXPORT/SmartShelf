using SmartShelf.web.Data;
using SmartShelf.web.Interfaces;
using SmartShelf.web.DTOs.Dashboard;


namespace SmartShelf.web.Services
{
    public class DashboardService
    {
        private readonly ISummaryService _summaryService;

        public DashboardService(ISummaryService summaryService)
        {
            _summaryService = summaryService;
        }

        public DashboardDto GetDashboard()
        {
            var dashboard = new DashboardDto
            {
                Summary = _summaryService.GetSummary(),
                Inventory = new List<InventoryItemDto>(),
                Alerts = new List<AlertDto>(),
                Status = new DashboardStatusDto()
            };

            return dashboard;
        }
    }
}