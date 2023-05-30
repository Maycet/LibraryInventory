using LibraryInventory.Data;
using LibraryInventory.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LibraryInventory.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _DbContext;
        public HomeController(ApplicationDbContext context) => _DbContext = context;

        public IActionResult Index()
        {
            HomeViewModel HomeViewModel = new HomeViewModel
            {
                Books = _DbContext.Book.ToList(),
                Authors = _DbContext.Author.ToList(),
                Publishers = _DbContext.Publisher.ToList()
            };
            return View(HomeViewModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}