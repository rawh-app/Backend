using static RAWH.DAL.Enums.AppEnums;
public class PneumoniaSurveyRequest
{
    // Child Info
    public string ChildName { get; set; }
    public DateTime DateOfBirth { get; set; }
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
}