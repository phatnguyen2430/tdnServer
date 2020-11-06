using ApplicationCore.Entities.EssayExerciseAggregate;
using ApplicationCore.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class EssayAnswerRepository : EfRepository<EssayAnswer>, IEssayAnswerRepository
    {
        public EssayAnswerRepository(NoisContext context) : base(context)
        {

        }

        /// <summary>
        /// get essay answer by answer id and essay exercise id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EssayAnswer> EagerGetByIdAsync(int id)
        {
            return await DbSet.Include(c => c.Id == id).FirstOrDefaultAsync();
            //return await DbSet.Include(c => c.AnswerId == answerId && c.EssayExerciseId == essayExerciseId).FirstOrDefaultAsync();
        }

        public async Task<bool> CheckAnswerIsExisting(int answerId, int essayExcerciseId)
        {
            var list = await DbSet.Where(x => x.AnswerId == answerId).Where(x => x.EssayExerciseId == essayExcerciseId).ToListAsync();

            var count = list.Count();

            if (list.Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
