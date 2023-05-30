using LibraryInventory.Data;
using LibraryInventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibraryInventory.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _DbContext;

        public AuthorsController(ApplicationDbContext context) => _DbContext = context;

        public async Task<IActionResult> Index()
            => _DbContext.Author != null ?
                    View(await _DbContext.Author.ToListAsync()) :
                    Problem("Entity set 'ApplicationDbContext.Author' is null.");

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _DbContext.Author == null) return NotFound();

            Author? Author =
                await _DbContext.Author
                      .Include(auhor => auhor.Books)
                      .ThenInclude(authorBook => authorBook.Book)
                      .FirstOrDefaultAsync(author => author.Id == id);

            if (Author == null) return NotFound();

            return View(Author);
        }

        public IActionResult Create()
        {
            ViewBag.AvailableBooks = new MultiSelectList(_DbContext.Book, "ISBN", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName")] Author author, int[] selectedBooks)
        {
            if (ModelState.IsValid)
            {
                _DbContext.Add(author);
                await _DbContext.SaveChangesAsync();

                if (selectedBooks != null && selectedBooks.Length > 0 && _DbContext.Book != null)
                    foreach (int selectedItem in selectedBooks)
                    {
                        Book? SelectedBook = await _DbContext.Book.FirstOrDefaultAsync(book => selectedItem == book.ISBN);
                        if (SelectedBook == null) continue;
                        _DbContext.Add(
                            new Author_Book()
                            {
                                Author = author,
                                AuthorId = author.Id,
                                Book = SelectedBook,
                                BookISBN = selectedItem
                            });

                        await _DbContext.SaveChangesAsync();
                    }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.AvailableBooks = new MultiSelectList(_DbContext.Book,
                                                         "ISBN",
                                                         "Title",
                                                         author.Books?.Select(authorBook => authorBook.Book));
            return View(author);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _DbContext.Author == null) return NotFound();

            Author? Author =
                await _DbContext.Author
                      .FirstOrDefaultAsync(author => author.Id == id);
            if (Author == null) return NotFound();

            ViewBag.AvailableBooks = new MultiSelectList(_DbContext.Book,
                                                         "ISBN",
                                                         "Title",
                                                         Author.Books?.Select(authorBook => authorBook.Book));
            return View(Author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName")] Author author, int[] selectedBooks)
        {
            if (id != author.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _DbContext.Update(author);
                    await _DbContext.SaveChangesAsync();

                    await UpdateBooksAssociations(author, selectedBooks);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AvailableBooks = new MultiSelectList(_DbContext.Book,
                                                         "ISBN",
                                                         "Title",
                                                         author.Books?.Select(authorBook => authorBook.Book));
            return View(author);
        }

        private async Task UpdateBooksAssociations(Author author, int[] selectedBooks)
        {
            if (_DbContext.Author_Book == null) return;

            Author_Book[] CurrentAssociations =
                _DbContext.Author_Book.Where(authorBook => authorBook.AuthorId == author.Id).ToArray();

            int[] CurrentAssociationsISBN = CurrentAssociations.Select(authorBook => authorBook.BookISBN).ToArray();
            int[] AssociationsToDeleteISBN = CurrentAssociationsISBN.Except(selectedBooks).ToArray();

            foreach (int isbnToDelete in AssociationsToDeleteISBN)
            {
                Author_Book? AuthorBookToDelete =
                    CurrentAssociations.FirstOrDefault(authorBook => authorBook.BookISBN == isbnToDelete);

                if (AuthorBookToDelete != null)
                {
                    _DbContext.Remove(AuthorBookToDelete);
                    await _DbContext.SaveChangesAsync();
                }
            }

            if (_DbContext.Book == null) return;

            int[] AssociationsToAddISBN = selectedBooks.Except(CurrentAssociationsISBN).ToArray();
            Book[] Books = _DbContext.Book.ToArray();

            foreach (int isbnToAdd in AssociationsToAddISBN)
            {
                Book? SelectedBook = Books.FirstOrDefault(book => book.ISBN == isbnToAdd);
                if (SelectedBook == null) continue;

                _DbContext.Add(new Author_Book()
                {
                    Author = author,
                    AuthorId = author.Id,
                    Book = SelectedBook,
                    BookISBN = isbnToAdd
                });
                await _DbContext.SaveChangesAsync();
            }
        }

        public async Task<IActionResult> Delete(int? id) => await Details(id);

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_DbContext.Author == null)
                return Problem("Entity set 'ApplicationDbContext.Author' is null.");

            Author? Author = await _DbContext.Author.FindAsync(id);
            if (Author != null) _DbContext.Author.Remove(Author);

            await _DbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
            => (_DbContext.Author?.Any(author => author.Id == id)).GetValueOrDefault();
    }
}