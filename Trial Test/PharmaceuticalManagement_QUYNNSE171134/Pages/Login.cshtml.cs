using BusinessLogic.IService;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PharmaceuticalManagement_QUYNNSE171134.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IStoreAccountService _storeAccountService;

        [BindProperty]
        public StoreAccount StoreAccountDTO { get; set; } = default!;

        public string ErrorMessage { get; set; }

        public LoginModel(IStoreAccountService storeAccountService)
        {
            _storeAccountService = storeAccountService;
        }

        public async Task<IActionResult> OnPost()
        {
            StoreAccount result = await _storeAccountService.Login(StoreAccountDTO.EmailAddress, StoreAccountDTO.StoreAccountPassword);

            if (result != null)
            {
                return RedirectToPage("/Medicines/Index");
            }

            return RedirectToPage("Login");

        }
    }
}
