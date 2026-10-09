using System.ComponentModel.DataAnnotations;

namespace HospitalSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Patient Name is required")]
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Time is required")]
        [Display(Name = "Appointment Time")]
        public string AppointmentTime { get; set; } = string.Empty;

          
        [Required]
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
    }
}