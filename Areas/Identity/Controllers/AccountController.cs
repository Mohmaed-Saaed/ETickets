using Microsoft.AspNetCore.Mvc;

namespace ETickets.Areas.Identity.Controllers
{
    public class AccountController : Controller
    {
        [Area("Identity")]
        public IActionResult Register()
        {
            return View();
        }
            
    }
}
