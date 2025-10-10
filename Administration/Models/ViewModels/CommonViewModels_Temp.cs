using System.ComponentModel.DataAnnotations;

namespace Administration.Models.ViewModels
{
    // ViewModel pour les entrainements avec association aux programmes
    public class TrainingViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre de l'entrainement est requis")]
        [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
        [Display(Name = "Titre de l'entrainement")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La durée est requise")]
        [Range(1, int.MaxValue, ErrorMessage = "La durée doit être positive")]
        [Display(Name = "Durée (en minutes)")]
        public int DurationMinutes { get; set; }

        [Display(Name = "Équipement requis")]
        public string? Equipment { get; set; }

        [Required(ErrorMessage = "Le niveau est requis")]
        [StringLength(50, ErrorMessage = "Le niveau ne peut pas dépasser 50 caractères")]
        [Display(Name = "Niveau")]
        public string Level { get; set; } = string.Empty;

        [Required(ErrorMessage = "La description est requise")]
        [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Programmes associés")]
        public List<int>? SelectedProgramIds { get; set; } = new List<int>();
    }

    // ViewModel pour les programmes avec association aux entrainements  
    public class ProgramViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est requis")]
        [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
        [Display(Name = "Titre")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Le résumé ne peut pas dépasser 1000 caractères")]
        [Display(Name = "Résumé")]
        public string? Summary { get; set; }

        [StringLength(50, ErrorMessage = "La difficulté ne peut pas dépasser 50 caractères")]
        [Display(Name = "Difficulté")]
        public string? Difficulty { get; set; }

        [Required(ErrorMessage = "Le prix est requis")]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être positif")]
        [Display(Name = "Prix")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "L'URL de l'image ne peut pas dépasser 500 caractères")]
        [Display(Name = "URL de l'image")]
        [Url(ErrorMessage = "L'URL de l'image n'est pas valide")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Entrainements associés")]
        public List<int>? SelectedTrainingIds { get; set; } = new List<int>();
    }
}
