using Bazar360.Data;
using Bazar360.Models;
using Bazar360.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Bazar360.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order anOrder)
        {
            var products = HttpContext.Session.Get<List<Product>>("products");
            if (products != null)
            {
                foreach (var product in products)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.PrductId=product.Id;
                    anOrder.OrderDetails.Add(orderDetails);
                }
            }
            anOrder.OrderNo = GetOrderNo(); 
            _db.Orders.Add(anOrder);
            await _db.SaveChangesAsync();
            HttpContext.Session.Set("products", null);
            return View();
        }

        public string GetOrderNo()
        {
            var orderCount = _db.Orders.Count()+1;
            return orderCount.ToString("0");
        }




    }
}
