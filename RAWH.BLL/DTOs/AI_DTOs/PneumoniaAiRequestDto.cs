using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAWH.BLL.DTOs.AI_DTOs
{
    public class PneumoniaAiRequestDto
    {
        public int Age { get; set; }
        public string Gender { get; set; }

        public string FeverDuration { get; set; }
        public string FeverLevel { get; set; }
        public string FeverResponse { get; set; }

        public string CoughTime { get; set; }
        public string CoughType { get; set; }
        public string PhlegmStatus { get; set; }
        public string CoughSeverity { get; set; }

        public bool HasAbnormalBreathingSound { get; set; }
        public string BreathingEffort { get; set; }
        public string FeedingAbility { get; set; }
        public string HasChestIndrawing { get; set; }

        public bool HasNasalFlaring { get; set; }
        public bool HasCyanosis { get; set; }

        public bool FatigueStatus { get; set; }
        public string AppetiteStatus { get; set; }

        public bool HasWeakCry { get; set; }
        public bool HasSevereRunnyNoseWithBreathingDifficulty { get; set; }

        public string RecurrentChestIssues { get; set; }
        public string HeartCondition { get; set; }

        public string? AudioPath { get; set; }
    }
}
