using SmartShelf.web.Data;
using SmartShelf.web.Interfaces;
using SmartShelf.web.DTOs.Dashboard;


namespace SmartShelf.web.Services
{
    public class DashboardService
    {
        private readonly SmartShelfContext _context;

        public DashboardService(SmartShelfContext context)
        {
            _context = context;
        }

        public DashboardDto GetDashboard()
        {
            // Build the dashboard 
            var dashboard = new DashboardDto
            {
                Summary = new DashboardSummaryDto(),
                Inventory = new List<InventoryItemDto>(),
                Alerts = new List<AlertDto>(),
                Status = new DashboardStatusDto()
            };

            return dashboard;
        }
    }
}