using ETickets.Data;
using ETickets.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NPOI.OpenXmlFormats.Dml.Diagram;
using System.Threading.Tasks;

namespace ETickets.Utilities
{
    public class DbInitializer : IDbInitializer
    {

        private readonly ApplicationDbContext _Context;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly UserManager<ApplicationUser> _UserManager;
        public DbInitializer(ApplicationDbContext context,
                            RoleManager<IdentityRole> roleManager,
                            UserManager<ApplicationUser> userManager)
        {
            _Context = context;
            _RoleManager = roleManager;
            _UserManager = userManager;
        }

        public void Init()
        {
            _Context.Database.EnsureCreated();

            if (_Context.Database.GetPendingMigrations().Any())
            {
                _Context.Database.Migrate();
            }

            if(!_Context.Users.Any() || !_Context.Roles.Any())
            {
                _RoleManager.CreateAsync(new(SD.SuperAdmin)).GetAwaiter().GetResult();
                _RoleManager.CreateAsync(new(SD.Admin)).GetAwaiter().GetResult();
                _RoleManager.CreateAsync(new(SD.Employee)).GetAwaiter().GetResult();
                _RoleManager.CreateAsync(new(SD.Customer)).GetAwaiter().GetResult();

              var result =  _UserManager.CreateAsync(new ApplicationUser()
                {
                    FirstName = "Super",
                    LastName = "Admin",
                    UserName = "SuperAdmin",
                    Email = "muhameds913@gmail.com",
                    EmailConfirmed = true,
                }, "SuperAdmin1!").GetAwaiter().GetResult();

                if(result.Succeeded)
                {
                    var superAdmin = _UserManager.FindByNameAsync(SD.SuperAdmin).GetAwaiter().GetResult();

                    if (superAdmin is not null)
                    {
                        _UserManager.AddToRoleAsync(superAdmin, SD.SuperAdmin).GetAwaiter().GetResult();
                    }
                } 
              
            }

            if (!_Context.Days.Any()) {
                _Context.Days.Add(new Day { Name = "Sunday" });
                _Context.Days.Add(new Day { Name = "Monday" });
                _Context.Days.Add(new Day { Name = "Tuesday" });
                _Context.Days.Add(new Day { Name = "Wednesday" });
                _Context.Days.Add(new Day { Name = "Thursday" });
                _Context.Days.Add(new Day { Name = "Friday" });
                _Context.Days.Add(new Day { Name = "Saturday" });
                _Context.SaveChanges();
            }
        }
    }
}
