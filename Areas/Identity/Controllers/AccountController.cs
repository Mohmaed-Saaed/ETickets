using ETickets.Models;
using ETickets.ModelView;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;


namespace ETickets.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IEmailSender _EmailSender;

        public AccountController(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _UserManager = userManager;
            _EmailSender = emailSender;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            // autoMapping
            var user = registerVM.Adapt<ApplicationUser>();

            var result = await _UserManager.CreateAsync(user, registerVM.Password);

            if (result.Succeeded)
            {
                //Send confirmation email.
                var token = await _UserManager.GenerateEmailConfirmationTokenAsync(user);

                var link = Url.Action("ConfrimEmail", "Account", new { userId = user.Id,  token, area = "Identity" }, Request.Scheme);  // Request.Scheme the protocal

                await _EmailSender.SendEmailAsync(user.Email ?? "", "Confirm your account", $"<h1> Confirm your account by clicking <a href='{link}'> here </ a> </ h1>");

                TempData["Success"] = "Please confirm your email";

            } else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerVM);
            }

            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        public async Task<IActionResult> ConfrimEmail(string userId, string token)
        {

            var user = await _UserManager.FindByIdAsync(userId);


            if (user is not null)
            {

                var result = await _UserManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    TempData["success"] = "Email confirmed successfully";
                }
                else
                {
                    TempData["error"] = $"{string.Join(", " ,result.Errors)}";
                }
            }

            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

    } 
}
