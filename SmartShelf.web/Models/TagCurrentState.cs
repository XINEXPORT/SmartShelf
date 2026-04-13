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
        public int ReadCount { get; set; }

        [Required]
        public int Frequency { get; set; }

        [Required]
        public bool IsPresent { get; set; }

        [Required]
        public int MissedScanCount { get; set; } = 0;

        [Required]
        public DateTime LastSeenTimestamp { get; set; }


        //Relationships
        [ForeignKey(nameof(EPC))]
        public Tag Tag { get; set; } = null!;

        [ForeignKey(nameof(ReaderId))]
        public Reader Reader { get; set; } = null!;
    }
}