using BusinessLogic.IService;
using Data.PaggingItem;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.InteropServices;

namespace MVC.Controllers
{
    public class MedicineInformationsController : Controller
    {
        private readonly IMedicineService _medicineService;
        private readonly IManufacturerService _manufacturerService;

        private const string MANAGER_ROLE = "2";

        public MedicineInformationsController(IMedicineService medicineService, IManufacturerService manufacturerService)
        {
            _medicineService = medicineService;
            _manufacturerService = manufacturerService;
        }


        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 3, string? nameSearch = null)
        {
            var userRole = HttpContext.Session.GetString("userRole");

            TempData["userRole"] = userRole;
            PaginatedList<MedicineInformation> medicineList = await _medicineService.GetAllMedicine(pageNumber, pageSize, nameSearch);
            return View(medicineList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ICollection<Manufacturer> manufacturers = await _manufacturerService.GetManufacturers();

            var manufacturerList = manufacturers.Select(m => new SelectListItem
            {
                Value = m.ManufacturerId,
                Text = m.ManufacturerName
            }).ToList();
            ViewBag.ManufacturerId = manufacturerList;

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(MedicineInformation medicineInformation)
        {
            try
            {
                var userRole = HttpContext.Session.GetString("userRole");

                if (userRole != MANAGER_ROLE)
                {
                    return RedirectToAction("Login");
                }

                await _medicineService.CreateMedicine(medicineInformation);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("ActiveIngredients", ex.Message);

                var manufacturers = await _manufacturerService.GetManufacturers();
                ViewBag.ManufacturerId = manufacturers.Select(m => new SelectListItem
                {
                    Value = m.ManufacturerId,
                    Text = m.ManufacturerName
                }).ToList();

                return View(medicineInformation);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            MedicineInformation medicine = await _medicineService.GetMedicine(id);

            if (medicine == null)
            {
                return NotFound();
            }

            ICollection<Manufacturer> manufacturers = await _manufacturerService.GetManufacturers();

            var manufacturerList = manufacturers.Select(m => new SelectListItem
            {
                Value = m.ManufacturerId,
                Text = m.ManufacturerName
            }).ToList();
            ViewBag.ManufacturerId = manufacturerList;

            
            return View(medicine);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(MedicineInformation medicineInformation)
        {
            try
            {
                var userRole = HttpContext.Session.GetString("userRole");

                if (userRole != MANAGER_ROLE)
                {
                    return RedirectToAction("Login");
                }

                await _medicineService.UpdateMedicine(medicineInformation);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ActiveIngredients", ex.Message);

                var manufacturers = await _manufacturerService.GetManufacturers();
                ViewBag.ManufacturerId = manufacturers.Select(m => new SelectListItem
                {
                    Value = m.ManufacturerId,
                    Text = m.ManufacturerName
                }).ToList();

                return View(medicineInformation);
                throw;
            }

        }
    }
}
