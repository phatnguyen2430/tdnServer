using ApplicationCore.Entities.AnswerAggregate;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.AnswerService
{
    public interface IAnswerService
    {
        Task<Answer> EagerGetByIdAsync(int id);
        //Task<bool> UpdateLastTimeStatusAsync(Answer computer);
        //Task CheckLastTimeStatusAsync();
        //LogicResult<Answer> Validate(Answer computer);
        LogicResult<Answer> CreateAnswer(Answer answer);
        Task<List<Answer>> GetAllAsync();
        Task<List<Answer>> GetByIdsAsync(List<int> ids);
        Task<Answer> GetByIdAsync(int id);
        Task<Answer> AddAsync(Answer entity);
        Task<bool> CheckExistingBasedOnTestId(int testId, int userId);
        Task<List<Answer>> GetAllByUserIdAsync(int studentId);
       Task<List<Answer>> GetAllAnswersPagingByUserId(int pageSize, int pageIndex, int userId);
        Task<int> CountTotalAnswerByUserId(int userId);
    }
}
