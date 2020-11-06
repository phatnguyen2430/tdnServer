using ApplicationCore.Entities.Identity;
using ApplicationCore.Models;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Identity
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
        Task<LogicResult<object>> SendRecoverLinkAsync(string email);
        Task<LogicResult<object>> VerifyRecoverLinkAsync(string email, string token);
        Task<LogicResult<object>> ResetPasswordAsync(string email, string token, string newPassword);
    }
}
