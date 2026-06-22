using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using static RAWH.DAL.Enums.AppEnums;

namespace RAWH.BLL.DTOs
{
    public class PneumoniaSurveyCreateDto
    {
        public string? ChildName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }

        public FeverDuration FeverDuration { get; set; }
        public FeverLevel FeverLevel { get; set; }
        public FeverResponse FeverResponse { get; set; }

        public CoughTime CoughTime { get; set; }
        public CoughType CoughType { get; set; }
        public PhlegmStatus PhlegmStatus { get; set; }
        public CoughSeverity CoughSeverity { get; set; }

        public bool HasAbnormalBreathingSound { get; set; }
        public BreathingEffort BreathingEffort { get; set; }
        public FeedingAbility FeedingAbility { get; set; }
        public HasChestIndrawing HasChestIndrawing { get; set; }

        public bool HasNasalFlaring { get; set; }
        public bool HasCyanosis { get; set; }

        public bool FatigueStatus { get; set; }
        public AppetiteStatus AppetiteStatus { get; set; }

        public bool HasWeakCry { get; set; }
        public bool HasSevereRunnyNoseWithBreathingDifficulty { get; set; }

        public RecurrentChestIssues RecurrentChestIssues { get; set; }
        public HeartCondition HeartCondition { get; set; }
        
        

    }
}
