using Azure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ThingMagic;

namespace SmartShelf.web.Models
{
    public class TagReadEvent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string EPC { get; set; } = null!;

        [Required]
        public int ReaderId { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public int Antenna { get; set; }

        [Required]
        public int Rssi { get; set; }

        [ForeignKey(nameof(EPC))]
        public Tag Tag { get; set; } = null!;

        [ForeignKey(nameof(ReaderId))]
        public Reader Reader { get; set; } = null!;
    }
}