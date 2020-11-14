using ApplicationCore.Entities.TestAggregate;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.TestService
{
    public interface ITestService
    {
        Task<Test> EagerGetByIdAsync(int id);
        //Task<bool> UpdateLastTimeStatusAsync(Test student);
        //Task CheckLastTimeStatusAsync();
        //LogicResult<Test> Validate(Test student);
        LogicResult<Test> CreateTest(Test student);
        Task<List<Test>> GetAllAsync();
        Task<List<Test>> GetByIdsAsync(List<int> ids);
        Task<Test> GetByIdAsync(int id);
        Task<Test> AddAsync(Test entity);
        Task<List<Test>> GetAllTestsPaging(int pageSize, int pageIndex);
        Task<int> CountTotalTest();
    }
}
