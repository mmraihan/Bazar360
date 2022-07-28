using Bazar360.Data;
using Microsoft.AspNetCore.Mvc;

namespace Bazar360.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db=db;
        }
        public IActionResult Index()
        {
            var products = _db.Products.ToList();
            return View(products);
        }


    }
}
