using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccess.Entities;

namespace PharmaceuticalManagement_QUYNNSE171134.Pages.Medicines
{
    public class CreateModel : PageModel
    {
        private readonly DataAccess.Entities.MyDbContext _context;

        public CreateModel(DataAccess.Entities.MyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerId");
            return Page();
        }

        [BindProperty]
        public MedicineInformation MedicineInformation { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.MedicineInformations.Add(MedicineInformation);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
