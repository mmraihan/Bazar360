using Bazar360.Data;
using Bazar360.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bazar360.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.ProductTypes.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes productTypes)
        {
            if (ModelState.IsValid) //Server Side Valid
            {
                _db.ProductTypes.Add(productTypes);
                await _db.SaveChangesAsync();

                TempData["save"] = "Product saved successfully"; //Alertify

                return RedirectToAction(actionName: nameof(Index));
            }
            return View(productTypes);
        }


        public ActionResult Edit(int? id)
        {
            if (id ==null)
            {
                return NotFound();
            }
            var productTypes = _db.ProductTypes.Find(id);
            if (productTypes==null)
            {
                return NotFound();
            }
            return View(productTypes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypes productTypes)
        {
            if (ModelState.IsValid) //Server Side Valid
            {
                _db.ProductTypes.Update(productTypes);
                await _db.SaveChangesAsync();
                TempData["edit"] = "Product updated successfully";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(productTypes);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productTypes = _db.ProductTypes.Find(id);
            if (productTypes == null)
            {
                return NotFound();
            }
            return View(productTypes);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productTypes = _db.ProductTypes.Find(id);
            if (productTypes == null)
            {
                return NotFound();
            }
            return View(productTypes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task <ActionResult> Delete(int? id, ProductTypes productTypes)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id != productTypes.Id)
            {
                return NotFound();
            }

            var productType = _db.ProductTypes.Find(id);
            if (productTypes == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.ProductTypes.Remove(productType);
                await _db.SaveChangesAsync();

                TempData["delete"] = "Product deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);
        }


    }
}


