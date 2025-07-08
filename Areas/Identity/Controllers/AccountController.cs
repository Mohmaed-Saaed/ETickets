using ETickets.Models;
using ETickets.ModelView;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Threading.Tasks;


namespace ETickets.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IEmailSender _EmailSender;
        private readonly SignInManager<ApplicationUser> _SignInManager;
        public AccountController(UserManager<ApplicationUser> userManager, IEmailSender emailSender, SignInManager<ApplicationUser> signInManager)
        {
            _UserManager = userManager;
            _EmailSender = emailSender;
            _SignInManager = signInManager;

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
                await SendToken(user);
                TempData["Success"] = "Confirm your email";
            } else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerVM);
            }

            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public async Task SendToken(ApplicationUser user)
        {

            var token = await _UserManager.GenerateEmailConfirmationTokenAsync(user);

            var link = Url.Action("ConfrimEmail", "Account", new { userId = user.Id, token, area = "Identity" }, Request.Scheme);  // Request.Scheme the protocal

            await _EmailSender.SendEmailAsync(user.Email ?? "", "Confirm your account", $"<h1> Confirm your account by clicking <a href='{link}'> here </ a> </ h1>");

         
        }

        public async Task<JsonResult> SendEmailConfirmationByUserNameOrEmail(string UserNameOrEmail)
        {
            var user = await _UserManager.FindByEmailAsync(UserNameOrEmail);

            user ??= await _UserManager.FindByNameAsync(UserNameOrEmail);

            if(user is not null)
            {
               await SendToken(user);
                TempData["Success"] = "Confirm your email";
                return new JsonResult(new { status = true });
            }
            TempData["Success"] = "Somthing went wrong!";
            return new JsonResult(new { status = false });
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginVM());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        
        
        {
            var user = await _UserManager.FindByEmailAsync(loginVM.UserNameOrEmail);

            user ??= await _UserManager.FindByNameAsync (loginVM.UserNameOrEmail);

            if (user is not null)
            {
                var result = await _UserManager.CheckPasswordAsync(user, loginVM.Password);

                if (result)
                {
                    if(!user.EmailConfirmed)
                    {
                        TempData["error"] = "Confirm your email.";
                        loginVM.EmailConfirmed = false;
                        TempData["UserNameOrEmail"] = loginVM.UserNameOrEmail;
                        return View(loginVM);
                    }
                    if (!user.LockoutEnabled)
                    {
                        TempData["error"] = $"Account blocked. {user.LockoutEnd}.";
                        return View(loginVM);
                    }

                    TempData["success"] = "Logined in.";
                    await _SignInManager.SignInAsync(user, loginVM.RememberMe);
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
            }

            TempData["error"] = "User name or email is wrong.";
            ModelState.AddModelError(string.Empty, "User name or email is wrong.");
            return View(loginVM);            
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

            return RedirectToAction(nameof(EmailConfrimed));
        }

        public IActionResult EmailConfrimed()
        {
            return View();
        }

        public  new async Task<IActionResult>  SignOut()
        {
            await _SignInManager.SignOutAsync();
            TempData["seccuess"] = "Signed out.";
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

    } 
}
