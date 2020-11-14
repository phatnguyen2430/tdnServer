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
    public class TestRepository : EfRepository<Test>, ITestRepository
    {
        public TestRepository(NoisContext context) : base(context)
        {

        }

        public async Task<Test> EagerGetByIdAsync(int id)
        {
            return await DbSet.Include(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Test>> GetAllPaging(int pageSize, int pageIndex)
        {
            var query = await DbSet.Where(x => x.Type != TestType.AssTest).ToListAsync();
            var count = query.Count();
            var result = query.OrderByDescending(x => x.CreatedOnUtc).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return result;
        }
    }
}
