using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
  
  namespace HeroAPI.Data
  {
    public class ApplicationDbContext :IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        :base(options)
        {
        }

        public DbSet<Hero> Hero { get; set; }

        public DbSet<TokenStore> TokenStore { get; set; }

        /// <summary>
        /// Setup indexes and table relations
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
          base.OnModelCreating(builder);

          builder.Entity<TokenStore>()
            .HasIndex(t => new { t.Token});

          builder.Entity<Hero>()
            .HasIndex(h => new { h.HeroName });
        }
    }
  }