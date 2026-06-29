using Microsoft.AspNetCore.Mvc;
using QAAutomationPortfolio.Extensions;
using QAAutomationPortfolio.Models;

namespace QAAutomationPortfolio.Controllers
{
    public class AdminAuthController : Controller
    {
        // Hardcoded credentials for demo — in production use ASP.NET Identity
        private const string AdminUsername = "admin";
        private const string AdminPassword = "nexus2026";

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") == "true")
                return RedirectToAction("Index", "Products");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AdminLoginModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Username == AdminUsername && model.Password == AdminPassword)
            {
                HttpContext.Session.SetString("AdminLoggedIn", "true");
                HttpContext.Session.SetString("AdminUsername", model.Username);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Products");
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminLoggedIn");
            HttpContext.Session.Remove("AdminUsername");
            return RedirectToAction("Index", "Home");
        }
    }
}
