using ApplicationCore.Entities.EssayExerciseAggregate;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.EssayExerciseService;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EssayAnswerService : IEssayAnswerService
    {
        #region fields
        IUnitOfWork _unitOfWork;
        #endregion

        public EssayAnswerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public Task CheckLastTimeStatusAsync()
        //{
        //    throw new NotImplementedException();
        //}


        public LogicResult<EssayAnswer> CreateEssayAnswer(EssayAnswer essayExercise)
        {
            try
            {
                if (essayExercise is null)
                {
                    return new LogicResult<EssayAnswer>
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Annotation can not be empty." }
                    };
                }
                _unitOfWork.EssayAnswerRepository.AddAsync(essayExercise);
                _unitOfWork.SaveChangesAsync();
                return new LogicResult<EssayAnswer>
                {
                    IsSuccess = true,
                    Data = essayExercise
                };
            }
            catch (Exception ex)
            {
                return new LogicResult<EssayAnswer>
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

        public Task<EssayAnswer> EagerGetByIdAsync(int id)
        {
            return _unitOfWork.EssayAnswerRepository.EagerGetByIdAsync(id);
        }

        public async Task<List<EssayAnswer>> GetAllAsync()
        {
            var essayAnswers = await _unitOfWork.EssayAnswerRepository.GetAllAsync();
            return essayAnswers;
        }

        public Task<List<EssayAnswer>> GetByIdsAsync(List<int> ids)
        {
            var result = _unitOfWork.EssayAnswerRepository.GetQueryable(filter: x => ids.Contains(x.Id)).ToList();
            return Task.FromResult(result);
        }
        public async Task<EssayAnswer> GetByIdAsync(int id)
        {
            return await _unitOfWork.EssayAnswerRepository.GetByIdAsync(id);
        }
        public async Task<EssayAnswer> AddAsync(EssayAnswer entity)
        {
            await _unitOfWork.EssayAnswerRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        //public Task<bool> UpdateLastTimeStatusAsync(EssayAnswer essayExercise)
        //{
        //    throw new NotImplementedException();
        //}

        //public LogicResult<EssayAnswer> Validate(EssayAnswer essayExercise)
        //{
        //    throw new NotImplementedException();
        //}
        public async Task<bool> CheckExistingBasedOnAnswerIdAndExerciseId(int answerId, int essayExerciseId)
        {
            return await _unitOfWork.EssayAnswerRepository.CheckAnswerIsExisting(answerId, essayExerciseId);
        }
    }
}