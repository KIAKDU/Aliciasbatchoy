using AliciasWebDisplay.Services;
using Microsoft.EntityFrameworkCore;
using AliciasWebDisplay.Data;
using Microsoft.AspNetCore.Identity;
using AliciasWebDisplay.Areas.Identity.Data;

namespace AliciasWebDisplay
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add session services
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Register database contexts
            builder.Services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDbContext<LapazUserDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SecondDatabaseConnection")));

            builder.Services.AddDbContext<VillaUserDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ThirdDatabaseConnection")));

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDbContext<JaroCategoryDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDbContext<JaroSOHDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDbContext<LapazApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SecondDatabaseConnection")));

            builder.Services.AddDbContext<LapazCategoryDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SecondDatabaseConnection")));

            builder.Services.AddDbContext<LapazSOHDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SecondDatabaseConnection")));

            builder.Services.AddDbContext<VillaApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ThirdDatabaseConnection")));

            builder.Services.AddDbContext<VillaCategoryDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ThirdDatabaseConnection")));

            builder.Services.AddDbContext<VillaSOHDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ThirdDatabaseConnection")));

            // Add MVC and Razor Pages services
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // Enable session middleware
            builder.Services.AddSession();
            app.UseSession();

            // Map default controller route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{UserCode?}");

            app.MapControllerRoute(
                 name: "lapaz",
                 pattern: "Products/Lapaz",
                 defaults: new { controller = "Products", action = "LapazIndex" });

            app.MapControllerRoute(
                 name: "villa",
                 pattern: "Products/Villa",
                 defaults: new { controller = "Products", action = "VillaIndex" });

            app.MapControllerRoute(
                  name: "category",
                  pattern: "Category/Index",
                  defaults: new { controller = "Category", action = "Index" });

            app.MapControllerRoute(
                  name: "stocks",
                  pattern: "Stocks/Index",
                  defaults: new { controller = "Stock", action = "Index" });          

            // Map Razor Pages
            app.MapRazorPages();

            // Run the app
            app.Run();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache(); // Use memory cache for session
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
            });
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession(); // Ensure session is enabled here
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }


    }
}
