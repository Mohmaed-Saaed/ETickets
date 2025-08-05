using ETickets.Data;
using ETickets.Models;
using ETickets.Repositry;
using ETickets.Repositry.IRepositry;
using ETickets.Servies;
using ETickets.Servies.IServies;
using ETickets.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using static Microsoft.IO.RecyclableMemoryStreamManager;

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
            builder.Services.AddScoped<IActorMovieRepository, AcortMovieRepository>();
            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<IMovieDayRepository, MovieDayRepository>();
            builder.Services.AddScoped<IChairRepository, ChairRepository>();
            builder.Services.AddScoped<IDayRepository, DayRepository>();

            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddScoped<IApplicationUserOTPRepository, ApplicationUserOTPRepository>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IMovieAdminSaveService, MovieAdminSaveService>();
            builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
            builder.Services.AddScoped<IReservationDetailRepository, ReservationDetailRepository>();


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/Identity/Account/Login"; // Default path if user is not authenticated
                options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // When user lacks permission
                options.SlidingExpiration = true; // if he is active 60 minues are refreshing
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // The user is AuAuthenticated for 60 minutes
            });


            //To add sessions.
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                //options.Cookie.Expiration = TimeSpan.FromMinutes(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe")); // StripeSettings is the dependency of stripe section in appSettings.

            Stripe.StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"]; // Stripe.StripeConfiguration.ApiKey Global StripeConfiguration must have the secret key.




            #region AddLocalization
            var defualtCulture = "ar";

            var supportedCultures = new[]{
               new CultureInfo(defualtCulture),
               new CultureInfo("ar")
            };

            builder.Services.AddLocalization(option => option.ResourcesPath = "Resources");

            builder.Services.Configure<RequestLocalizationOptions>(config =>
            {
                config.DefaultRequestCulture = new RequestCulture(defualtCulture);
                config.SupportedCultures = supportedCultures;
                config.SupportedUICultures = supportedCultures;
                config.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider()); // Add culture from query

            });

            #endregion

            var app = builder.Build();

            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            using (var scope = app.Services.CreateScope())
            {
                
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                // use DbInitializer
                dbInitializer.Init();
            }

            app.Run();
        }
    }
}
