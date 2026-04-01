using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartShelf.web.Models
{
    public class Tag
    {
        [Key]
        public string EPC { get; set; } = null!;  

        [Required]
        public int ProductId { get; set; }        

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        // one tag → many read events
        public ICollection<TagReadEvent> TagReadEvents { get; set; } = new List<TagReadEvent>();
    }
}