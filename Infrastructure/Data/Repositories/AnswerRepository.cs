using ApplicationCore.Entities.AnswerAggregate;
using ApplicationCore.Entities.TestAggregate;
using ApplicationCore.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class AnswerRepository : EfRepository<Answer>, IAnswerRepository
    {
        public AnswerRepository(NoisContext context) : base(context)
        {

        }

        /// <summary>
        /// get ansswere by test id and student id
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Answer> EagerGetByIdAsync(int id)
        {
            return await DbSet.Include(c => c.Id == id ).FirstOrDefaultAsync();
        }

        public async Task<bool> CheckIfStudentDidTest(int userId, int testId)
        {
            var list = await DbSet.Where(x => x.UserId == userId).Where(x => x.TestId == testId).ToListAsync();

            var count = list.Count();

            if (list.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<Answer>> GetAnswersByUserId(int userId)
        {
            return await DbSet.Where(x => x.UserId == userId).ToListAsync();
        }
        public async Task<List<Answer>> GetAllPagingByUserId(int pageSize, int pageIndex, int userId)
        {
            var query = await DbSet.Where(x => x.UserId == userId).ToListAsync();
            var count = query.Count();
            var result = query.OrderByDescending(x => x.CreatedOnUtc).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return result;
        }
    }
}
