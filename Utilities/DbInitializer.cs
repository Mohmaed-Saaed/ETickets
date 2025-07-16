using ETickets.Data;
using ETickets.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NPOI.OpenXmlFormats.Dml.Diagram;
using NPOI.SS.Formula.Functions;
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

            if(!_Context.Users.Any())
            {
                _RoleManager.CreateAsync(new(SD.SuperAdmin)).GetAwaiter().GetResult();
                _RoleManager.CreateAsync(new(SD.Admin)).GetAwaiter().GetResult();
                _RoleManager.CreateAsync(new(SD.Employee)).GetAwaiter().GetResult();
                _RoleManager.CreateAsync(new(SD.Customer)).GetAwaiter().GetResult();

                _UserManager.CreateAsync(new ApplicationUser()
                {
                    FirstName = "Super",
                    LastName = "Admin",
                    UserName = "SuperAdmin",
                    Email = "muhameds913@gmail.com",
                    EmailConfirmed = true,
                }, "SuperAdmin1!").GetAwaiter().GetResult();

                var superAdmin = _UserManager.FindByNameAsync(SD.SuperAdmin).GetAwaiter().GetResult();

                if (superAdmin is not null)
                {
                    _UserManager.AddToRoleAsync(superAdmin, SD.SuperAdmin).GetAwaiter().GetResult();
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

            if (!_Context.Cinemas.Any()) {
                _Context.Cinemas.Add(new Cinema {Address = "Address 1" ,Name ="Cinema 1" ,NumberOfChairs = 10 });
                _Context.Cinemas.Add(new Cinema {Address = "Address 2" ,Name ="Cinema 2" ,NumberOfChairs = 20 });

            }

            if (!_Context.Categories.Any())
            {
                _Context.Categories.Add(new Category { Name = "Action"});
                _Context.Categories.Add(new Category { Name = "Drama"});
            }

            if (!_Context.MovieDisplayStates.Any())
            {
                _Context.MovieDisplayStates.Add(new MovieDisplayState { Status = "Expired" });
                _Context.MovieDisplayStates.Add(new MovieDisplayState { Status = "Comming soon" });
                _Context.MovieDisplayStates.Add(new MovieDisplayState { Status = "Available" });
            }

            if (!_Context.Actors.Any())
            {
                _Context.Actors.Add(new Actor { FirstName = "Jhin", LastName = "Talon", Bio = "Bio", News = "News", });
            }

            if (!_Context.Movies.Any())
            {
                _Context.Movies.Add(new Movie { Name = "Movie 1" , CategoryId = 1 , CinemaId = 1, MovieDisplayStateId = 3  , Description = "Description" 
                ,Price = 100 , StartDate = DateTime.UtcNow , EndDate = DateTime.UtcNow.AddDays(10) });
            }

            if (!_Context.ActorMovies.Any())
            {
                _Context.ActorMovies.Add(new ActorMovie { ActorId = 1 , MovieId = 1});
            }
        }
    }
}
