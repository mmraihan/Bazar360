using System.ComponentModel.DataAnnotations;

namespace Bazar360.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="Order No")]
        public string OrderNo { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name ="Phone No")]
        public string PhoneNo { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        public DateTime OrderDate { get; set; }
        public virtual List <OrderDetails> OrderDetails { get; set; }


    }
}
