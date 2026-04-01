using System.ComponentModel.DataAnnotations;

namespace SmartShelf.web.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }  

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public int Threshold { get; set; }

        //Relationships
        // one product → many tags
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}