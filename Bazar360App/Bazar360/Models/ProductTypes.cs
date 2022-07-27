using System.ComponentModel.DataAnnotations;

namespace Bazar360.Models
{
    public class ProductTypes
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Product Type")]
        public string? ProductType { get; set; }
    }
}
