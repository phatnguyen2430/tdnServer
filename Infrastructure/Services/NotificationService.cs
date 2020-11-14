using ApplicationCore.Entities.NotificationAggregate;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.NotificationsService;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        #region fields
        IUnitOfWork _unitOfWork;
        #endregion

        #region ctor
        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Notification> AddAsync(Notification entity)
        {
            await _unitOfWork.NotificationRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public Task<LogicResult<Notification>> CreateNotification(Notification annotation)
        {
            throw new NotImplementedException();
        }

        public Task<Notification> EagerGetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Notification>> GetAllAsync()
        {
            var notifications = await _unitOfWork.NotificationRepository.GetAllAsync();
            return notifications;
        }

        public async Task<Notification> GetByIdAsync(int id)
        {
            return await _unitOfWork.NotificationRepository.GetByIdAsync(id);
        }

        public async Task<List<Notification>> GetByIdsAsync(List<int> ids)
        {
            var listRes = new List<Notification>();
            foreach (var id in ids)
            {
                var noti = await _unitOfWork.NotificationRepository.GetByIdAsync(id);
                listRes.Add(noti);
            }
            return listRes;
        }

        public async Task<Notification> UpdateAsync(Notification annotation)
        {
            await _unitOfWork.NotificationRepository.UpdateAsync(annotation);
            await _unitOfWork.SaveChangesAsync();
            return annotation;
        }

        public async Task<List<Notification>> GetByUserIdAsync(int id)
        {
            var notifications = await _unitOfWork.NotificationRepository.GetNotificationByUserId(id);
            return notifications;
        }

        #endregion
    }
}
