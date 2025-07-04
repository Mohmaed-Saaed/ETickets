using ETickets.Data;
using ETickets.Repositry;
using ETickets.Repositry.IRepositry;
using ETickets.Servies;
using ETickets.Servies.IServies;
using Microsoft.EntityFrameworkCore;

namespace ETickets
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
            builder.Services.AddScoped<ICinemaRepository , CinemaRepository>();
            builder.Services.AddScoped<IMovieDisplayStateRepository , MovieDisplayStateRepository>();
            builder.Services.AddScoped<ICategoryRepository , CategoryRepository>();
            builder.Services.AddScoped<IActorRepository, ActosRepository>();
            builder.Services.AddScoped<IActorMovieRepository, AcotMovieRepository>();
            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddTransient<IMovieAdminSaveService, MovieAdminSaveService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
