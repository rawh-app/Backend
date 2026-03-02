using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAWH.BLL.DTOs.AI_DTOs
{
    public class PneumoniaAiResponseDto
    {
        public bool IsPneumoniaSuspected { get; set; }
        public double Confidence { get; set; }
        public string Severity { get; set; }
        public string Recommendation { get; set; }
    }
}
