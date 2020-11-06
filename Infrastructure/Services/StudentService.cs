using ApplicationCore.Entities.StudentAggregate;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.StudentService;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class StudentService : IStudentService
    {
        #region fields
        IUnitOfWork _unitOfWork;
        #endregion

        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<LogicResult<Student>> CreateStudent(Student student)
        {
            try
            {
                if (student is null)
                {
                    return new LogicResult<Student>
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Annotation can not be empty." }
                    };
                }
                await _unitOfWork.StudentRepository.AddAsync(student);
                await _unitOfWork.SaveChangesAsync();
                return new LogicResult<Student>
                {
                    IsSuccess = true,
                    Data = student
                };
            }
            catch (Exception ex)
            {
                return new LogicResult<Student>
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

        public async Task<Student> AddAsync(Student entity)
        {
            await _unitOfWork.StudentRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }


        public async Task<Student> EagerGetByIdAsync(int id)
        {
            return await _unitOfWork.StudentRepository.EagerGetByIdAsync(id);
        }
        public async Task<Student> GetByIdAsync(int id)
        {
            return await _unitOfWork.StudentRepository.GetByIdAsync(id);
        }

        public async Task<List<Student>> GetAllAsync()
        {
            var students = await _unitOfWork.StudentRepository.GetAllAsync();
            return students;
        }

        public async Task<List<Student>> GetByIdsAsync(List<int> ids)
        {
            var result =  _unitOfWork.StudentRepository.GetQueryable(filter: x => ids.Contains(x.Id)).ToList();
            return await Task.FromResult(result);
        }
        public async Task<Student> UpdateAsync(Student student)
        {
            await _unitOfWork.StudentRepository.UpdateAsync(student);
            await _unitOfWork.SaveChangesAsync();
            return student;
        }
    }
}