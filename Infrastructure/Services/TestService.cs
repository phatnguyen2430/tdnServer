using ApplicationCore.Entities.TestAggregate;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.TestService;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class TestService : ITestService
    {
        #region fields
        IUnitOfWork _unitOfWork;
        #endregion
        public TestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Test> AddAsync(Test entity)
        {
            await _unitOfWork.TestRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public LogicResult<Test> CreateTest(Test student)
        {
            try
            {
                if (student is null)
                {
                    return new LogicResult<Test>
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Annotation can not be empty." }
                    };
                }
                _unitOfWork.TestRepository.AddAsync(student);
                _unitOfWork.SaveChangesAsync();
                return new LogicResult<Test>
                {
                    IsSuccess = true,
                    Data = student
                };
            }
            catch (Exception ex)
            {
                return new LogicResult<Test>
                {
                    IsSuccess = false,
                    Errors = new List<string>
                    {
                        "Unable to save changes.",
                        ex.Message
                    }
                };
            }
        }

        public Task<Test> EagerGetByIdAsync(int id)
        {
            return _unitOfWork.TestRepository.EagerGetByIdAsync(id);
        }
        public async Task<Test> GetByIdAsync(int id)
        {
            return await _unitOfWork.TestRepository.GetByIdAsync(id);
        }
        public async Task<List<Test>> GetAllAsync()
        {
            var tests = await _unitOfWork.TestRepository.GetAllAsync();
            return tests;
        }

        public Task<List<Test>> GetByIdsAsync(List<int> ids)
        {
            var result = _unitOfWork.TestRepository.GetQueryable(filter: x => ids.Contains(x.Id)).ToList();
            return Task.FromResult(result);
        }

        public async Task<List<Test>> GetAllTestsPaging(int pageSize, int pageIndex)
        {
            var result =  await _unitOfWork.TestRepository.GetAllPaging(pageSize,pageIndex);
            return result;
        }

        public async Task<int> CountTotalTest()
        {
            var total = await _unitOfWork.TestRepository.GetAllAsync();
            int result = total.Count();
            return result;
        }
    }
}