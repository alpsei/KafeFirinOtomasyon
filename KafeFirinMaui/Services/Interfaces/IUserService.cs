using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeFirinMaui.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<Users>> GetUsersAsync();
        Task<bool> CreateUsersAsync(Users user, int? createdBy = null);
        Task<bool> UpdateUsersAsync(Users user, int? updatedBy);
        Task<bool> DeleteUsersAsync(int userId);
        Task<bool> DeleteUsersAsync(string username);
        Task<Users> GetUserByIdAsync(int userId);
        Task<Users> GetUserByUsernameAsync(string username);
        Task<Users> UserLogin(string username, string password);
        Task<bool> UpdatePasswordAsync(string username, string secQuestion, string secAnswer, string newPassword);
        Task<Users?> GetUserByUsernameAndSecurity(string username, string secQuestion, string secAnswer);
    }
}
