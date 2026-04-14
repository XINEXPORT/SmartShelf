namespace SmartShelf.web.DTOs.Dashboard
{
    public class DashboardDto
    {
        public DashboardSummaryDto Summary { get; set; } = new();
        public List<InventoryItemDto> Inventory { get; set; } = new();
        public List<AlertDto> Alerts { get; set; } = new();
        public DashboardStatusDto Status { get; set; } = new();
    }
}