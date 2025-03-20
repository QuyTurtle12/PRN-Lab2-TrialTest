using BusinessLogic.IService;
using Data.PaggingItem;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace MVC.Controllers
{
	public class MedicineInformationsController : Controller
	{
		private readonly IMedicineService _medicineService;

		private const string MANAGER_ROLE = "2";

		public MedicineInformationsController(IMedicineService medicineService)
		{
			_medicineService = medicineService;
		}


		[HttpGet]
		public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 3, string? nameSearch = null)
		{
			var userRole = HttpContext.Session.GetString("userRole");

			if (userRole == null)
			{
                return RedirectToAction("Login");
            }

            TempData["userRole"] = userRole;
            PaginatedList<MedicineInformation> medicineList = await _medicineService.GetAllMedicine(pageNumber, pageSize, nameSearch);
			return View(medicineList);
		}

		[HttpPost]
		public async Task<IActionResult> Create(MedicineInformation medicineInformation)
		{
			var userRole = HttpContext.Session.GetString("userRole");

			if (userRole != MANAGER_ROLE)
			{
				return RedirectToAction("Login");
			}

			await _medicineService.CreateMedicine(medicineInformation);
			return RedirectToAction("Index");
		}

		[HttpPut]
		public async Task<IActionResult> Edit(MedicineInformation medicineInformation)
		{
			var userRole = HttpContext.Session.GetString("userRole");

			if (userRole != MANAGER_ROLE)
			{
				return RedirectToAction("Login");
			}

			await _medicineService.UpdateMedicine(medicineInformation);
			return RedirectToAction("Index");
		}
	}
}
