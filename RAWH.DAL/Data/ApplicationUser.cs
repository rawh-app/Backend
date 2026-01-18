using EntityFrameworkCore.EncryptColumn.Attribute;
using Microsoft.AspNetCore.Identity;
namespace RAWH.DAL.Data
{
    public class ApplicationUser : IdentityUser
    {
        [EncryptColumn]
        public string resetPasswordEmail { get; set; }
        public string? GoogleId { get; set; }
        public PneumoniaSurveyRequest pneumoniaSurveyRequest{ get; set; }
}
}
