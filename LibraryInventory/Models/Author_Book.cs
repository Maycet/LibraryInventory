using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryInventory.Models
{
    [Table("autores_has_libros")]
    public class Author_Book
    {
        /// <summary>
        /// Identificador del autor
        /// </summary>
        [Column("autores_id")]
        [Display(Name = "Identificador autor")]
        public int AuthorId { get; set; }

        /// <summary>
        /// Autor asociado
        /// </summary>
        [Display(Name = "Autor")]
        public Author Author { get; set; } = new Author();

        /// <summary>
        /// Identificador del libro (código ISBN)
        /// </summary>
        [Column("libros_ISBN")]
        [Display(Name = "ISBN libro")]
        public int BookISBN { get; set; }

        /// <summary>
        /// Libro asociado
        /// </summary>
        [Display(Name = "Libro")]
        public Book Book { get; set; } = new Book();
    }
}