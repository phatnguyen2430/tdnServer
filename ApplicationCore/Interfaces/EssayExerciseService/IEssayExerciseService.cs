using ApplicationCore.Entities.EssayExerciseAggregate;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.EssayExerciseService
{
    public interface IEssayExerciseService
    {
        Task<EssayExercise> EagerGetByIdAsync(int id);
        //Task<bool> UpdateLastTimeStatusAsync(EssayExercise essayExercise);
        //Task CheckLastTimeStatusAsync();
        //LogicResult<EssayExercise> Validate(EssayExercise essayExercise);
        LogicResult<EssayExercise> CreateEssayExercise(EssayExercise essayExercise);
        Task<List<EssayExercise>> GetAllAsync();
        Task<List<EssayExercise>> GetByIdsAsync(List<int> ids);
        Task<EssayExercise> GetByIdAsync(int id);
        Task<EssayExercise> AddAsync(EssayExercise entity);
    }
}
