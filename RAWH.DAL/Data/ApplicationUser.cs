using EntityFrameworkCore.EncryptColumn.Attribute;
using Microsoft.AspNetCore.Identity;
namespace RAWH.DAL.Data
{
    public class ApplicationUser : IdentityUser
    {
        [EncryptColumn]
        public string resetPasswordEmail { get; set; }
    }
}
