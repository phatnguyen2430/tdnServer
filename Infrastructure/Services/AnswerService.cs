using ApplicationCore.Entities.AnswerAggregate;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.AnswerService;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AnswerService : IAnswerService
    {
        #region fields
        IUnitOfWork _unitOfWork;
        #endregion

        #region ctor
        public AnswerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public Task CheckLastTimeStatusAsync()
        //{
        //    throw new NotImplementedException();
        //}

        public LogicResult<Answer> CreateAnswer(Answer answer)
        {
            try
            {
                if (answer is null)
                {
                    return new LogicResult<Answer>
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Annotation can not be empty." }
                    };
                }
                _unitOfWork.AnswerRepository.AddAsync(answer);
                _unitOfWork.SaveChangesAsync();
                return new LogicResult<Answer>
                {
                    IsSuccess = true,
                    Data = answer
                };
            }
            catch (Exception ex)
            {
                return new LogicResult<Answer>
                {
                    IsSuccess = false,
                    Errors = new List<string>
                    {
                        "Unable to save changes.",
                        ex.Message
                    }
                };
            }
        }

        public Task<Answer> EagerGetByIdAsync(int id)
        {
            return _unitOfWork.AnswerRepository.EagerGetByIdAsync(id);
        }

        public async Task<List<Answer>> GetAllAsync()
        {
            var answer = await _unitOfWork.AnswerRepository.GetAllAsync();
            return answer;
        }

        public Task<List<Answer>> GetByIdsAsync(List<int> ids)
        {
            var result = _unitOfWork.AnswerRepository.GetQueryable(filter: x => ids.Contains(x.Id)).ToList();
            return Task.FromResult(result);
        }

        //public Task<bool> UpdateLastTimeStatusAsync(Answer computer)
        //{
        //    throw new NotImplementedException();
        //}

        //public LogicResult<Answer> Validate(Answer computer)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Answer> GetByIdAsync(int id)
        {
            return await _unitOfWork.AnswerRepository.GetByIdAsync(id);
        }
        public async Task<Answer> AddAsync(Answer entity)
        {
            await _unitOfWork.AnswerRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        //check if this answer has already been existing or not
        public async Task<bool> CheckExistingBasedOnTestId(int UserId, int testId)
        {
            return await _unitOfWork.AnswerRepository.CheckIfStudentDidTest(UserId,testId);
        }

        public async Task<List<Answer>> GetAllByUserIdAsync(int UserId)
        {
            return await _unitOfWork.AnswerRepository.GetAnswersByUserId(UserId);
        }

        public async Task<List<Answer>> GetAllAnswersPagingByUserId(int pageSize, int pageIndex,int userId)
        {
            var result = await _unitOfWork.AnswerRepository.GetAllPagingByUserId(pageSize, pageIndex, userId);
            return result;
        }

        public async Task<int> CountTotalAnswerByUserId(int userId)
        {
            var total = await _unitOfWork.AnswerRepository.GetAnswersByUserId(userId);
            int result = total.Count();
            return result;
        }

        #endregion
    }
}
