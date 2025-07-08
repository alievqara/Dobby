using Microsoft.EntityFrameworkCore;
using Both.Models;



namespace Both.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserProfile> Users { get; set; } = default!;
    public DbSet<TaskItem> Tasks { get; set; } = default!;
    public DbSet<Expense> Expenses { get; set; } = default!;

}
