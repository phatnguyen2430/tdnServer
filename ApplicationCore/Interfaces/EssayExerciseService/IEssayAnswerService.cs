using ApplicationCore.Entities.EssayExerciseAggregate;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.EssayExerciseService
{
    public interface IEssayAnswerService
    {
        Task<EssayAnswer> EagerGetByIdAsync(int id);
        //Task<bool> UpdateLastTimeStatusAsync(EssayAnswer essayExercise);
        //Task CheckLastTimeStatusAsync();
        //LogicResult<EssayAnswer> Validate(EssayAnswer essayExercise);
        LogicResult<EssayAnswer> CreateEssayAnswer(EssayAnswer essayExercise);
        Task<List<EssayAnswer>> GetAllAsync();
        Task<List<EssayAnswer>> GetByIdsAsync(List<int> ids);
        Task<EssayAnswer> GetByIdAsync(int id);
        Task<EssayAnswer> AddAsync(EssayAnswer entity);
        Task<bool> CheckExistingBasedOnAnswerIdAndExerciseId(int answerId, int essayExerciseId);
    }
}
