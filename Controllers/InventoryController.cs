using Microsoft.AspNetCore.Mvc;
using AIOS.Models;
using AIOS.Repositories;

namespace AIOS.Controllers
{
    public class InventoryController : Controller
    {
        private readonly VehicleRepository _repo;

        public InventoryController(VehicleRepository repo)
        {
            _repo = repo;
        }

        // ----------------------------------------
        // GET: /Inventory
        // ----------------------------------------
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vehicles = await _repo.GetAllAsync();
            return View(vehicles);
        }

        // ----------------------------------------
        // GET: /Inventory/Add
        // ----------------------------------------
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // ----------------------------------------
        // POST: /Inventory/Add
        // ----------------------------------------
        [HttpPost]
        public async Task<IActionResult> Add(Vehicle vehicle, IFormFile photo)
        {
            if (!ModelState.IsValid)
                return View(vehicle);

            // Upload photo
            if (photo != null)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                string fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                string filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                vehicle.PhotoUrl = "/uploads/" + fileName;
            }

            await _repo.AddAsync(vehicle);

            return RedirectToAction("Index");
        }

        // ----------------------------------------
        // GET: /Inventory/Details/{id}
        // ----------------------------------------
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var vehicle = await _repo.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }

        // ----------------------------------------
        // GET: /Inventory/Edit/{id}
        // ----------------------------------------
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var vehicle = await _repo.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }

        // ----------------------------------------
        // POST: /Inventory/Edit
        // ----------------------------------------
        [HttpPost]
        public async Task<IActionResult> Edit(Vehicle vehicle, IFormFile? newPhoto)
        {
            if (!ModelState.IsValid)
                return View(vehicle);

            if (newPhoto != null)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                string fileName = Guid.NewGuid() + Path.GetExtension(newPhoto.FileName);
                string filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await newPhoto.CopyToAsync(stream);
                }

                vehicle.PhotoUrl = "/uploads/" + fileName;
            }

            await _repo.UpdateAsync(vehicle);
            return RedirectToAction("Details", new { id = vehicle.Id });
        }

        // ----------------------------------------
        // GET: /Inventory/Delete/{id}
        // ----------------------------------------
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var vehicle = await _repo.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound();

            return View(vehicle);
        }

        // ----------------------------------------
        // POST: /Inventory/Delete
        // ----------------------------------------
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
