using ApplicationCore.Entities.MultipleChoicesExerciseAggregate;
using ApplicationCore.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class MultipleChoicesExerciseRepository : EfRepository<MultipleChoicesExercise>, IMultipleChoicesExerciseRepository
    {
        public MultipleChoicesExerciseRepository(NoisContext context) : base(context)
        {

        }

        public async Task<MultipleChoicesExercise> EagerGetByIdAsync(int id)
        {
            return await DbSet.Include(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<MultipleChoicesExercise>> GetByTestIdAsync(int testId)
        {
            var multipleChoicesExerciseList = await DbSet.Where(x => x.TestId == testId).ToListAsync();
            return multipleChoicesExerciseList;
        }
    }
}
