using ETickets.Models;
using ETickets.ModelView;
using ETickets.Repositry.IRepositry;
using ETickets.Utilities;
using Mapster;
using Microsoft.AspNetCore.Authorization;
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
        private readonly SignInManager<ApplicationUser> _SignInManager;
        private readonly IApplicationUserOTPRepository _ApplicationUserOTPRepository;
        public AccountController(UserManager<ApplicationUser> userManager, IEmailSender emailSender,
            SignInManager<ApplicationUser> signInManager,
            IApplicationUserOTPRepository applicationUserOTPRepository)
        {
            _UserManager = userManager;
            _EmailSender = emailSender;
            _SignInManager = signInManager;
            _ApplicationUserOTPRepository = applicationUserOTPRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (IsUserLogedIn())
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (IsUserLogedIn())
            {
                return NotFound();
            }
            // autoMapping
            var user = registerVM.Adapt<ApplicationUser>();


            var result = await _UserManager.CreateAsync(user, registerVM.Password);

            if (result.Succeeded)
            {
                //Send confirmation email.
                 await SendToken(user);
                 await _UserManager.AddToRoleAsync(user, SD.Customer);
                TempData["Success"] = "Please confirm your email.Then, Login";
            }
            else
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
            if (!IsUserLogedIn())
            {
                var token = await _UserManager.GenerateEmailConfirmationTokenAsync(user);

                var link = Url.Action("ConfrimEmail", "Account", new { userId = user.Id, token, area = "Identity" }, Request.Scheme);  // Request.Scheme the protocal

                await _EmailSender.SendEmailAsync(user.Email ?? "", "Confirm your account", $"<h1> Confirm your account by clicking <a href='{link}'> here </ a> </ h1>");

            }

        }

        public async Task<JsonResult> SendEmailConfirmationByUserNameOrEmail(string UserNameOrEmail)
        {

            if (IsUserLogedIn())
            {
                return new JsonResult(new { status = false });
            }
            var user = await _UserManager.FindByEmailAsync(UserNameOrEmail);

            user ??= await _UserManager.FindByNameAsync(UserNameOrEmail);

            if (user is not null)
            {
                await SendToken(user);
                TempData["Success"] = "Confirm your email";
                return new JsonResult(new { status = true });
            }
            TempData["Success"] = "Somthing went wrong!";
            return new JsonResult(new { status = false });
        }

        [HttpGet]

        //[Authorize()]
        public IActionResult Login()
        {
            if (IsUserLogedIn())
            {
                return NotFound();
            }
            return View(new LoginVM());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (IsUserLogedIn())
            {
                return NotFound();
            }
            var user = await _UserManager.FindByEmailAsync(loginVM.UserNameOrEmail);

            user ??= await _UserManager.FindByNameAsync(loginVM.UserNameOrEmail);

            if (user is not null)
            {
                var result = await _UserManager.CheckPasswordAsync(user, loginVM.Password);

                if (result)
                {
                    if (!user.EmailConfirmed)
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

            TempData["error"] = "Username&Email or password wrong.";
            ModelState.AddModelError(string.Empty, "Username&Email or password wrong.");
            return View(loginVM);
        }

        public async Task<IActionResult> ConfrimEmail(string userId, string token)
        {
            if (IsUserLogedIn())
            {
                return NotFound();
            }

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
                    TempData["error"] = $"{string.Join(", ", result.Errors)}";
                }
            }

            return RedirectToAction(nameof(EmailConfrimed));
        }

        public IActionResult EmailConfrimed()
        {
            if (IsUserLogedIn())
            {
            return NotFound();
            }
                return View();
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            if (IsUserLogedIn())
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {
            if (IsUserLogedIn())
            {
                return NotFound();
            }
            var user = await _UserManager.FindByEmailAsync(forgetPasswordVM.UserNameOrEmail);

            user ??= await _UserManager.FindByNameAsync(forgetPasswordVM.UserNameOrEmail);

            if (user is not null)
            {
                int numberOfMaxOTPs = 4;

                var userOTPs = _ApplicationUserOTPRepository
                                .Get(X => X.ApplicationUserId == user.Id && X.SendDate.Day == DateTime.UtcNow.Day).Count();

                if (userOTPs > numberOfMaxOTPs)
                {
                    TempData["error"] = "You have reached max numbers of OTPs for today.";
                    return View(forgetPasswordVM);
                }

                var OTPNumber = new Random().Next(100000, 999999);

                await _EmailSender.SendEmailAsync(user.Email ?? "", "Reset password", $"<h1> This is your OTP number {OTPNumber} to reset your password </ h1>");

                _ApplicationUserOTPRepository.Create(new ApplicationUserOTP
                {
                    OTPNumber = OTPNumber,
                    SendDate = DateTime.UtcNow,
                    ValidTo = DateTime.UtcNow.AddMinutes(30),
                    Status = false,
                    ApplicationUserId = user.Id,
                    Reason = "ForgetPassword"
                });
                var userId = user.Id;
                return RedirectToAction(nameof(ResetPassword), "Account", new { area = "Identity", userId }); // This will redirect to ResetPassword with controller and area and the id of the user will be put in the url.
            }
            TempData["error"] = "Wrong Username or email";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string userId)
        {
            if (IsUserLogedIn())
            {
                return NotFound();
            }
            var result = await _UserManager.FindByIdAsync(userId);

            if (result is not null)
            {

                return View(new ResetPasswordVM() { UserId = userId });
            }

            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (IsUserLogedIn())
            {
                return NotFound();
            }

            var user = await _UserManager.FindByIdAsync(resetPasswordVM.UserId);

            if (user is not null)
            {
                var LastUserOTP = _ApplicationUserOTPRepository.Get(x => x.ApplicationUserId == user.Id).OrderBy(x => x.Id).LastOrDefault();

                if (LastUserOTP is not null)
                {
                    if (resetPasswordVM.OTPNumber == LastUserOTP.OTPNumber)
                    {
                        if (DateTime.UtcNow <= LastUserOTP.ValidTo && !LastUserOTP.Status)
                        {
                            var token = await _UserManager.GeneratePasswordResetTokenAsync(user); // we get the token for security reasons.So,No one can change user's password without a token.

                            var result = await _UserManager.ResetPasswordAsync(user, token, resetPasswordVM.Password);

                            if (result.Succeeded)
                            {
                                LastUserOTP.Status = true;
                                _ApplicationUserOTPRepository.Update(LastUserOTP);
                                TempData["success"] = "Password reset.";
                                return RedirectToAction(nameof(Login));

                            }
                            else
                            {
                                TempData["error"] = $"{string.Join(", ", result.Errors)}";
                            }
                        }
                    }
                    TempData["error"] = "Invalid or Expired OTP";
                    return View(resetPasswordVM);
                }
            }
            return NotFound();
        }

        public new async Task<IActionResult> SignOut()
        {
            if (IsUserLogedIn())
            {
                await _SignInManager.SignOutAsync();
                TempData["success"] = "Signed out.";
                return RedirectToAction("Index", "Home", new { area = "Customer" });
             
            }

            return NotFound();
        }


        public bool IsUserLogedIn()
        {
            return (User.Identity is not null && User.Identity.IsAuthenticated);
        }

    }
}
