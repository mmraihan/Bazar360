using Bazar360.Data;
using Bazar360.Models;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Bazar360.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _he;

        public ProductController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }
        public IActionResult Index()
        {
            var products = _db.Products.ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile image)
        {
            if (ModelState.IsValid) //Server Side Valid
            {
                if (image !=null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));

                    await  image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image ="Images/" + image.FileName;
                }  
                _db.Products.Add(product);
                await _db.SaveChangesAsync();

                TempData["save"] = "Product saved successfully"; //Alertify

                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


    }
}
