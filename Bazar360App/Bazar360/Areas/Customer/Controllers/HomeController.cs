using Bazar360.Data;
using Bazar360.Models;
using Bazar360.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using X.PagedList;

namespace Bazar360.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
       
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db = null)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index(int ? page)
        {
            var products = _db.Products.Include(c => c.ProductTypes).Include(c => c.ProductTypes).ToList().ToPagedList(page??1,pageSize:6); //--coalescing operator
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Details(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).Include(c=>c.SpecialTag).FirstOrDefault(c => c.Id == id);
            return View(product);
        }

        [HttpPost]
        [ActionName("Details")]
        public IActionResult ProductDetails(int? id)
        {
            List<Product> products = new List<Product>(); //Session

            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (product==null)
            {
                return NotFound();
            }
            products = HttpContext.Session.Get<List<Product>>("products");
            if (products==null)
            {
                products = new List<Product>();
            }
            products.Add(product);
            HttpContext.Session.Set("products", products); //Session
            return View(product);
        }

      

        public IActionResult Remove(int? id)
        {
            var products = HttpContext.Session.Get<List<Product>>("products");

            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        //[HttpPost]
        //[ActionName("Remove")]

        //public IActionResult RemoveFromCart(int? id)
        //{
        //    var products = HttpContext.Session.Get<List<Product>>("products");

        //    if (products!=null)
        //    {
        //        var product = products.FirstOrDefault(c => c.Id == id);
        //        if (product != null)
        //        {
        //            products.Remove(product);
        //            HttpContext.Session.Set("products", products);
        //        }
        //    }
           
        //    return RedirectToAction(nameof(Index));
        //}


        public IActionResult Cart()
        {
            var products = HttpContext.Session.Get<List<Product>>("products");
            if (products==null)
            {
                products = new List<Product>();
            }

            return View(products);
        }

    }
}