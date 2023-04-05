using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models
{
    public class Item
    {
        [Required]
        public string? ID { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Notes { get; set; }

        public bool Done { get; set; }
    }
}
