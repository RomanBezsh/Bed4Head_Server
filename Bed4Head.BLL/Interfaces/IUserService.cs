using Bed4Head.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bed4Head.BLL.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO?> GetUserByEmailAsync(string email);
        Task RegisterAsync(UserDTO userDto, string password);
        Task<bool> VerifyPasswordAsync(string email, string password);
    }
}
