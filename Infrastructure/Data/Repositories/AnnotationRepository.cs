using ApplicationCore.Entities.AnnotationAggregate;
using ApplicationCore.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class AnnotationRepository : EfRepository<Annotation>, IAnnotationRepository
    {
        public AnnotationRepository(NoisContext context) : base(context)
        {

        }
        /// <summary>
        /// Get Annotation list by id aync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Annotation> EagerGetByIdAsync(int id)
        {
            return await DbSet.Include(c => c.Id == id).FirstOrDefaultAsync();
        }
    }
}