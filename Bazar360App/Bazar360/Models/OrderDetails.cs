using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bazar360.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }

        [Display(Name ="Order")]
        public int OrderId { get; set; }

        [Display(Name ="Product")]
        public int PrductId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [ForeignKey("PrductId")]
        public Product Product { get; set; }

    }
}
