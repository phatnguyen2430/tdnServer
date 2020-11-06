using ApplicationCore.Entities.AnnotationAggregate;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.AnnotationService
{
    public interface IAnnotationService
    {
        Task<Annotation> EagerGetByIdAsync(int id);
        Task<LogicResult<Annotation>> CreateAnnotation(Annotation annotation);
        Task<List<Annotation>> GetAllAsync();
        Task<List<Annotation>> GetByIdsAsync(List<int> ids);
        Task<Annotation> UpdateAsync(Annotation annotation);
        //Task<bool> UpdateLastTimeStatusAsync(Annotation annotation);
        //Task CheckLastTimeStatusAsync();
        //LogicResult<Annotation> Validate(Annotation annotation);
        Task<Annotation> GetByIdAsync(int id);
        Task<Annotation> AddAsync(Annotation entity);
    }
}
