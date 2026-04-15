using SmartShelf.web.DTOs.Dashboard;
using SmartShelf.web.Interfaces;

public class AlertService : IAlertService
{
    private readonly IInventoryService _inventoryService;

    public AlertService(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public List<AlertDto> GetAlerts()
    {
        var inventory = _inventoryService.GetInventory();
        var alerts = new List<AlertDto>();
        int nextId = 1;

        foreach (var item in inventory)
        {
            if (item.Count == 0)
            {
                alerts.Add(new AlertDto
                {
                    Id = nextId++,
                    ProductName = item.ProductName,
                    Message = "Stock dropped to 0",
                    Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
                });
            }
            else if (item.IsLowStock)
            {
                alerts.Add(new AlertDto
                {
                    Id = nextId++,
                    ProductName = item.ProductName,
                    Message = $"Stock is low ({item.Count} remaining)",
                    Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
                });
            }
        }

        return alerts;
    }
}