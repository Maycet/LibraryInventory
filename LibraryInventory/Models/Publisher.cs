using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryInventory.Models
{
    [Table("editoriales")]
    public class Publisher
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [Key]
        [Column("id")]
        [Display(Name = "Identificador")]
        public int Id { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [Required]
        [MaxLength(45)]
        [Column("nombre")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        /// <summary>
        /// Sede
        /// </summary>
        [MaxLength(45)]
        [Column("sede")]
        [Display(Name = "Sede")]
        public string? Office { get; set; }

        /// <summary>
        /// Libros relacionados
        /// </summary>
        [Display(Name = "Libros relacionados")]
        public List<Book>? Books { get; set; } = new List<Book>();
    }
}