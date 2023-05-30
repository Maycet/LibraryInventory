using LibraryInventory.Data;
using LibraryInventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibraryInventory.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _DbContext;

        public BooksController(ApplicationDbContext context) => _DbContext = context;

        public async Task<IActionResult> Index()
            => _DbContext.Book != null ?
                    View(await _DbContext.Book.Include(book => book.Publisher).ToListAsync()) :
                    Problem("Entity set 'ApplicationDbContext.Book' is null.");

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _DbContext.Book == null) return NotFound();

            Book? Book =
                await _DbContext.Book
                      .Include(book => book.Publisher)
                      .Include(book => book.Authors)
                      .ThenInclude(authorBook => authorBook.Author)
                      .FirstOrDefaultAsync(book => book.ISBN == id);
            
            if (Book == null) return NotFound();

            return View(Book);
        }

        public IActionResult Create()
        {
            ViewBag.AvailablePublishers = new SelectList(_DbContext.Publisher, "Id", "Name");
            ViewBag.AvailableAuthors = new MultiSelectList(_DbContext.Author, "Id", "LastName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ISBN,PublisherId,Title,Synopsis,NumberOfPages")] Book book,
                                                int[] selectedAuthors)
        {
            if (_DbContext.Publisher == null)
                return Problem("Entity set 'ApplicationDbContext.Publisher' is null.");

            Publisher? Publisher =
                await _DbContext.Publisher
                      .FirstOrDefaultAsync(publisher => publisher.Id == book.PublisherId);

            if (Publisher != null)
            {
                book.Publisher = Publisher;
                ModelState.Remove(nameof(book.Publisher));
            }

            if (ModelState.IsValid)
            {
                _DbContext.Add(book);
                await _DbContext.SaveChangesAsync();

                if (selectedAuthors != null && selectedAuthors.Length > 0 && _DbContext.Author != null)
                    foreach (int selectedItem in selectedAuthors)
                    {
                        Author? SelectedAuthor =
                            await _DbContext.Author.FirstOrDefaultAsync(author => selectedItem == author.Id);

                        if (SelectedAuthor == null) continue;
                        _DbContext.Add(
                            new Author_Book()
                            {
                                Author = SelectedAuthor,
                                AuthorId = selectedItem,
                                Book = book,
                                BookISBN = book.ISBN
                            });

                        await _DbContext.SaveChangesAsync();
                    }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.AvailablePublishers = new SelectList(_DbContext.Publisher, "Id", "Name", book.PublisherId);
            ViewBag.AvailableAuthors = new MultiSelectList(_DbContext.Author,
                                                           "Id",
                                                           "LastName",
                                                           book.Authors?.Select(authorBook => authorBook.Author));
            return View(book);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _DbContext.Book == null) return NotFound();

            Book? Book = await _DbContext.Book.Include(book => book.Publisher).FirstOrDefaultAsync(book => book.ISBN == id);
            if (Book == null) return NotFound();

            ViewBag.AvailablePublishers = new SelectList(_DbContext.Publisher, "Id", "Name", Book.PublisherId);
            ViewBag.AvailableAuthors = new MultiSelectList(_DbContext.Author,
                                                           "Id",
                                                           "LastName",
                                                           Book.Authors?.Select(authorBook => authorBook.Author));
            return View(Book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ISBN,PublisherId,Title,Synopsis,NumberOfPages")] Book book,
                                              int[] selectedAuthors)
        {
            Publisher? Publisher =
                await _DbContext.Publisher
                      .FirstOrDefaultAsync(publisher => publisher.Id == book.PublisherId);

            if (Publisher != null)
            {
                book.Publisher = Publisher;
                ModelState.Remove(nameof(book.Publisher));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _DbContext.Update(book);
                    await _DbContext.SaveChangesAsync();

                    await UpdateAuthorsAssociations(book, selectedAuthors);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.ISBN)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AvailablePublishers = new SelectList(_DbContext.Publisher, "Id", "Name", book.PublisherId);
            return View(book);
        }

        private async Task UpdateAuthorsAssociations(Book book, int[] selectedAuthors)
        {
            if (_DbContext.Author_Book == null) return;

            Author_Book[] CurrentAssociations =
                _DbContext.Author_Book.Where(authorBook => authorBook.BookISBN == book.ISBN).ToArray();

            int[] CurrentAssociationsIds = CurrentAssociations.Select(authorBook => authorBook.AuthorId).ToArray();
            int[] AssociationsToDeleteIds = CurrentAssociationsIds.Except(selectedAuthors).ToArray();

            foreach (int idToDelete in AssociationsToDeleteIds)
            {
                Author_Book? AuthorBookToDelete =
                    CurrentAssociations.FirstOrDefault(authorBook => authorBook.AuthorId == idToDelete);

                if (AuthorBookToDelete != null)
                {
                    _DbContext.Remove(AuthorBookToDelete);
                    await _DbContext.SaveChangesAsync();
                }
            }

            if (_DbContext.Author == null) return;

            int[] AssociationsToAddId = selectedAuthors.Except(CurrentAssociationsIds).ToArray();
            Author[] Authors = _DbContext.Author.ToArray();

            foreach (int idToAdd in AssociationsToAddId)
            {
                Author? SelectedAuthor = Authors.FirstOrDefault(book => book.Id == idToAdd);
                if (SelectedAuthor == null) continue;

                _DbContext.Add(new Author_Book()
                {
                    Author = SelectedAuthor,
                    AuthorId = idToAdd,
                    Book = book,
                    BookISBN = book.ISBN
                });
                await _DbContext.SaveChangesAsync();
            }
        }

        public async Task<IActionResult> Delete(int? id) => await Details(id);

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_DbContext.Book == null)
                return Problem("Entity set 'ApplicationDbContext.Book' is null.");

            Book? Book = await _DbContext.Book.FirstOrDefaultAsync(book => book.ISBN == id);
            if (Book != null) _DbContext.Book.Remove(Book);

            await _DbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
            => (_DbContext.Book?.Any(e => e.ISBN == id)).GetValueOrDefault();
    }
}