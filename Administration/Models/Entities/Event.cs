using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Administration.Models.Entities
{
    [Table("Events")]
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "L'ID utilisateur est requis")]
        [Display(Name = "ID Utilisateur")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Le titre est requis")]
        [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
        [Display(Name = "Titre")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date est requise")]
        [Display(Name = "Date et heure")]
        public DateTime DateTime { get; set; }

        [Required(ErrorMessage = "Le lieu est requis")]
        [StringLength(300, ErrorMessage = "Le lieu ne peut pas dépasser 300 caractères")]
        [Display(Name = "Lieu")]
        public string Location { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Les notes ne peuvent pas dépasser 1000 caractères")]
        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Le nombre de participants doit être supérieur à 0")]
        [Display(Name = "Nombre maximum de participants")]
        public int? MaxParticipants { get; set; }

        [Display(Name = "Utilisateur")]
        public virtual User? User { get; set; }
    }
}