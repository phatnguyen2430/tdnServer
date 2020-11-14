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
    public class EssayExerciseService : IEssayExerciseService
    {
        #region fields
        IUnitOfWork _unitOfWork;
        #endregion

        public EssayExerciseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public LogicResult<EssayExercise> CreateEssayExercise(EssayExercise essayExercise)
        {
            try
            {
                if (essayExercise is null)
                {
                    return new LogicResult<EssayExercise>
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Annotation can not be empty." }
                    };
                }
                _unitOfWork.EssayExerciseRepository.AddAsync(essayExercise);
                _unitOfWork.SaveChangesAsync();
                return new LogicResult<EssayExercise>
                {
                    IsSuccess = true,
                    Data = essayExercise
                };
            }
            catch (Exception ex)
            {
                return new LogicResult<EssayExercise>
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

        public Task<EssayExercise> EagerGetByIdAsync(int id)
        {
            return _unitOfWork.EssayExerciseRepository.EagerGetByIdAsync(id);
        }

        public async Task<List<EssayExercise>> GetAllAsync()
        {
            var essayExercises = await _unitOfWork.EssayExerciseRepository.GetAllAsync();
            return essayExercises;
        }

        public Task<List<EssayExercise>> GetByIdsAsync(List<int> ids)
        {
            var result = _unitOfWork.EssayExerciseRepository.GetQueryable(filter: x => ids.Contains(x.Id)).ToList();
            return Task.FromResult(result);
        }
        public async Task<EssayExercise> GetByIdAsync(int id)
        {
            return await _unitOfWork.EssayExerciseRepository.GetByIdAsync(id);
        }
        public async Task<EssayExercise> AddAsync(EssayExercise entity)
        {
            await _unitOfWork.EssayExerciseRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task<List<EssayExercise>> GetByTestIdAsync(int testId)
        {
            return await _unitOfWork.EssayExerciseRepository.GetByTestIdAsync(testId);
        }

    }
}
