using LibraryInventory.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryInventory
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder ApplicationBuilder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

            string ConnectionString = ApplicationBuilder.Configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            ApplicationBuilder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(ConnectionString));

            // Add services to the container.
            ApplicationBuilder.Services.AddControllersWithViews();

            WebApplication WebApplication = ApplicationBuilder.Build();

            // Configure the HTTP request pipeline.
            if (!WebApplication.Environment.IsDevelopment())
            {
                WebApplication.UseExceptionHandler("/Home/Error");
                WebApplication.UseHsts();
            }

            WebApplication.UseHttpsRedirection();
            WebApplication.UseStaticFiles();

            WebApplication.UseRouting();

            WebApplication.UseAuthorization();

            WebApplication.MapControllerRoute(name: "default", pattern: "{controller=Books}/{action=Index}");

            WebApplication.Run();
        }
    }
}