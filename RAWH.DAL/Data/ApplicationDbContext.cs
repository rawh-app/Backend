using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace RAWH.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>

    {
        private readonly IEncryptionProvider _encryptionProvider;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            _encryptionProvider = new GenerateEncryptionProvider("63fdf5d627c30b56");
        }
        public DbSet<Student> Students { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
