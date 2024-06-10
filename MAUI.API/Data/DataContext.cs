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

        AddSeedData(modelBuilder);
    }

    private static void  AddSeedData(ModelBuilder modelBuilder)
    {
            Item[] items = [
                new Item { Id = 1, ItemName = "ItemA", InitialCount = 1, Expiry = "0000-00-00", Lot = "0000-00-00", InventoryDate = "0000-00-00", QuantityCount = 10, User = "Employee A" },
                new Item { Id = 2, ItemName = "ItemB", InitialCount = 1, Expiry = "0000-00-00", Lot = "0000-00-00", InventoryDate = "0000-00-00", QuantityCount = 10, User = "Employee A" }
            ];
            ItemOption[] itemOptions = [
                new ItemOption { ItemId = 1, UOM = "Pcs"},
                new ItemOption { ItemId = 1, UOM = "Box"}
        ];

        modelBuilder.Entity<Item>()
            .HasData(items);

        modelBuilder.Entity<ItemOption>()
            .HasData(itemOptions);

    }

}
