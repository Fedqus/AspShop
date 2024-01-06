using kurs.Models;
using kurs.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace kurs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                .UseLazyLoadingProxies());
            builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
            {
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
                .AddRoles<IdentityRole<int>>()
                .AddEntityFrameworkStores<DatabaseContext>();
            builder.Services.AddAuthorization();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<FileManager>();
            builder.Services.AddEntityManagers();
            builder.Services.AddScoped<CartManager>();
            builder.Services.AddScoped<OrderManager>();
            builder.Services.AddScoped<DatabaseBackupManager>();
            var app = builder.Build();

            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}