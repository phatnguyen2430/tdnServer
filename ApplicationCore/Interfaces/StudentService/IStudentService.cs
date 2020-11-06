using ApplicationCore.Entities.StudentAggregate;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.StudentService
{
    public interface IStudentService
    {
        Task<Student> EagerGetByIdAsync(int id);
        //Task<bool> UpdateLastTimeStatusAsync(Student student);
        //Task CheckLastTimeStatusAsync();
        //LogicResult<Student> Validate(Student student);
        Task<LogicResult<Student>> CreateStudent(Student student);
        Task<List<Student>> GetAllAsync();
        Task<List<Student>> GetByIdsAsync(List<int> ids);
        Task<Student> UpdateAsync(Student student);
        Task<Student> AddAsync(Student entity);
        Task<Student> GetByIdAsync(int id);
    }
}
