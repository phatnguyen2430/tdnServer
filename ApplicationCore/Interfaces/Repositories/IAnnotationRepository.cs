using ApplicationCore.Entities.AnnotationAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IAnnotationRepository : IRepositoryAsync<Annotation>
    {
        Task<Annotation> EagerGetByIdAsync(int id);
    }
}
