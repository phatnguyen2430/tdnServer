using ApplicationCore.Entities.Identity;
using ApplicationCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Identity
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password, string name,
            int age, string address, string phoneNumber,string role);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
        Task<LogicResult<object>> SendRecoverLinkAsync(string email);
        Task<LogicResult<object>> VerifyRecoverLinkAsync(string email, string token);
        Task<LogicResult<object>> ResetPasswordAsync(string email, string token, string newPassword);
        Task<User> GetByIdAsync(int id);
        Task<User> UpdateUserAsync(User user);
        Task<List<User>> GetAllPaging(int pageSize, int pageIndex);
        Task<User> UpdatePasswordAsync(User user, string currentPassword, string newPassword);
        Task<User> UpdateEmailAsync(User user, string newEmail, string token);
        Task<bool> DeleteUserAsync(User user);
        Task<int> GetUserIdByEmail(string email);
        Task<string> GetUserRoleByEmail(string email);
    }
}
