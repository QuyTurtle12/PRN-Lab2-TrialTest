using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccess.Entities;
using BusinessLogic.IService;
using Data.PaggingItem;
using Microsoft.AspNetCore.Mvc;

namespace PharmaceuticalManagement_QUYNNSE171134.Pages.Medicines
{
    public class IndexModel : PageModel
    {
        private readonly IMedicineService _medicineService;

        public IndexModel(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        public PaginatedList<MedicineInformation> MedicineInformationList { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync(int index = 1, int pageSize = 3)
        {
            MedicineInformationList = await _medicineService.GetAllMedicine(index, pageSize, null);

            return Page();
        }
    }
}
