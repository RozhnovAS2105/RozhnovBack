using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RozhnovBack.Models;

namespace RozhnovBack.Data
{
    public class RozhnovBackContext : DbContext
    {
        public RozhnovBackContext (DbContextOptions<RozhnovBackContext> options)
            : base(options)
        {
        }

        public DbSet<RozhnovBack.Models.Room> Room { get; set; } = default!;

        public DbSet<RozhnovBack.Models.Guest>? Guest { get; set; }

        public DbSet<RozhnovBack.Models.Reservation>? Reservation { get; set; }

        // Метод для настройки точности и масштаба свойств decimal
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка точности и масштаба для свойств типа decimal
            modelBuilder.Entity<Room>()
                .Property(r => r.PricePerNight)
                .HasColumnType("decimal(18,2)"); // Указываем точность и масштаб для PricePerNight

            modelBuilder.Entity<Reservation>()
                .Property(r => r.RoomPricePerNight)
                .HasColumnType("decimal(18,2)"); // Указываем точность и масштаб для RoomPricePerNight
        }
    }
}
