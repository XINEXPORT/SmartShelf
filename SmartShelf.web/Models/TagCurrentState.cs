using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartShelf.web.Models
{
    public class TagCurrentState
    {
        [Key]
        public string EPC { get; set; } = null!;

        [Required]
        public int ReaderId { get; set; }

        [Required]
        public int Antenna { get; set; }

        [Required]
        public int Rssi { get; set; }

        [Required]
        public DateTime LastSeenTimestamp { get; set; }

        //Relationships
        [ForeignKey(nameof(EPC))]
        public Tag Tag { get; set; } = null!;

        [ForeignKey(nameof(ReaderId))]
        public Reader Reader { get; set; } = null!;
    }
}