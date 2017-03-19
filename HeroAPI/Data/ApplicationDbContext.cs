  using Microsoft.EntityFrameworkCore;
  
  namespace HeroAPI.Data
  {
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        :base(options)
        {
        }

        public DbSet<Hero> Hero { get; set; }
    }
  }