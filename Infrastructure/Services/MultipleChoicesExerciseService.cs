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
    public class MultipleChoicesExerciseService : IMultipleChoicesExerciseService
    {
        #region fields
        IUnitOfWork _unitOfWork;
        #endregion

        public MultipleChoicesExerciseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public LogicResult<MultipleChoicesExercise> CreateMultipleChoicesExercise(MultipleChoicesExercise multipleChoicesExercise)
        {
            try
            {
                if (multipleChoicesExercise is null)
                {
                    return new LogicResult<MultipleChoicesExercise>
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Annotation can not be empty." }
                    };
                }
                _unitOfWork.MultipleChoicesExerciseRepository.AddAsync(multipleChoicesExercise);
                _unitOfWork.SaveChangesAsync();
                return new LogicResult<MultipleChoicesExercise>
                {
                    IsSuccess = true,
                    Data = multipleChoicesExercise
                };
            }
            catch (Exception ex)
            {
                return new LogicResult<MultipleChoicesExercise>
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
        public async Task<MultipleChoicesExercise> AddAsync(MultipleChoicesExercise entity)
        {
            await _unitOfWork.MultipleChoicesExerciseRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public Task<MultipleChoicesExercise> EagerGetByIdAsync(int id)
        {
            return _unitOfWork.MultipleChoicesExerciseRepository.EagerGetByIdAsync(id);
        }
        public async Task<MultipleChoicesExercise> GetByIdAsync(int id)
        {
            return await _unitOfWork.MultipleChoicesExerciseRepository.GetByIdAsync(id);
        }
        public async Task<List<MultipleChoicesExercise>> GetAllAsync()
        {
            var multipleChoicesExercises = await _unitOfWork.MultipleChoicesExerciseRepository.GetAllAsync();
            return multipleChoicesExercises;
        }

        public async Task<List<MultipleChoicesExercise>> GetByTestIdAsync(int testId)
        {
            return await _unitOfWork.MultipleChoicesExerciseRepository.GetByTestIdAsync(testId);
        }


        public Task<List<MultipleChoicesExercise>> GetByIdsAsync(List<int> ids)
        {
            var result = _unitOfWork.MultipleChoicesExerciseRepository.GetQueryable(filter: x => ids.Contains(x.Id)).ToList();
            return Task.FromResult(result);
        }

        public async Task<MultipleChoicesExercise> UpdateAsync(MultipleChoicesExercise multipleChoicesExercise)
        {
            await _unitOfWork.MultipleChoicesExerciseRepository.UpdateAsync(multipleChoicesExercise);
            await _unitOfWork.SaveChangesAsync();
            return multipleChoicesExercise;
        }


    }
}
