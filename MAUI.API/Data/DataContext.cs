using MAUI.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAUI.API.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }

    public DbSet<CountItems> CountItems { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemOption> ItemOptions { get; set; }
    public DbSet<Sheet> Sheets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItemOption>()
                    .HasKey(io => new { io.ItemId, io.UOM });

    }

}
