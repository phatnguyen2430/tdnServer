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

        public async Task<List<EssayAnswer>> GetByAnswerIdAsync(int answerId)
        {
            return await DbSet.Where(x => x.AnswerId == answerId).ToListAsync();
        }
        public async Task<List<int>> GetUnfixedTestIdsAsync(int pageIndex)
        {
            var listEssayExercises = await DbSet.Where(x => x.Result == null).ToListAsync();
            var listIds = new List<int>();
            foreach (var essayExercise in listEssayExercises)
            {
                listIds.Add(essayExercise.AnswerId);
            }

            //make group by
            var resList = listIds.GroupBy(x => x).ToList();

            //add to list
            var testIds = new List<int>();
            foreach (var item in resList)
            {
                testIds.Add(item.FirstOrDefault());
            }

            //add paging 
            var result = testIds.OrderByDescending(x => x).Skip((pageIndex - 1) * 25).Take(25).ToList();
            return result;
        }
    }
}
