namespace SmartShelf.web.DTOs.Dashboard
{
    public class AlertDto
    {
        public int AlertId { get; set; }

        public string Type { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}