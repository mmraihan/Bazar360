using Bazar360.Data;
using Bazar360.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bazar360.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index( decimal? lowAmount, decimal? largeAmount)
        {
            var products = _db.Products.Include(c => c.ProductTypes).Include(p => p.SpecialTag).Where(c=>c.Price>=lowAmount && c.Price<= largeAmount).ToList();
            if (lowAmount==null || largeAmount==null)
            {
                products = _db.Products.Include(c => c.ProductTypes).Include(p => p.SpecialTag).ToList();
                return View(products);
            }
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
                #region Dupilicate Product Removed

                var searchProduct = _db.Products.FirstOrDefault(c => c.Name == product.Name);
                if (searchProduct != null)
                {
                    ViewBag.message = "This product is already exist";
                    ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
                    ViewData["specialTagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
                   
                    return View(product);
                }

                #endregion



                if (product.ImageFile != null)
                {
                    string folder = "Images/";
                    folder +=Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                    string serverFolder = Path.Combine(_webHost.WebRootPath, folder);
                    product.ImageUrl = "/" + folder; //save url to db

                    await product.ImageFile.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                 
                }

                if (product.ImageFile==null)
                {
                    product.ImageUrl = "/Images/No-image-found.jpg";
                }
                _db.Products.Add(product);
                await _db.SaveChangesAsync();

                TempData["save"] = "Product saved successfully"; //Alertify

                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        public ActionResult Edit(int? id)
        {
           

            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["specialTagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");

            if (id==null)
            {
                return NotFound();
            }

            var product = _db.Products.Include(c=>c.ProductTypes).Include(c=>c.SpecialTag).FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid) //Server Side Valid
            {
                if (product.ImageFile != null)
                {
                    string folder = "Images/";
                    folder += Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                    string serverFolder = Path.Combine(_webHost.WebRootPath, folder);
                    product.ImageUrl = "/" + folder; //save url to db

                    await product.ImageFile.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

                }

                if (product.ImageFile == null)
                {
                    product.ImageUrl = "/Images/No-image-found.jpg";
                }
                _db.Products.Add(product);
                await _db.SaveChangesAsync();

                TempData["save"] = "Product updated successfully"; //Alertify

                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public ActionResult Details (int? id)
        {
            if (id ==null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c=>c.ProductTypes).Include(c=>c.SpecialTag).FirstOrDefault(x => x.Id == id);

            if (product==null)
            {
                return NotFound();
            }
            return View(product);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Delete(int? id, Product product)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id != product.Id)
            {
                return NotFound();
            }

            var productFromDb = _db.Products.Find(id);
            if (productFromDb == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Products.Remove(productFromDb);
                await _db.SaveChangesAsync();

                TempData["delete"] = "Product deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }





    }
}
