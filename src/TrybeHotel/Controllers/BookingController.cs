using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TrybeHotel.Dto;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("booking")]
  
    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert)
        {
            try
            {
                var token = HttpContext.User.Identity as ClaimsIdentity;
                var email = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                var roomToBook = _repository.GetRoomById(bookingInsert.RoomId); // should be in a services layer
                if (bookingInsert.GuestQuant > roomToBook.Capacity)
                    throw new BadHttpRequestException("Guest quantity over room capacity");

                return Created("", _repository.Add(bookingInsert, email));
            }
            catch (BadHttpRequestException err)
            {
                return BadRequest(new { message = err.Message });
            }
            catch (InvalidDataException err)
            {
                return Unauthorized(new { message = err.Message });
            }
        }


        [HttpGet("{Bookingid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult GetBooking(int Bookingid){
           try
           {
                var token = HttpContext.User.Identity as ClaimsIdentity;
                var email = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                return Ok(_repository.GetBooking(Bookingid, email));
           }
           catch (InvalidDataException err)
           {
                return Unauthorized(new { message = err.Message });
           }
           catch (NullReferenceException err)
           {
                return NotFound(new { message = err.Message });
           }
           catch (UnauthorizedAccessException err)
           {
                return Unauthorized(new { message = err.Message });
           }
        }
    }
}