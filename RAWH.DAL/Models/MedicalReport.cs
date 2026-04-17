using RAWH.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAWH.DAL.Models
{
    public class MedicalReport
    {
        public int Id { get; set; }

        public string? ChildId { get; set; }
        public ApplicationUser? Child { get; set; }

        public string? DoctorId { get; set; }
        public ApplicationUser? Doctor { get; set; }

        public string? Diagnosis { get; set; }

        public string? Notes { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
