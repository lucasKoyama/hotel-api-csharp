namespace TrybeHotel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 1. Implemente as models da aplicação
public class Room {
  [Key]
  public int RoomId { get; set; }
  public string? Name { get; set; }
  public int Capacity { get; set; }
  public string? Image { get; set; }

  [ForeignKey("HotelId")]
  public int HotelId { get; set; }
  public virtual Hotel? Hotel { get; set; } = null!; // virtual telling that this entity will exist

  public virtual ICollection<Booking>? Bookings { get; set; } = null!;
}