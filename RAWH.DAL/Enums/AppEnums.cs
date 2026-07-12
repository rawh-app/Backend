namespace RAWH.DAL.Enums
{
    public class AppEnums
    {
        public enum Gender
        {
            Male ,
            Female
        }
        public enum FeverDuration
        {
            LessThan24Hours,     
            From1To3Days,        
            MoreThan3Days,       
            NormalTemperature    
        }
        public enum FeverResponse
        {
            Unknown,
            ReducedEasily,       
            ReducedWithDifficulty,
            NotReduced,          
            DidNotTakeMedicine   
        }

        public enum FeverLevel
        {
            Unknown,             
            LessThan38,          
            From38To39,          
            MoreThan39,          
            NotMeasured          
        }
        public enum CoughType
        {
            Unknown,
            DryCough,            
            ProductiveCough      
        }
        public enum CoughSeverity
        {
            Unknown,
            Mild,
            Moderate,
            Severe
        }
        public enum PhlegmStatus
        {
            Unknown,
            EasyToExit,        
            DifficultToExit,    
            NoPhlegm             
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
            None,
            StartedToday,
            FewDays,
            AllDay
        }
       
        public enum BreathingEffort
        {
            Normal,              // لا، تنفسه طبيعي
            FastBreathing,       // نعم، تنفسه سريع
            LooksExhausted       // نعم، يبدو عليه التعب أثناء التنفس
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
            Not_Sure            // لا أعلم
        }

        public enum AgeUnit
        {
            Days,
            Months,
            Years
        }

    }
}
