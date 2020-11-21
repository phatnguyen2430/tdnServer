using ApplicationCore.Entities.Identity;
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

        #region admin service
        /// <summary>
        /// register api
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("admin/register")]
        [AllowAnonymous]
        public async Task<IActionResult> AdminRegister([FromBody] RegistrationRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Unauthorized(ModelState.Values.SelectMany(p => p.Errors.Select(t => t.ErrorMessage).ToList()).ToList());
                }
                var result = await _service.IdentityService.RegisterAsync(model.Email, model.Password,
                    model.Name,
                    model.Age,
                    model.Address,
                    model.PhoneNumber,
                    "admin");

                //get user Id
                var userId = await _service.IdentityService.GetUserIdByEmail(model.Email);

                if (!result.IsSuccess)
                {
                    return Unauthorized(result.ErrorMessages);
                }
                return SuccessResult(new AuthSuccessResponse
                {
                    Token = result.Token,
                    RefreshToken = result.RefreshToken,
                    UserId = userId
                });
            }
            catch (Exception e )
            {
                return Unauthorized(e.ToString());
            }
        }

        [HttpPost("admin/login")]
        [AllowAnonymous]
        public async Task<IActionResult> AdminLogin([FromBody] LoginRequest model)
        {
            try
            {
                var result = await _service.IdentityService.LoginAsync(model.Email, model.Password);
                //get user Id
                var userId = await _service.IdentityService.GetUserIdByEmail(model.Email);

                //check role
                var role = await _service.IdentityService.GetUserRoleByEmail(model.Email);
                if (role != "admin")
                {
                    return Unauthorized("This is " + role + " account not Admin Account");
                }

                if (!result.IsSuccess)
                {
                    return Unauthorized(result.ErrorMessages);
                }
                return SuccessResult(new AuthSuccessResponse
                {
                    Token = result.Token,
                    RefreshToken = result.RefreshToken,
                    UserId = userId
                });
            }
            catch (Exception e)
            {
                return Unauthorized(e.ToString());
            }
        }

        #endregion


        /// <summary>
        /// register api
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Unauthorized(ModelState.Values.SelectMany(p => p.Errors.Select(t => t.ErrorMessage).ToList()).ToList());
                }
                var result = await _service.IdentityService.RegisterAsync(model.Email, model.Password,
                    model.Name,
                    model.Age,
                    model.Address,
                    model.PhoneNumber,
                    "student");

                //get user Id
                var userId = await _service.IdentityService.GetUserIdByEmail(model.Email);

                if (!result.IsSuccess)
                {
                    return Unauthorized(result.ErrorMessages);
                }
                return SuccessResult(new AuthSuccessResponse
                {
                    Token = result.Token,
                    RefreshToken = result.RefreshToken,
                    UserId = userId
                });
            }
            catch (Exception e)
            {
                return Unauthorized(e.ToString());
            }
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
            try
            {
                var result = await _service.IdentityService.LoginAsync(model.Email, model.Password);
                //get user Id
                var userId = await _service.IdentityService.GetUserIdByEmail(model.Email);

                //check role
                var role = await _service.IdentityService.GetUserRoleByEmail(model.Email);
                if (role != "student")
                {
                    return Unauthorized("This is " + role + " account not student account");
                }

                if (!result.IsSuccess)
                {
                    return Unauthorized(result.ErrorMessages);
                }
                return SuccessResult(new AuthSuccessResponse
                {
                    Token = result.Token,
                    RefreshToken = result.RefreshToken,
                    UserId = userId
                });
            }
            catch (Exception e)
            {
                return Unauthorized(e.ToString());
            }
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest model)
        {
            try
            {
                var result = await _service.IdentityService.RefreshTokenAsync(model.Token, model.RefreshToken);
                if (!result.IsSuccess)
                {
                    return Unauthorized(result.ErrorMessages);
                }
                return SuccessResult(new AuthSuccessResponse
                {
                    Token = result.Token,
                    RefreshToken = result.RefreshToken
                });
            }
            catch (Exception e)
            {
                return Unauthorized(e.ToString());
            }
        }

        //update user detail
        [AllowAnonymous]
        [HttpPost("update/detail")]
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
                return Unauthorized(e.Message);
            }
        }

        //update user password
        [AllowAnonymous]
        [HttpPut("update/password")]
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
                return Unauthorized(e.Message);
            }
        }

        //update user email
        [AllowAnonymous]
        [HttpPut("update/email")]
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
                return Unauthorized(e.Message);
            }
        }

        //delete user
        [AllowAnonymous]
        [HttpDelete("delete/user")]
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
                    return Unauthorized("Failed to delete User has Id =" + UserId.ToString() + " .");
                }
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }

        //get all user paging
        [AllowAnonymous]
        [HttpPost("get/all/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPaging([FromRoute] int pageIndex)
        {
            var newUsers = new List<AccountResponseModel>();
            try
            {
                var queryRes = await _service.IdentityService.GetAllPaging(25, pageIndex);
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
                return Unauthorized(e.Message);
            }
        }


        //active -> list ids
        //update user detail
        [AllowAnonymous]
        [HttpPut("update/active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateActiveUser([FromBody] List<int> ids)
        {
            try
            {
                var users = new List<User>();
                //get all user from ids list 
                foreach (var id in ids)
                {
                    var user = await _service.IdentityService.GetByIdAsync(id);
                    user.IsActive = true;
                    users.Add(user);
                }

                //change data of user
                foreach (var user in users)
                {
                    await _service.IdentityService.UpdateUserAsync(user);
                }

                return SuccessResult("Users Activate successfully.");
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }
    }
}