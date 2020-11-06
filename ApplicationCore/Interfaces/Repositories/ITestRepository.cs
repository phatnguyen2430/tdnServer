using ApplicationCore.Entities.TestAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface ITestRepository : IRepositoryAsync<Test>
    {
        Task<Test> EagerGetByIdAsync(int id);
    }
}
