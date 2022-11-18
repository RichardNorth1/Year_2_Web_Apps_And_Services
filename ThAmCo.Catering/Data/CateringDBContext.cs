using Microsoft.EntityFrameworkCore;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ThAmCo.Catering.Data
{
    public class CateringDBContext : DbContext
    {
        public string DbPath { get; }

        public DbSet<FoodBooking> FoodBooking { get; set; }
        public DbSet<FoodItem> FoodItem { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuFoodItem> MenuFoodItem { get; set; }

        public CateringDBContext(DbContextOptions<CateringDBContext> options) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "Catering.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key
            modelBuilder.Entity<MenuFoodItem>()
                .HasKey(a => new { a.MenuId, a.FoodItemId });



            modelBuilder.Entity<MenuFoodItem>()
                .HasOne(a => a.Menu)
                .WithMany(m => m.MenuItems)
                .HasForeignKey(m => m.MenuId);

            modelBuilder.Entity<MenuFoodItem>()
                .HasOne(f => f.Fooditem)
                .WithMany(m => m.Menus)
                .HasForeignKey(f => f.FoodItemId);

            modelBuilder.Entity<FoodItem>()
                .HasData(
                new FoodItem(1,"fish", 6.00),
                new FoodItem(2, "chips", 2.50),
                new FoodItem(3, "peas", 1.00),
                new FoodItem(4, "carrots", 1.00),
                new FoodItem(5, "steak", 9.00),
                new FoodItem(6, "lobster", 10.00),
                new FoodItem(7, "gammon", 5.00)
                );


            modelBuilder.Entity<MenuFoodItem>()
                .HasData(
                new MenuFoodItem(1, 1),
                new MenuFoodItem(1, 2),
                new MenuFoodItem(1, 3),
                new MenuFoodItem(2, 1),
                new MenuFoodItem(2, 4),
                new MenuFoodItem(2, 5),
                new MenuFoodItem(2, 6)
                );

            modelBuilder.Entity<Menu>()
                .HasData(
                new Menu(1,"Menu 1"),
                new Menu(2, "Menu 2")
                );
            modelBuilder.Entity<FoodBooking>()
                .HasData(
                new FoodBooking(1, 1, 50, 1),
                new FoodBooking(2, 2, 50, 2)
                );


        }
    }
}
