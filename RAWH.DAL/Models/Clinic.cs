using RAWH.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAWH.DAL.Models
{
    public class Clinic
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public ICollection<ApplicationUser>? Doctors { get; set; }
    }
}
