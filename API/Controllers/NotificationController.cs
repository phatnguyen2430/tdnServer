using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.NotificationAggregate;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Notification;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : BaseApiController
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IService _service;
        private readonly IMapper _mapper;
        #endregion

        #region ctor
        public NotificationController(IUnitOfWork unitOfWork,
            IService service,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _service = service;
            _mapper = mapper;
        }
        #endregion
        //create
        [AllowAnonymous]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationRequestModel model)
        {
            try
            {
                var newNotification = new NotificationResponseModel();
                if (!ModelState.IsValid)
                {
                    ValidModel();
                }
                //check if user is existing

                //else -> add answer
                var newNoti = new Notification()
                {
                    Content = model.Content,
                    IsChecked = model.IsChecked,
                    
                    Type = model.Type,
                    UserId = model.UserId,
                    testId = model.TestId
                };
                await _service.NotificationService.AddAsync(newNoti);

                var responseModel = new NotificationResponseModel()
                {
                    Id = newNoti.Id,
                    Content = newNoti.Content,
                    CreatedOnUtc = newNoti.CreatedOnUtc,
                    IsChecked = newNoti.IsChecked,
                    Type = newNoti.Type,
                    UserId = newNoti.UserId
                };


                return SuccessResult(responseModel, "Created Notification successfully.");
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }

        //get by Id
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNotificationById([FromRoute] int id)
        {
            try
            {
                var notification = await _service.NotificationService.GetByIdAsync(id);
                if (notification == null)
                {
                    return Unauthorized($"Can not found Notification with Id: {id}");
                }
                var notificationRes = new NotificationResponseModel()
                {
                    Id = notification.Id,
                    Content = notification.Content,
                    CreatedOnUtc = notification.CreatedOnUtc,
                    IsChecked = notification.IsChecked,
                    Type = notification.Type,
                    UpdatedOnUtc = notification.UpdatedOnUtc,
                    UserId = notification.UserId
                };
                return SuccessResult(notificationRes, "Get Notification successfully.");
            }
            catch (Exception e)
            {
                return Unauthorized(e.ToString());                
            }
        }

        //get all by userId
        [HttpGet("user/{userId}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNotificationByUserId([FromRoute] int userId)
        {
            try
            {
                var notifications = await _service.NotificationService.GetByUserIdAsync(userId);
                if (notifications == null)
                {
                    return Unauthorized($"Can not found Notification with Id: {userId}");
                }
                var notificationsRes = new List<NotificationResponseModel>();
                foreach (var notification in notifications)
                {
                    var newNotification = new NotificationResponseModel()
                    {
                        Id = notification.Id,
                        Content = notification.Content,
                        CreatedOnUtc = notification.CreatedOnUtc,
                        IsChecked = notification.IsChecked,
                        Type = notification.Type,
                        UpdatedOnUtc = notification.UpdatedOnUtc,
                        UserId = notification.UserId
                    };
                    notificationsRes.Add(newNotification);
                }

                return SuccessResult(notificationsRes, "Get Notifications successfully.");
            }
            catch (Exception e)
            {
                return Unauthorized(e.ToString());
            }
        }

        //get all not read


        //get all have read


        //update
        [AllowAnonymous]
        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult>UpdateNotification([FromBody] NotificationResponseModel model)
        {
            try
            {
                var notiOnServer = await _unitOfWork.NotificationRepository.GetByIdAsync(model.Id);

                var newNotification = new NotificationResponseModel();
                if (!ModelState.IsValid)
                {
                    ValidModel();
                }
                
                // check notification model
                if(model.Type.ToString() != null)
                {
                    notiOnServer.Type = model.Type;
                }
                if(model.IsChecked.ToString() != null)
                {
                    notiOnServer.IsChecked = model.IsChecked;
                }
                if (model.Content != null)
                {
                    notiOnServer.Content = model.Content;
                }

                await _service.NotificationService.UpdateAsync(notiOnServer);

                var responseModel = new NotificationResponseModel()
                {
                    Id = notiOnServer.Id,
                    Content = notiOnServer.Content,
                    CreatedOnUtc = notiOnServer.CreatedOnUtc,
                    IsChecked = notiOnServer.IsChecked,
                    Type = notiOnServer.Type,
                    UserId = notiOnServer.UserId
                };


                return SuccessResult(responseModel, "Update Notification successfully.");
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }

    }
}
