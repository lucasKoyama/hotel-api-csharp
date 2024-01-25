using Microsoft.EntityFrameworkCore;
using TrybeHotel.Models;

namespace TrybeHotel.Repository;
public class TrybeHotelContext : DbContext, ITrybeHotelContext
{
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<Hotel> Hotels { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Booking> Bookings { get; set; } = null!;
    public TrybeHotelContext(DbContextOptions<TrybeHotelContext> options) : base(options) {
    }
    public TrybeHotelContext() { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = "Server=localhost;User=SA;Password=TrybeHotel12!;TrustServerCertificate=True";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hotel>()
            .HasOne(h => h.City)
            .WithMany(c => c.Hotels)
            .HasForeignKey(h => h.CityId);

        modelBuilder.Entity<Room>()
            .HasMany(b => b.Bookings)
            .WithOne(r => r.Room)
            .HasForeignKey(b => b.RoomId);

        modelBuilder.Entity<User>()
            .HasMany(b => b.Bookings)
            .WithOne(u => u.User)
            .HasForeignKey(b => b.UserId);
    }

}