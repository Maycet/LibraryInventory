using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryInventory.Models
{
    [Table("libros")]
    public class Book
    {
        /// <summary>
        /// Código ISBN (identificador del libro)
        /// </summary>
        [Key]
        [Column("ISBN")]
        [Display(Name = "ISBN")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ISBN { get; set; }

        /// <summary>
        /// Identificador de la editorial
        /// </summary>
        [Required]
        [Display(Name = "Identificador editorial")]
        [Column("editoriales_id")]
        public int PublisherId { get; set; }

        /// <summary>
        /// Editorial
        /// </summary>
        [Required]
        [Display(Name = "Editorial")]
        public Publisher Publisher { get; set; }

        /// <summary>
        /// Título
        /// </summary>
        [MaxLength(45)]
        [Required]
        [Display(Name = "Título")]
        [Column("titulo")]
        public string Title { get; set; }

        /// <summary>
        /// Sinopsis
        /// </summary>
        [Column("sinopsis", TypeName = "ntext")]
        [Display(Name = "Sinopsis")]
        public string? Synopsis { get; set; }

        /// <summary>
        /// Número de páginas
        /// </summary>
        [MaxLength(45)]
        [Column("n_paginas")]
        [Display(Name = "No. Páginas")]
        public string? NumberOfPages { get; set; }

        /// <summary>
        /// Autores relacionados
        /// </summary>
        [Display(Name = "Autores")]
        public List<Author_Book>? Authors { get; set; } = new List<Author_Book>();
    }
}