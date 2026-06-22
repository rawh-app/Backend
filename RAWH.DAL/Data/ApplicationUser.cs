using EntityFrameworkCore.EncryptColumn.Attribute;
using Microsoft.AspNetCore.Identity;
using RAWH.DAL.Models;

namespace RAWH.DAL.Data
{
    public class ApplicationUser : IdentityUser
    {
        [EncryptColumn]
        public string? resetPasswordEmail { get; set; }

        public string? GoogleId { get; set; }

        public PneumoniaSurveyRequest? PneumoniaSurveyRequest { get; set; }

        public int? HospitalId { get; set; }
        public Hospital? Hospital { get; set; }

        public int? ClinicId { get; set; }
        public Clinic? Clinic { get; set; }
        public int? MotherId { get; set; }
        public Appointment? MotherAppointment { get; set; }
        public ICollection<Appointment>? DoctorAppointments { get; set; }
        public ICollection<MedicalReport>? DoctorMedicalReports { get; set; }

        public ICollection<MedicalReport>? ChildMedicalReports { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}