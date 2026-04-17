using RAWH.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAWH.DAL.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public DateTime? AppointmentDate { get; set; }

        public string? DoctorId { get; set; }
        public ApplicationUser? Doctor { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public string? Status { get; set; }
    }
}
