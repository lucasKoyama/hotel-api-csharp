using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.EntityFrameworkCore;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 6. Desenvolva o endpoint GET /room/:hotelId
        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
            return _context.Rooms
                .Include(r => r.Hotel)
                .ThenInclude(h => h.City)
                .Where(r => r.HotelId == HotelId)
                .Select(room => new RoomDto{
                    RoomId = room.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    Hotel = new HotelDto {
                        HotelId = room.HotelId,
                        Name = room.Hotel.Name,
                        Address = room.Hotel.Address,
                        CityId = room.Hotel.CityId,
                        CityName = room.Hotel.City.Name,
                        State = room.Hotel.City.State,
                    }
                });
        }

        // 7. Desenvolva o endpoint POST /room
        public RoomDto AddRoom(Room room) {
            _context.Rooms.Add(room);
            Save();
            var newRoom = _context.Rooms
                .Include(r => r.Hotel)
                .ThenInclude(h => h.City)
                .Where(r => r.RoomId == room.RoomId)
                .Single();

            return new RoomDto {
                RoomId = newRoom.RoomId,
                Name = newRoom.Name,
                Capacity = newRoom.Capacity,
                Image = newRoom.Image,
                Hotel = new HotelDto {
                    HotelId = newRoom.HotelId,
                    Name = newRoom.Hotel.Name,
                    Address = newRoom.Hotel.Address,
                    CityId = newRoom.Hotel.CityId,
                    CityName = newRoom.Hotel.City.Name,
                    State = room.Hotel.City.State,
                }
            };
        }

        // 8. Desenvolva o endpoint DELETE /room/:roomId
        public void DeleteRoom(int RoomId) {
            var room = _context.Rooms.Include(r => r.Hotel).Single(r => r.RoomId == RoomId);
            _context.Rooms.Remove(room);
            Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}