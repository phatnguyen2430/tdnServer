using ApplicationCore.Entities.StudentAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IStudentRepository : IRepositoryAsync<Student>
    {
        Task<Student> EagerGetByIdAsync(int id);
    }
}
