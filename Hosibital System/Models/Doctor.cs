using System.ComponentModel.DataAnnotations;

namespace HospitalSystem.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Doctor Name is required")]
        [Display(Name = "Doctor Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialization is required")]
        public string Specialization { get; set; } = string.Empty;

        [Display(Name = "Doctor Image URL")]
        public string ImageUrl { get; set; } = string.Empty;

        
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}