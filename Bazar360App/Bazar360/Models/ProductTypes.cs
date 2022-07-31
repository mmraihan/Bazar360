using System.ComponentModel.DataAnnotations;

namespace Bazar360.Models
{
    public class ProductTypes
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Category")]
        public string? ProductType { get; set; }
    }
}
