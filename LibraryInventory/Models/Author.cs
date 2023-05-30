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
        [MaxLength(45)]
        [Column("nombre")]
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Nombre es obligatorio.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Apellido(s)
        /// </summary>
        [MaxLength(45)]
        [Column("apellidos")]
        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "Apellido es obligatorio.")]
        public string LastName { get; set; }

        /// <summary>
        /// Libros relacionados
        /// </summary>
        [Display(Name = "Libros relacionados")]
        public List<Author_Book>? Books { get; set; } = new List<Author_Book>();
    }
}