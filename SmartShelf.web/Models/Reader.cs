using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartShelf.web.Models
{
    public class Reader
    {
        public int Id { get; set; }

        public string Location { get; set; } = string.Empty;

        //Relationships
        public ICollection<TagReadEvent> TagReadEvents { get; set; } = new List<TagReadEvent>();

        public ICollection<TagCurrentState> TagCurrentStates { get; set; } = new List<TagCurrentState>();
    }
}