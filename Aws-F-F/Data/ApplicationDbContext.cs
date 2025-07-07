using Microsoft.EntityFrameworkCore;
using Aws_F_F.Models;

namespace Aws_F_F.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Video> Videos { get; set; }
        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // يمكن إضافة المزيد من التخصيصات هنا إذا لزم الأمر
        }
    }
    
}
