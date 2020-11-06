using ApplicationCore.Entities.MultipleChoicesExerciseAggregate;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.MultipleChoicesExerciseService;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class MultipleChoicesAnswerService : IMultipleChoicesAnswerService
    {
        #region fields
        IUnitOfWork _unitOfWork;
        #endregion

        public MultipleChoicesAnswerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<MultipleChoicesAnswer> AddAsync(MultipleChoicesAnswer entity)
        {
            await _unitOfWork.MultipleChoicesAnswerRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public LogicResult<MultipleChoicesAnswer> CreateMultipleChoicesAnswer(MultipleChoicesAnswer multipleChoicesAnswer)
        {
            try
            {
                if (multipleChoicesAnswer is null)
                {
                    return new LogicResult<MultipleChoicesAnswer>
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Annotation can not be empty." }
                    };
                }
                _unitOfWork.MultipleChoicesAnswerRepository.AddAsync(multipleChoicesAnswer);
                _unitOfWork.SaveChangesAsync();
                return new LogicResult<MultipleChoicesAnswer>
                {
                    IsSuccess = true,
                    Data = multipleChoicesAnswer
                };
            }
            catch (Exception ex)
            {
                return new LogicResult<MultipleChoicesAnswer>
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

        public Task<MultipleChoicesAnswer> EagerGetByIdAsync(int id)
        {
            return _unitOfWork.MultipleChoicesAnswerRepository.EagerGetByIdAsync(id);
        }

        public async Task<List<MultipleChoicesAnswer>> GetAllAsync()
        {
            var multipleChoicesAnswers = await _unitOfWork.MultipleChoicesAnswerRepository.GetAllAsync();
            return multipleChoicesAnswers;
        }

        public Task<List<MultipleChoicesAnswer>> GetByIdsAsync(List<int> ids)
        {
            var result = _unitOfWork.MultipleChoicesAnswerRepository.GetQueryable(filter: x => ids.Contains(x.Id)).ToList();
            return Task.FromResult(result);
        }
        public async Task<MultipleChoicesAnswer> GetByIdAsync(int id)
        {
            return await _unitOfWork.MultipleChoicesAnswerRepository.GetByIdAsync(id);
        }
        public async Task<bool> CheckExistingBasedOnAnswerIdAndExerciseId(int answerId, int multipleChoicesExerciseId)
        {
            return await _unitOfWork.MultipleChoicesAnswerRepository.CheckAnswerIsExisting(answerId, multipleChoicesExerciseId);
        }
    }
}