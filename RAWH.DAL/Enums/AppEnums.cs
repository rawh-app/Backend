namespace RAWH.DAL.Enums
{
    public class AppEnums
    {
        public enum Gender
        {
            Male = 0,
            Female = 1
        }
        public enum FeverDuration
        {
            LessThan24Hours,     // منذ أقل من 24 ساعة
            From1To3Days,        // من 1-3 أيام
            MoreThan3Days,       // منذ أكثر من 3 أيام
            NormalTemperature    // حرارة الطفل طبيعية
        }
        public enum FeverResponse
        {
            ReducedEasily,       // نعم، تنخفض بسهولة
            ReducedWithDifficulty, // تنخفض بصعوبة
            NotReduced,          // لا تنخفض
            DidNotTakeMedicine   // لم يأخذ الطفل خافض للحرارة
        }
        public enum CanCompleteMealWithoutStopping
        {

        }

        public enum FeverLevel
        {
            LessThan38,          // أقل من 38
            From38To39,          // من 38 إلى 39
            MoreThan39,          // أعلى من 39
            NotMeasured          // لم يتم قياس درجة الحرارة
        }
        public enum CoughTime
        {
            IncreasesDaytime,    // يزداد نهاراً
            IncreasesNighttime,  // يزداد ليلاً
            ContinuousAllDay,    // مستمر طوال اليوم
            NoCough              // لا يعاني الطفل من السعال
        }
        public enum AppetiteStatus
        {
            RefusesCompletely,   // يرفض الأكل والشرب تماماً
            EatsLessThanUsual,   // يتناول كميات أقل من المعتاد
            EatsNormally         // يتناول الطعام والشراب بشكل طبيعي
        }
        public enum CoughDuration
        {
            None = 0,
            StartedToday = 1,
            FewDays = 2,
            AllDay = 3
        }
        public enum CoughType
        {
            DryCough,            // سعال جاف
            ProductiveCough      // سعال مصحوب ببلغم
        }
        public enum CoughSeverity
        {
            Mild,
            Moderate,
            Severe
        }
        public enum BreathingEffort
        {
            Normal,              // لا، تنفسه طبيعي
            FastBreathing,       // نعم، تنفسه سريع
            LooksExhausted       // نعم، يبدو عليه التعب أثناء التنفس
        }
        public enum PhlegmStatus
        {
            EasyToExit,          // نعم، البلغم خفيف ويخرج بسهولة
            DifficultToExit,     // لا، البلغم كثيف ويصعب خروجه
            NoPhlegm             // لا يوجد بلغم
        }
        public enum FeedingAbility
        {
            CanComplete,         // نعم
            StopsToCatchBreath,  // لا، يتوقف لالتقاط أنفاسه
            DifficultyCompleting // يصعب بسرعة ولا يكمل
        }
        public enum RecurrentChestIssues
        {
            Significantly,       // نعم، تتكرر بشكل ملحوظ
            Sometimes,           // أحياناً
            Rarely               // نادراً
        }
        public enum HeartCondition
        {
            Yes,                 // نعم
            No,                  // لا
            IDontKnow            // لا أعلم
        }
        public enum HasChestIndrawing
        {
            Yes,                 // نعم
            No,                  // لا
            IDontKnow            // لا أعلم
        }


    }
}
