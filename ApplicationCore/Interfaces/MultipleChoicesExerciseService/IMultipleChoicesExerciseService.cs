using ApplicationCore.Entities.MultipleChoicesExerciseAggregate;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.MultipleChoicesExerciseService
{
    public interface IMultipleChoicesExerciseService
    {
        Task<MultipleChoicesExercise> EagerGetByIdAsync(int id);
        //Task<bool> UpdateLastTimeStatusAsync(MultipleChoicesExercise multipleChoicesExercise);
        //Task CheckLastTimeStatusAsync();
        //LogicResult<MultipleChoicesExercise> Validate(MultipleChoicesExercise multipleChoicesExercise);
        LogicResult<MultipleChoicesExercise> CreateMultipleChoicesExercise(MultipleChoicesExercise multipleChoicesExercise);
        Task<List<MultipleChoicesExercise>> GetAllAsync();
        Task<List<MultipleChoicesExercise>> GetByIdsAsync(List<int> ids);
        Task<MultipleChoicesExercise> GetByIdAsync(int id);
        Task<MultipleChoicesExercise> AddAsync(MultipleChoicesExercise entity);
        Task<List<MultipleChoicesExercise>> GetByTestIdAsync(int testId);
        Task<MultipleChoicesExercise> UpdateAsync(MultipleChoicesExercise multipleChoicesExercise);
    }
}



//68/16 lu gia 