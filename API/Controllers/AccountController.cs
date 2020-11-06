using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models.Account;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : BaseApiController
    {
        private readonly IService _service;
        public AccountController(IService service)
        {
            _service = service;
        }

        /// <summary>
        /// register api
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest model)
        {
            if (!ModelState.IsValid)
            {
                return ErrorResult(ModelState.Values.SelectMany(p => p.Errors.Select(t => t.ErrorMessage).ToList()).ToList());
            }
            var result = await _service.IdentityService.RegisterAsync(model.Email, model.Password);
            if (!result.IsSuccess)
            {
                return ErrorResult(result.ErrorMessages);
            }
            return SuccessResult(new AuthSuccessResponse
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken
            });
        }

        /// <summary>
        /// login api
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var result = await _service.IdentityService.LoginAsync(model.UserName, model.Password);
            if (!result.IsSuccess)
            {
                return ErrorResult(result.ErrorMessages);
            }
            return SuccessResult(new AuthSuccessResponse
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken
            });
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest model)
        {
            var result = await _service.IdentityService.RefreshTokenAsync(model.Token, model.RefreshToken);
            if (!result.IsSuccess)
            {
                return ErrorResult(result.ErrorMessages);
            }
            return SuccessResult(new AuthSuccessResponse
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken
            });
        }
    }
}