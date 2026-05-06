namespace SmartShelf.web.DTOs.Dashboard
{
    public class IndividualInventoryItemDto
    {
        public int ProductId { get; set; }
        public string EPC { get; set; } = string.Empty;
        public int Rssi { get; set; }
        public string Shelf { get; set; } = string.Empty;
    }
}
