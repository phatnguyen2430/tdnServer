using ApplicationCore.Entities.MultipleChoicesExerciseAggregate;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.MultipleChoicesExerciseService
{
    public interface IMultipleChoicesAnswerService
    {
        Task<MultipleChoicesAnswer> EagerGetByIdAsync(int id);
        //Task<bool> UpdateLastTimeStatusAsync(MultipleChoicesAnswer multipleChoicesExercise);
        //Task CheckLastTimeStatusAsync();
        //LogicResult<MultipleChoicesAnswer> Validate(MultipleChoicesAnswer multipleChoicesExercise);
        LogicResult<MultipleChoicesAnswer> CreateMultipleChoicesAnswer(MultipleChoicesAnswer multipleChoicesAnswer);
        Task<List<MultipleChoicesAnswer>> GetAllAsync();
        Task<List<MultipleChoicesAnswer>> GetByIdsAsync(List<int> ids);
        Task<MultipleChoicesAnswer> GetByIdAsync(int id);
        Task<MultipleChoicesAnswer> AddAsync(MultipleChoicesAnswer entity);
        Task<bool> CheckExistingBasedOnAnswerIdAndExerciseId(int answerId, int multipleChoicesExerciseId);
        Task<List<MultipleChoicesAnswer>> GetByAnswerIdAsync(int answerId);

    }
}
