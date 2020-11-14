using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
            var result = await _service.IdentityService.RegisterAsync(model.Email, model.Password,
                model.Name,
                model.Age,
                model.Address,
                model.PhoneNumber);

            //get user Id
            var userId = await _service.IdentityService.GetUserIdByEmail(model.Email);

            if (!result.IsSuccess)
            {
                return ErrorResult(result.ErrorMessages);
            }
            return SuccessResult(new AuthSuccessResponse
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken,
                UserId = userId
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
            //get user Id
            var userId = await _service.IdentityService.GetUserIdByEmail(model.UserName);
            if (!result.IsSuccess)
            {
                return ErrorResult(result.ErrorMessages);
            }
            return SuccessResult(new AuthSuccessResponse
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken,
                UserId = userId
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

        //update user detail
        [AllowAnonymous]
        [HttpPost("update-detail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserDetail([FromBody] UpdateDetailModel model)
        {
            try
            {
                var userOnServer = await _service.IdentityService.GetByIdAsync(model.Id);
                //change data of user
                if (model.Name != null)
                {
                    userOnServer.Name = model.Name;
                }
                if(model.Age.ToString() != null)
                {
                    userOnServer.Age = model.Age;
                }

                if(model.Address != null)
                {
                    userOnServer.Address = model.Address;
                }

                if(model.PhoneNumber != null)
                {
                    userOnServer.PhoneNumber = model.PhoneNumber;
                }
                var queryRes = await _service.IdentityService.UpdateUserAsync(userOnServer);

                //map entity to response model
                var resAccountModel = new AccountResponseModel()
                {
                    Address = queryRes.Address,
                    Age = queryRes.Age,
                    Email = queryRes.Email,
                    Id = queryRes.Id,
                    Name = queryRes.Name,
                    PhoneNumber = queryRes.PhoneNumber
                };
                return SuccessResult(resAccountModel, "Update User Detail successfully.");
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
        }

        //update user password
        [AllowAnonymous]
        [HttpPost("update-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdatePasswordModel model)
        {
            try
            {
                var userOnServer = await _service.IdentityService.GetByIdAsync(model.UserId);
                //update user password
                var queryRes = await _service.IdentityService.UpdatePasswordAsync(userOnServer,model.CurrentPassword, model.NewPassword);
                var resAccountModel = new AccountResponseModel()
                {
                    Address = queryRes.Address,
                    Age = queryRes.Age,
                    Email = queryRes.Email,
                    Id = queryRes.Id,
                    Name = queryRes.Name,
                    PhoneNumber = queryRes.PhoneNumber
                };


                return SuccessResult(resAccountModel, "Update User Password successfully.");
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
        }

        //update user email
        [AllowAnonymous]
        [HttpPost("update-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserEmail([FromBody] UpdateEmailModel model)
        {
            try
            {
                var userOnServer = await _service.IdentityService.GetByIdAsync(model.UserId);
                //update user password
                var queryRes = await _service.IdentityService.UpdateEmailAsync(userOnServer, model.NewEmail, model.CurrentToken);
                var resAccountModel = new AccountResponseModel()
                {
                    Address = queryRes.Address,
                    Age = queryRes.Age,
                    Email = queryRes.Email,
                    Id = queryRes.Id,
                    Name = queryRes.Name,
                    PhoneNumber = queryRes.PhoneNumber
                };

                return SuccessResult(resAccountModel, "Update User Email successfully.");
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
        }

        //delete user
        [AllowAnonymous]
        [HttpPost("delete-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(int UserId)
        {
            try
            {
                var userOnServer = await _service.IdentityService.GetByIdAsync(UserId);
                //update user password
                var queryRes = await _service.IdentityService.DeleteUserAsync(userOnServer);
                if (queryRes)
                {
                    return SuccessResult(queryRes, "Delete User successfully.");
                }
                else
                {
                    return ErrorResult("Failed to delete User has Id =" + UserId.ToString() + " .");
                }
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
        }

        //get all user paging
        [AllowAnonymous]
        [HttpPost("get-all-paging")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPaging([FromBody] AccountPagingModel model)
        {
            var newUsers = new List<AccountResponseModel>();
            try
            {
                var queryRes = await _service.IdentityService.GetAllPaging(model.PageSize, model.PageIndex);
                if (queryRes.Count() > 0)
                {
                    //render page
                    newUsers = queryRes.Select(x => new AccountResponseModel
                    {
                        Id = x.Id,
                        Address = x.Address,
                        Age = x.Age,
                        Email = x.Email,
                        Name = x.Name,
                        PhoneNumber = x.PhoneNumber
                    }).ToList();
                    return SuccessResult(newUsers, "Get all Users with paging successfully.");
                }
                else
                {
                    return SuccessResult(newUsers, "Get all Users with paging successfully.");
                }
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
        }

    }
}