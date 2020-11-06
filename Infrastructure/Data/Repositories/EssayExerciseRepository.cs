using ApplicationCore.Entities.EssayExerciseAggregate;
using ApplicationCore.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class EssayExerciseRepository : EfRepository<EssayExercise>, IEssayExerciseRepository
    {
        public EssayExerciseRepository(NoisContext context) : base(context)
        {

        }

        /// <summary>
        /// get essay exercise by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EssayExercise> EagerGetByIdAsync(int id)
        {
            return await DbSet.Include(c => c.Id == id).FirstOrDefaultAsync();
        }
    }
}
