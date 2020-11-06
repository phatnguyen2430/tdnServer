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
    public class MultipleChoicesAnswerRepository: EfRepository<MultipleChoicesAnswer>, IMultipleChoicesAnswerRepository
    {
        public MultipleChoicesAnswerRepository(NoisContext context) : base(context)
        {

        }

        public async Task<MultipleChoicesAnswer> EagerGetByIdAsync(int id)
        {
            return await DbSet.Include(c => c.Id == id).FirstOrDefaultAsync();
        }
        public async Task<bool> CheckAnswerIsExisting(int answerId, int multipleChoicesExerciseId)
        {
            var list = await DbSet.Where(x => x.AnswerId == answerId).Where(x => x.MultipleChoicesExerciseId == multipleChoicesExerciseId).ToListAsync();

            var count = list.Count();

            if (list.Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
