namespace TrybeHotel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 1. Implemente as models da aplicação
// TO-DO: show this CityId, virtual City City in the explanation of the model creation in my gist repo!
public class Hotel {
  [Key]
  public int HotelId { get; set; }
  public string? Name { get; set; }
  public string? Address { get; set; }

  [ForeignKey("CityId")]
  public int CityId { get; set; }
  public virtual City? City { get; set; } = null!; // virtual is used here to tell that City will exist, and its the city from the Id above
  public virtual ICollection<Room>? Rooms { get; set; } = null!; // virtual telling that this entity will exist
}