using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Administration.Models.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "L'email est requis")]
        [StringLength(200, ErrorMessage = "L'email ne peut pas dépasser 200 caractères")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [StringLength(255, ErrorMessage = "Le mot de passe ne peut pas dépasser 255 caractères")]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est requis")]
        [StringLength(100, ErrorMessage = "Le prénom ne peut pas dépasser 100 caractères")]
        [Display(Name = "Prénom")]
        [Column("Prenom")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom est requis")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
        [Display(Name = "Nom")]
        [Column("Nom")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Administrateur")]
        public bool IsAdmin { get; set; } = false;

        [StringLength(20, ErrorMessage = "Le téléphone ne peut pas dépasser 20 caractères")]
        [Phone(ErrorMessage = "Le numéro de téléphone n'est pas valide")]
        [Display(Name = "Téléphone")]
        [NotMapped]
        public string? Phone { get; set; }

        [Display(Name = "Événements")]
        public virtual ICollection<Event> Events { get; set; } = new HashSet<Event>();

        [NotMapped]
        [Display(Name = "Nom complet")]
        public string FullName => $"{FirstName} {LastName}";
    }
}