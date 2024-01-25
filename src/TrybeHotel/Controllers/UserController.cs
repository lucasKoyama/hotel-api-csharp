using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("user")]

    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Admin")]
        public IActionResult GetUsers()
        {
            return Ok(_repository.GetUsers());
        }
    
        [HttpPost]
        public IActionResult Add([FromBody] UserDtoInsert user)
        {
            if (_repository.EmailAlreadyUsed(user.Email))
                return Conflict(new { message = "User email already exists" });
            return Created("", _repository.Add(user));
        }
    }
}