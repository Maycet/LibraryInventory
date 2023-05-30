using LibraryInventory.Data;
using LibraryInventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IActionResult Create()
        {
            ViewBag.AvailableBooks = new MultiSelectList(_DbContext.Book, "ISBN", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Office")] Publisher publisher, int[] selectedBooks)
        {
            if (ModelState.IsValid)
            {
                _DbContext.Add(publisher);
                await _DbContext.SaveChangesAsync();

                if (selectedBooks != null && selectedBooks.Length > 0 && _DbContext.Book != null)
                    foreach (int selectedItem in selectedBooks)
                    {
                        Book? SelectedBook = await _DbContext.Book.FirstOrDefaultAsync(book => selectedItem == book.ISBN);
                        if (SelectedBook == null) continue;

                        SelectedBook.Publisher = publisher;
                        SelectedBook.PublisherId = publisher.Id;
                        _DbContext.Update(SelectedBook);

                        await _DbContext.SaveChangesAsync();
                    }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.AvailableBooks = new MultiSelectList(_DbContext.Book,
                                                         "ISBN",
                                                         "Title",
                                                         publisher.Books);
            return View(publisher);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _DbContext.Publisher == null) return NotFound();

            Publisher? Publisher =
                await _DbContext.Publisher
                      .Include(publisher => publisher.Books)
                      .FirstOrDefaultAsync(publisher => publisher.Id == id);
            if (Publisher == null) return NotFound();

            ViewBag.AvailableBooks = new MultiSelectList(_DbContext.Book,
                                                         "ISBN",
                                                         "Title",
                                                         Publisher.Books);
            return View(Publisher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Office")] Publisher publisher, int[] selectedBooks)
        {
            if (id != publisher.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _DbContext.Update(publisher);
                    await _DbContext.SaveChangesAsync();

                    if (selectedBooks != null && selectedBooks.Length > 0 && _DbContext.Book != null)
                        foreach (int selectedItem in selectedBooks)
                        {
                            Book? SelectedBook = await _DbContext.Book.FirstOrDefaultAsync(book => selectedItem == book.ISBN);
                            if (SelectedBook == null) continue;

                            SelectedBook.Publisher = publisher;
                            SelectedBook.PublisherId = publisher.Id;
                            _DbContext.Update(SelectedBook);

                            await _DbContext.SaveChangesAsync();
                        }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublisherExists(publisher.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AvailableBooks = new MultiSelectList(_DbContext.Book,
                                                         "ISBN",
                                                         "Title",
                                                         publisher.Books);
            return View(publisher);
        }

        public async Task<IActionResult> Delete(int? id) => await Details(id);

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_DbContext.Publisher == null)
                return Problem("Entity set 'ApplicationDbContext.Publisher'  is null.");

            Publisher? Publisher = await _DbContext.Publisher.FindAsync(id);
            if (Publisher != null) _DbContext.Publisher.Remove(Publisher);

            await _DbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublisherExists(int id)
            => (_DbContext.Publisher?.Any(publisher => publisher.Id == id)).GetValueOrDefault();
    }
}