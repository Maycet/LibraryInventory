using LibraryInventory.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LibraryInventory.Controllers
{
    public class HomeController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}