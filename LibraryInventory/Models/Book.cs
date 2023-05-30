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
        [Required(ErrorMessage = "ISBN es obligatorio.")]
        public int ISBN { get; set; }

        /// <summary>
        /// Identificador de la editorial
        /// </summary>
        [Column("editoriales_id")]
        [Display(Name = "Identificador editorial")]
        [Required(ErrorMessage = "Debe seleccionar una editorial.")]
        public int PublisherId { get; set; }

        /// <summary>
        /// Editorial
        /// </summary>
        [Display(Name = "Editorial")]
        [Required(ErrorMessage = "Debe seleccionar una editorial.")]
        public Publisher Publisher { get; set; }

        /// <summary>
        /// Título
        /// </summary>
        [MaxLength(45)]
        [Display(Name = "Título")]
        [Column("titulo")]
        [Required(ErrorMessage = "Título es obligatorio.")]
        public string Title { get; set; }

        /// <summary>
        /// Sinopsis
        /// </summary>
        [Column("sinopsis", TypeName = "ntext")]
        [Display(Name = "Sinopsis")]
        public string? Synopsis { get; set; }

        public string? ShortSynopsis
            => Synopsis != null ? Synopsis.Length < 70 ?
               Synopsis : $"{Synopsis[..66]}..." : null;

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