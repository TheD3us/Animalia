using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Administration.Models.Entities
{
    [Table("Trainings")]
    public class Training
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre de l'entrainement est requis")]
        [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
        [Display(Name = "Titre de l'entrainement")]
        [Column("Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La durée est requise")]
        [Display(Name = "Durée (en minutes)")]
        [Column("DurationMinutes")]
        public int DurationMinutes { get; set; }

        [Display(Name = "Équipement requis")]
        [Column("Equipment")]
        public string? Equipment { get; set; }

        [Required(ErrorMessage = "Le niveau est requis")]
        [StringLength(50, ErrorMessage = "Le niveau ne peut pas dépasser 50 caractères")]
        [Display(Name = "Niveau")]
        [Column("Level")]
        public string Level { get; set; } = string.Empty;

        [Required(ErrorMessage = "La description est requise")]
        [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères")]
        [Display(Name = "Description")]
        [Column("Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column("UserId")]
        [Display(Name = "Créateur")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [NotMapped]
        public string Name 
        { 
            get => Title; 
            set => Title = value; 
        }

        [NotMapped]
        public int Duration 
        { 
            get => DurationMinutes; 
            set => DurationMinutes = value; 
        }

        [NotMapped]
        public string? EquipmentRequired 
        { 
            get => Equipment; 
            set => Equipment = value; 
        }

        [NotMapped]
        public string DifficultyLevel 
        { 
            get => Level; 
            set => Level = value; 
        }

        [NotMapped]
        public string TrainingType { get; set; } = string.Empty;

        [NotMapped]
        public bool IsActive { get; set; } = true;

        [NotMapped]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [NotMapped]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [NotMapped]
        public int? CreatedByUserId { get; set; }
        
        [NotMapped]
        public virtual User? CreatedByUser { get; set; }
    }
}