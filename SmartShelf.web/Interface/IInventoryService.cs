using SmartShelf.web.DTOs;
using SmartShelf.web.DTOs.Dashboard;

namespace SmartShelf.web.Interfaces
{
    public interface IInventoryService
    {
        List<InventoryItemDto> GetInventory();
    }
}