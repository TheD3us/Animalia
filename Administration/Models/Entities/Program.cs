using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Administration.Models.Entities
{
    [Table("ProgramModels")]
    public class ProgramEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est requis")]
        [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
        [Display(Name = "Titre")]
        [Column("Title")] 
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Le résumé ne peut pas dépasser 1000 caractères")]
        [Display(Name = "Résumé")]
        [Column("Summary")]
        public string? Summary { get; set; }

        [StringLength(50, ErrorMessage = "La difficulté ne peut pas dépasser 50 caractères")]
        [Display(Name = "Difficulté")]
        [Column("Difficulty")]  
        public string? Difficulty { get; set; }

        [Required(ErrorMessage = "Le prix est requis")]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être positif")]
        [Column("Price", TypeName = "decimal(18,2)")]
        [Display(Name = "Prix")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "L'URL de l'image ne peut pas dépasser 500 caractères")]
        [Display(Name = "URL de l'image")]
        [Url(ErrorMessage = "L'URL de l'image n'est pas valide")]
        [Column("ImageUrl")]  
        public string? ImageUrl { get; set; }

        [Display(Name = "Entrainements")]
        public virtual ICollection<Training> Trainings { get; set; } = new HashSet<Training>();
    }
}