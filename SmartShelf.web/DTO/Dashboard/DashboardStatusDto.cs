namespace SmartShelf.web.DTOs.Dashboard
{
    public class DashboardStatusDto
    {
        public bool BackendOnline { get; set; }
        public bool ReaderConnected { get; set; }

        public DateTime? LastSuccessfulScan { get; set; }

        public bool SensorAvailable { get; set; }
        public string SensorMessage { get; set; } = string.Empty;
    }
}