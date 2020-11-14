using ApplicationCore.Entities.EssayExerciseAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IEssayExerciseRepository : IRepositoryAsync<EssayExercise>
    {
        Task<EssayExercise> EagerGetByIdAsync(int id);
        Task<List<EssayExercise>> GetByTestIdAsync(int testId);
    }
}
