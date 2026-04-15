using SmartShelf.web.DTOs.Dashboard;

namespace SmartShelf.web.Interfaces
{
    public interface IAlertService
    {
        List<AlertDto> GetAlerts();
    }
}