using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryInventory.Models
{
    [Table("autores")]
    public class Author
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [Required]
        [MaxLength(45)]
        [Column("nombre")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        /// <summary>
        /// Apellido(s)
        /// </summary>
        [Required]
        [MaxLength(45)]
        [Column("apellidos")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        /// <summary>
        /// Libros relacionados
        /// </summary>
        [Display(Name = "Libros relacionados")]
        public List<Author_Book>? Books { get; set; } = new List<Author_Book>();
    }
}