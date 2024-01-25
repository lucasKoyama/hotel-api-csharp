using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.EntityFrameworkCore;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            var user = _context.Users.FirstOrDefault(user => user.Email == email);
            if (user is null)
                throw new InvalidDataException("User not found!");

            var newBooking = new Booking
            {
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                UserId = user.UserId,
                RoomId = booking.RoomId,
            };
            _context.Bookings.Add(newBooking);
            Save();

            var savedBooking = _context.Bookings
                .Include(booked => booked.Room)
                .ThenInclude(room => room.Hotel)
                .ThenInclude(hotel => hotel.City)
                .FirstOrDefault(booked => booked.BookingId == newBooking.BookingId);
            return new BookingResponse {
                BookingId = savedBooking.BookingId,
                CheckIn = savedBooking.CheckIn,
                CheckOut = savedBooking.CheckOut,
                GuestQuant = savedBooking.GuestQuant,
                Room = new RoomDto {
                    RoomId = savedBooking.Room.RoomId,
                    Name = savedBooking.Room.Name,
                    Capacity = savedBooking.Room.Capacity,
                    Image = savedBooking.Room.Image,
                    Hotel = new HotelDto {
                        HotelId = savedBooking.Room.HotelId,
                        Name = savedBooking.Room.Hotel.Name,
                        Address = savedBooking.Room.Hotel.Address,
                        CityId = savedBooking.Room.Hotel.CityId,
                        CityName = savedBooking.Room.Hotel.City.Name,
                        State = savedBooking.Room.Hotel.City.State
                    }
                }
            };
        }

        public BookingResponse GetBooking(int bookingId, string email)
        {
            var user = _context.Users.FirstOrDefault(user => user.Email == email);
            if (user is null)
                throw new InvalidDataException("User not found!");

            var booked = _context.Bookings
                .Include(booked => booked.Room)
                .ThenInclude(room => room.Hotel)
                .ThenInclude(hotel => hotel.City)
                .FirstOrDefault(booked => booked.BookingId == bookingId);
            if (booked is null)
                throw new NullReferenceException("Booking not found!");
            if (user.UserId != booked.UserId)
                throw new UnauthorizedAccessException(
                    $"{user.Name} not Authorized! This booking doesn't belong to {user.Name}"
                );
            return new BookingResponse {
                BookingId = booked.BookingId,
                CheckIn = booked.CheckIn,
                CheckOut = booked.CheckOut,
                GuestQuant = booked.GuestQuant,
                Room = new RoomDto {
                    RoomId = booked.Room.RoomId,
                    Name = booked.Room.Name,
                    Capacity = booked.Room.Capacity,
                    Image = booked.Room.Image,
                    Hotel = new HotelDto {
                        HotelId = booked.Room.HotelId,
                        Name = booked.Room.Hotel.Name,
                        Address = booked.Room.Hotel.Address,
                        CityId = booked.Room.Hotel.CityId,
                        CityName = booked.Room.Hotel.City.Name,
                        State = booked.Room.Hotel.City.State,
                    }
                }
            };
        }

        public Room GetRoomById(int RoomId)
        {
            return _context.Rooms.FirstOrDefault(room => room.RoomId == RoomId);
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }

}