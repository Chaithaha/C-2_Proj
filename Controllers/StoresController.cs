using System;
using System.Diagnostics;
using CreativeColab.Data;
using CreativeColab.Models;
using CreativeColab.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CreativeColab.Controllers
{
    public class StoresController : Controller
    {
        private readonly PriceTrackerService _priceTrackerService;


        public StoresController(PriceTrackerService priceTrackerService)
        {
            _priceTrackerService = priceTrackerService;
        }

        // GET: Stores
        public async Task<IActionResult> Index()
        {
            var stores = await _priceTrackerService.GetAllStoresAsync();
            return View(stores);
        }

        // GET: Stores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stores/Create
        [HttpPost]
        public async Task<IActionResult> Create(Store store)
        {
            Debug.WriteLine(store.Name);
            Debug.WriteLine(store.Website);
            if (ModelState.IsValid)
            {
                await _priceTrackerService.AddStoreAsync(store);
                return RedirectToAction(nameof(Index));
            } else
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                Debug.WriteLine("Validation errors:");
                foreach (var err in errors)
                {
                    Debug.WriteLine(err);
                }

            }
            return NotFound();
        }

        // GET: Stores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var store = await _priceTrackerService.GetStoreByIdAsync(id.Value);
            if (store == null) return NotFound();
            return View(store);
        }

        // POST: Stores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StoreId,Name,Website")] Store store)
        {
            if (id != store.StoreId) return NotFound();
            if (ModelState.IsValid)
            {
                var updated = await _priceTrackerService.UpdateStoreAsync(store);
                if (!updated) return NotFound();
                return RedirectToAction(nameof(Index));
            }
            return View(store);
        }

        // POST: Stores/Delete (Called from popup confirm)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int storeId)
        {
            await _priceTrackerService.DeleteStoreAsync(storeId);
            return RedirectToAction(nameof(Index));
        }
    }
}
