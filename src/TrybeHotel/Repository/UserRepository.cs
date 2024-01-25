using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public UserDto Login(LoginDto login)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Password == login.Password && u.Email == login.Email);
            if (user is null) throw new InvalidDataException("Incorrect e-mail or password");
            return new UserDto {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType,
            };
        }
        public UserDto Add(UserDtoInsert user)
        {
            _context.Users.Add(new User {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = "client",
            });
            Save();
            var newUser = _context.Users.Where(u => u.Email == user.Email).Single();
            return new UserDto {
                UserId = newUser.UserId,
                Name = newUser.Name,
                Email = newUser.Email,
                UserType = "client",
            };
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            var userFromDB = _context.Users.FirstOrDefault(user => user.Email == userEmail);
            return new UserDto {
                UserId = userFromDB.UserId,
                Name = userFromDB.Name,
                Email = userFromDB.Email,
                UserType = userFromDB.UserType,
            };
        }

        public bool EmailAlreadyUsed(string userEmail)
        {
            return _context.Users.Any(user => user.Email == userEmail);
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var users = _context.Users.Select(user => new UserDto {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType,
            });
            return users;
        }

        public bool Save() {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}