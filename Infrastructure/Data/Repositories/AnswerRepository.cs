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
        /// <param name="studentId"></param>
        /// <returns></returns>
        public async Task<Answer> EagerGetByIdAsync(int id)
        {
            return await DbSet.Include(c => c.Id == id ).FirstOrDefaultAsync();
        }

        public async Task<bool> CheckIfStudentDidTest(int studentId, int testId)
        {
            var list = await DbSet.Where(x => x.StudentId == studentId).Where(x => x.TestId == testId).ToListAsync();

            var count = list.Count();

            if (list.Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
