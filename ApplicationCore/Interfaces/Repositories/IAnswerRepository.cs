﻿using ApplicationCore.Entities.AnswerAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IAnswerRepository : IRepositoryAsync<Answer>
    {
        Task<Answer> EagerGetByIdAsync(int id);
        Task<bool> CheckIfStudentDidTest(int userId, int testId);
        Task<List<Answer>> GetAnswersByUserId(int userId);
        Task<List<Answer>> GetAllPagingByUserId(int pageSize, int pageIndex, int userId);
    }
}
