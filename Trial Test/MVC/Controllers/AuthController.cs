using BusinessLogic.IService;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IStoreAccountService _storeAccountService;
		private readonly ILogger<AuthController> _logger;

		public AuthController(IStoreAccountService storeAccountService, ILogger<AuthController> logger)
		{
			_storeAccountService = storeAccountService;
			_logger = logger;
		}

		// GET: Display Login Form
		[HttpGet("login")]
		public IActionResult Login()
		{
			return View(); // Returns the login view
		}

		// POST: Handle Login
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromForm] string email, string password)
		{
			StoreAccount? user = await _storeAccountService.Login(email, password);

			if (user == null)
			{
				_logger.LogInformation("You do not have permission to do this function!");
				TempData["ErrorMessage"] = "You do not have permission to do this function!";
				return RedirectToAction("Login");
			}
			HttpContext.Session.SetString("userRole", user.Role.ToString());

			return RedirectToAction("Index", "MedicineInformations");
		}
	}
}
