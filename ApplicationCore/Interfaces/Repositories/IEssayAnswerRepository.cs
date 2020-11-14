using ApplicationCore.Entities.EssayExerciseAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IEssayAnswerRepository : IRepositoryAsync<EssayAnswer>
    {
        Task<EssayAnswer> EagerGetByIdAsync(int id);
        Task<bool> CheckAnswerIsExisting(int answerId, int essayExcerciseId);
        Task<List<EssayAnswer>> GetByAnswerIdAsync(int answerId);
    }
}
