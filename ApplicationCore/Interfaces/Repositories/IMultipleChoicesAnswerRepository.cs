using ApplicationCore.Entities.MultipleChoicesExerciseAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IMultipleChoicesAnswerRepository : IRepositoryAsync<MultipleChoicesAnswer>
    {
        Task<MultipleChoicesAnswer> EagerGetByIdAsync(int id);
        Task<bool> CheckAnswerIsExisting(int answerId, int multipleChoicesExerciseId);
    }
}
