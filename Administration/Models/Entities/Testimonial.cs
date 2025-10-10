using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Administration.Models.Entities
{
    [Table("Testimonials")]
    public class Testimonial
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
        [Display(Name = "Nom")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le contenu est requis")]
        [StringLength(1000, ErrorMessage = "Le contenu ne peut pas dépasser 1000 caractères")]
        [Display(Name = "Contenu")]
        public string Content { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "La note doit être entre 1 et 5")]
        [Display(Name = "Note")]
        public int? Rating { get; set; }

        [Display(Name = "Date de création")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [StringLength(500, ErrorMessage = "L'URL de l'image ne peut pas dépasser 500 caractères")]
        [Display(Name = "URL de l'image")]
        [Url(ErrorMessage = "L'URL de l'image n'est pas valide")]
        public string? ImageUrl { get; set; }
    }
}