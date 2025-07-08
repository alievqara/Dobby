using Microsoft.EntityFrameworkCore;



namespace Both.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserProfile> Users { get; set; }

}
