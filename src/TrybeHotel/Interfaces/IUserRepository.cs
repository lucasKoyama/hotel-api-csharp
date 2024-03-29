using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public interface IUserRepository
    {
        UserDto GetUserById(int userId);
        UserDto Add(UserDtoInsert user);
        UserDto Login(LoginDto login);
        UserDto GetUserByEmail(string userEmail);
        bool EmailAlreadyUsed(string userEmail);
        IEnumerable<UserDto> GetUsers();
        public bool Save();
    }

}