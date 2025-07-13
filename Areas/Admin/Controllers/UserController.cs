using ETickets.Models;
using ETickets.ModelView;
using ETickets.Repositry.IRepositry;
using ETickets.Utilities;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace ETickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin},{SD.Employee}")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;

        public UserController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _UserManager = userManager;
            _RoleManager = roleManager;

        }

        public IActionResult Index()
        {
            //_RoleManager.rol
            var user = _UserManager.Users.ToList();
            
            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> Save(string? id)
        {
            RegisterVM registerVM = new();

            if (id is not null)
            {
                var user = _UserManager.Users.FirstOrDefault(x => x.Id == id);

                if (user is not null)
                {
                    registerVM = user.Adapt<RegisterVM>();
                    registerVM.IsPasswordHiden = true;

                    var roles = await _UserManager.GetRolesAsync(user);

                    registerVM.UserRoles = _RoleManager.Roles.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
                    registerVM.Roles = roles;

                    return View(registerVM);
                }
            }
            registerVM.UserRoles = _RoleManager.Roles.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Save(RegisterVM registerVM)
        {

            if (registerVM.Id is not null)
            {

              var user = await _UserManager.FindByIdAsync(registerVM.Id);

                if(user is not null)
                {
                    var userOldRoles = await _UserManager.GetRolesAsync(user);

                    if (userOldRoles is not null)
                    {
                        var UserUpdated = registerVM.Adapt(user);

                        var addUpdatedUserResult = await _UserManager.UpdateAsync(UserUpdated);

                        if(addUpdatedUserResult.Succeeded)
                        {
                            var rolesRemovedResult = await _UserManager.RemoveFromRolesAsync(user, userOldRoles);

                            if (rolesRemovedResult.Succeeded)
                            {
                                var addUpdatedRoelsResult = await _UserManager.AddToRolesAsync(user, registerVM.Roles);

                                if (addUpdatedRoelsResult.Succeeded)
                                {
                                    TempData["success"] = "User updated";

                                    return RedirectToAction(nameof(Index));

                                }
                            }
                        } else
                        {
                            ModelState.AddModelError(string.Empty, string.Join(", ", addUpdatedUserResult.Errors.Select(e => e.Description)));
                            registerVM.IsPasswordHiden = true;
                            registerVM.UserRoles = _RoleManager.Roles.Select(x => new SelectListItem() { Value = x.Name, Text = x.Name }).ToList();
                            return View(registerVM);
                        }
                    }
                }
            }
            else
            {
                var user = registerVM.Adapt<ApplicationUser>();

                user.EmailConfirmed = true;

                var result = await _UserManager.CreateAsync(user, registerVM.Password);

                if (result.Succeeded)
                {

                    if (registerVM.Roles is not null && registerVM.Roles.Count() > 0)
                    {

                        var roleResult = await _UserManager.AddToRolesAsync(user, registerVM.Roles);

                        if (roleResult.Succeeded)
                        {
                            TempData["success"] = "User added";
                            return RedirectToAction(nameof(Index));
                        }

                    }
                    else
                    {
                        TempData["error"] = "Role didn't add!";
                    }

                }
                else
                {
                    registerVM.UserRoles = _RoleManager.Roles.Select(x => new SelectListItem() { Value = x.Id, Text = x.Name }).ToList();
                    ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors.Select(e => e.Description)));
                    return View(registerVM);
                }
            }


            return RedirectToAction(nameof(Index));
        }

        //public IActionResult Delete(int id)
        //{
        //    var user = _UserManager.GetOne(c => c.Id == id);

        //    if (user is not null)
        //    {
        //        _UserManager.Delete(user);
        //    }

        //    return RedirectToAction(nameof(Index));
        //}
    }
}

