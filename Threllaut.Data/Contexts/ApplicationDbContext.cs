using Microsoft.EntityFrameworkCore;

namespace Threllaut.Data.Contexts;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options)
    : DbContext(options), IDbContext
{
    public static string? Schema => "app";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>().ToTable("Users", "identity");
    }
}
