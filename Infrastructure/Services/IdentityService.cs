using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces.Identity;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.Repositories.Identity;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Email;

namespace Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IEmailService _emailService;
        private readonly Settings _settings;
        public IdentityService(UserManager<User> userManager,
            JwtSettings jwtSettings,
            TokenValidationParameters tokenValidationParameters,
            IUnitOfWork unitOfWork, IRefreshTokenRepository refreshTokenRepository,
            IEmailService emailService, Settings settings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
            _emailService = emailService;
            _settings = settings;
        }


        public async Task<AuthenticationResult> RegisterAsync(string email, string password, string name,
            int age, string address, string phoneNumber)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new List<string> { "User is already exists" }
                };
            }
            var newUser = new User
            {
                Email = email,
                UserName = email,
                Name = name,
                Address = address,
                Age = age,
                PhoneNumber = phoneNumber
            };
            var result = await _userManager.CreateAsync(newUser, password);

            //get user Id
            var userId = newUser.Id;
            //add role : 1-> student, 2->admin
            await _userManager.AddToRoleAsync(newUser, "student");


            if (!result.Succeeded)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = result.Errors.Select(p => p.Description).ToList()
                };
            }

            return await GenerateAuthenticationResultForUserAsync(newUser);
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new List<string> { "User does not exist" }
                };
            }
            var hasValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!hasValidPassword)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new List<string> { "User or password combination is wrong" }
                };
            }
            return await GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetClaimsPrincipalFromToken(token);
            if (validatedToken == null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new List<string> { "Invalid token" }
                };
            }

            var expiryDateUnix =
                long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix);
            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new List<string> { "This token hasn't expired yet" }
                };
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            var storeRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (storeRefreshToken == null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new List<string> { "This refresh token does not exist" }
                };
            }

            if (DateTime.UtcNow > storeRefreshToken.ExpiredOnUtc)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new List<string> { "This refresh token has expired" }
                };
            }

            if (storeRefreshToken.Invalidated)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new List<string> { "This refresh token has been invalidated" }
                };
            }

            if (storeRefreshToken.JwtId != jti)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new List<string> { "This refresh token does not match JWT" }
                };
            }

            storeRefreshToken.Used = true;
            await _refreshTokenRepository.UpdateAsync(storeRefreshToken);
            await _unitOfWork.SaveChangesAsync();
            var user = await _userManager.FindByIdAsync((validatedToken.Claims.Single(x => x.Type == "id").Value));
            return await GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<LogicResult<object>> SendRecoverLinkAsync(string email)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                return new LogicResult<object>
                {
                    Errors = new List<string> { "User does not exist" }
                };
            }
            var sendTo = new List<string> { existingUser.Email };
            var subject = "Password Recovery Requested";
            var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
            var body =
            $@"
            <div>
                Hello,
                <br>
                <br>
                To reset your account password, please visit the following link within 24 hours:
                <br>
                <br>
                <a href='{_settings.Domain}/auth/recover-password?email={existingUser.Email}&token={token}'
                   target='_blank'>{_settings.Domain}/auth/recover-password?email={existingUser.Email}&token={token}</a>
                <br>
                <br>
                Regards,
                <br>
                <br>
                The remote printing team.
            </div>
            ";
            await _emailService.SendEmailAsync(sendTo, subject, body);
            return new LogicResult<object>
            {
                IsSuccess = true
            };
        }

        public async Task<LogicResult<object>> VerifyRecoverLinkAsync(string email, string token)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                return new LogicResult<object>
                {
                    Errors = new List<string> { "User does not exist" }
                };
            }
            var verifyingData = await _userManager.VerifyUserTokenAsync(existingUser, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);
            if (!verifyingData)
            {
                return new LogicResult<object>
                {
                    Errors = new List<string> { "Token is invalid" }
                };
            }
            return new LogicResult<object>
            {
                IsSuccess = true
            };
        }

        public async Task<LogicResult<object>> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                return new LogicResult<object>
                {
                    Errors = new List<string> { "User does not exist" }
                };
            }
            var resetRequest = await _userManager.ResetPasswordAsync(existingUser, token, newPassword);
            if (!resetRequest.Succeeded)
            {
                return new LogicResult<object>
                {
                    Errors = new List<string> { "Token is not correct" }
                };
            }
            return new LogicResult<object>
            {
                IsSuccess = true
            };
        }

        private ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                return principal;
            }
            catch (Exception)
            {

                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                ExpiredOnUtc = DateTime.UtcNow.AddDays(30),
                Token = Guid.NewGuid().ToString()
            };
            await _refreshTokenRepository.AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return new AuthenticationResult
            {
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token,
                IsSuccess = true
            };
        }

        /// <summary>
        /// get by Id async
        /// </summary>
        /// <param id="user Id"></param>
        /// <returns></returns>
        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return user;
        }

        /// <summary>
        /// Update async
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> UpdateUserAsync(User user)
        {
            await _userManager.UpdateAsync(user);
            return user;
        }

        /// <summary>
        /// get all paging
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<User>> GetAllPaging(int pageSize, int pageIndex)
        {
            //get all users
            var users = await _userManager.GetUsersInRoleAsync("student");
            //query 
            var result = users.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return result;
        }

        /// <summary>
        /// update new password with hash code
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newPassword"></param>
        /// <param name="isValidated"></param>
        /// <returns></returns>
        public async Task<User> UpdatePasswordAsync(User user, string currentPassword, string newPassword)
        {
            //update user password hash
            await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return user;
        }

        /// <summary>
        /// change email async
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newEmail"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<User> UpdateEmailAsync(User user, string newEmail, string token)
        {
            //update user password hash
            await _userManager.ChangeEmailAsync(user, newEmail, token);
            return user;
        }

        /// <summary>
        /// Delete user async
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUserAsync(User user)
        {
            try
            {
                //update user password hash
                await _userManager.DeleteAsync(user);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<int> GetUserIdByEmail(string email)
        {
            try
            {
                //get user -> get id
                var user = await _userManager.FindByEmailAsync(email);
                return user.Id;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

    }
}
