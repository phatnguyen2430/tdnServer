using ApplicationCore.Entities.TestAggregate;
using ApplicationCore.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    }
}
