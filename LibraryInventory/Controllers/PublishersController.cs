using LibraryInventory.Data;
using LibraryInventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryInventory.Controllers
{
    public class PublishersController : Controller
    {
        private readonly ApplicationDbContext _DbContext;
        public PublishersController(ApplicationDbContext context) => _DbContext = context;

        public async Task<IActionResult> Index()
            => _DbContext.Publisher != null ?
                    View(await _DbContext.Publisher.ToListAsync()) :
                    Problem("Entity set 'ApplicationDbContext.Publisher'  is null.");

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _DbContext.Publisher == null) return NotFound();

            Publisher? Publisher =
                await _DbContext.Publisher
                      .Include(publisher => publisher.Books)
                      .FirstOrDefaultAsync(publisher => publisher.Id == id);

            if (Publisher == null) return NotFound();

            return View(Publisher);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Office")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                _DbContext.Add(publisher);
                await _DbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _DbContext.Publisher == null) return NotFound();

            Publisher? Publisher =
                await _DbContext.Publisher
                      .FirstOrDefaultAsync(publisher => publisher.Id == id);
            if (Publisher == null) return NotFound();

            return View(Publisher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Office")] Publisher publisher)
        {
            if (id != publisher.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _DbContext.Update(publisher);
                    await _DbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublisherExists(publisher.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(publisher);
        }

        public async Task<IActionResult> Delete(int? id) => await Details(id);

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_DbContext.Publisher == null)
                return Problem("Entity set 'ApplicationDbContext.Publisher'  is null.");

            Publisher? Publisher =
                await _DbContext.Publisher
                      .Include(publisher => publisher.Books)
                      .FirstOrDefaultAsync(publisher => publisher.Id == id);

            if (Publisher != null)
            {
                if (Publisher.Books.Any())
                {
                    ModelState.AddModelError("BooksAssociationExists",
                                             "No es posible eliminar una editorial con libros asociados.");
                    return View(Publisher);
                }

                _DbContext.Publisher.Remove(Publisher);
            }

            await _DbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublisherExists(int id)
            => (_DbContext.Publisher?.Any(publisher => publisher.Id == id)).GetValueOrDefault();
    }
}