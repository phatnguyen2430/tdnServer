using ApplicationCore.Entities.MultipleChoicesExerciseAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IMultipleChoicesExerciseRepository  : IRepositoryAsync<MultipleChoicesExercise>
    {
        Task<MultipleChoicesExercise> EagerGetByIdAsync(int id);
    }
}
