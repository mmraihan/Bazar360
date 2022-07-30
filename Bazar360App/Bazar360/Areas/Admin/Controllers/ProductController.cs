using Bazar360.Data;
using Bazar360.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bazar360.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHost;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHost)
        {
            _db = db;
            _webHost = webHost;
        }
        public IActionResult Index()
        {
            var products = _db.Products.Include(c=>c.ProductTypes).Include(p=>p.SpecialTag).ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["specialTagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid) //Server Side Valid
            {
                if (product.ImageFile != null)
                {
                    string folder = "Images/";
                    folder +=Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                    string serverFolder = Path.Combine(_webHost.WebRootPath, folder);
                    product.ImageUrl = "/" + folder; //save url to db

                    await product.ImageFile.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                 
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
