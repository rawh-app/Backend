using RAWH.DAL.Data;
using System.ComponentModel.DataAnnotations.Schema;
using static RAWH.DAL.Enums.AppEnums;
public class PneumoniaSurveyRequest
{
    // Child Info
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }

    [ForeignKey("ApplicationUser")]
    public string UserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public string ChildName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int Age { get; private set; }
    public AgeUnit AgeUnit { get; private set; }

    public void CalculateAge()
    {
        var today = DateTime.Today;
        var totalDays = (today - DateOfBirth).Days;

        if (totalDays < 28)
        {
            Age = totalDays;
            AgeUnit = AgeUnit.Days;
            return;
        }

        if (totalDays < 730)
        {
            Age = (int)(totalDays / 30.44);
            AgeUnit = AgeUnit.Months;
            return;
        }

        Age = (int)(totalDays / 365.25);
        AgeUnit = AgeUnit.Years;
    }






    public Gender Gender { get; set; }
    // Fever
    public FeverDuration FeverDuration { get; set; }
    public FeverLevel FeverLevel { get; set; }
    public FeverResponse FeverResponse { get; set; } // تعديل: عشان يشمل (بسهولة، بصعوبة، لم ينخفض)

    // Cough & Phlegm
    public CoughTime CoughTime { get; set; } // نهاراً، ليلاً، الخ..
    public CoughType CoughType { get; set; }
    public PhlegmStatus PhlegmStatus { get; set; } // إضافة: (سهل الخروج، كثيف وصعب، لا يوجد)
    public CoughSeverity CoughSeverity { get; set; }

    // Breathing & Physical Signs
    public bool HasAbnormalBreathingSound { get; set; }
    public BreathingEffort BreathingEffort { get; set; } // (طبيعي، سريع، ينهج)
    public FeedingAbility FeedingAbility { get; set; } // إضافة: مجهود الرضاعة
    public HasChestIndrawing HasChestIndrawing { get; set; } // انخماص الصدر
    public bool HasNasalFlaring { get; set; } // إضافة: اتساع فتحات الأنف
    public bool HasCyanosis { get; set; } // تغير لون الشفاه/اللسان

    // General Status
    public bool FatigueStatus { get; set; } // مرهق/نعسان، طبيعي
    public AppetiteStatus AppetiteStatus { get; set; } // يرفض تماماً، يأكل أقل، طبيعي
    public bool HasWeakCry { get; set; }
    public bool HasSevereRunnyNoseWithBreathingDifficulty { get; set; } // إضافة: الرشح الشديد

    // Medical History
    public RecurrentChestIssues RecurrentChestIssues { get; set; } // إضافة: سعال أو مشاكل صدرية متكررة
    public HeartCondition HeartCondition { get; set; } // إضافة: مشكلة في القلب (نعم، لا، لا أعلم)

    //Audio
    public string? AudioRecordPath { get; set; }


    //Survey Result From AI Model
    public string? RiskPrediction { get; set; } // Low Risk, Moderate Risk, High Risk, Severe Pneumonia
    public string? AudioRiskPrediction { get; set; } // Normal , Pneumonia

    public string? FinalDiagnosis { get; set; }
}