﻿using Bazar360.Data;
using Bazar360.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bazar360.Areas.Admin.Controllers
{
    [Area("Admin")]
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
    }
}


