using MHCI.Application.Interfaces;
using MHCI.Application.Models;
using MHCI.Infrastructure.Stores;

namespace MHCI.Application.Services
{
    public class UserService : IUserService
    {
        public UserModel? Authenticate(string email, string password)
        {
            var user = UserStore.Users.Where(x => string.Compare(x.Email, email, true) == 0 && x.Password.Equals(password))?.FirstOrDefault();

            return new UserModel
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Role = user.Role
            };
        }
    }
}
