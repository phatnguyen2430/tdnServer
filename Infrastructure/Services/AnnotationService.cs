using ApplicationCore.Entities.AnnotationAggregate;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.AnnotationService;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AnnotationService : IAnnotationService
    {
        #region fields
        IUnitOfWork _unitOfWork;
        #endregion

        #region ctor
        public AnnotationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<LogicResult<Annotation>> CreateAnnotation(Annotation annotation)
        {
            try
            {
                if (annotation is null)
                {
                    return new LogicResult<Annotation>
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Annotation can not be empty." }
                    };
                }
                await _unitOfWork.AnnotationRepository.AddAsync(annotation);
                await _unitOfWork.SaveChangesAsync();
                return new LogicResult<Annotation>
                {
                    IsSuccess = true,
                    Data = annotation
                };
            }
            catch (Exception ex)
            {
                return new LogicResult<Annotation>
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

        public async Task<Annotation> EagerGetByIdAsync(int id)
        {
            return await _unitOfWork.AnnotationRepository.EagerGetByIdAsync(id);
        }

        public async Task<List<Annotation>> GetAllAsync()
        {
            var annotations = await _unitOfWork.AnnotationRepository.GetAllAsync();
            return annotations;
        }

        public async Task<List<Annotation>> GetByIdsAsync(List<int> ids)
        {
            var result =  _unitOfWork.AnnotationRepository.GetQueryable(filter: x => ids.Contains(x.Id)).ToList();
            return await Task.FromResult(result);
        }

        public async Task<Annotation> UpdateAsync(Annotation annotation)
        {
            await _unitOfWork.AnnotationRepository.UpdateAsync(annotation);
            await _unitOfWork.SaveChangesAsync();
            return annotation;
        }

        //public Task<bool> UpdateAsync(Annotation annotation)
        //{
        //    throw new NotImplementedException();
        //}

        //public LogicResult<Annotation> Validate(Annotation annotation)
        //{
        //    throw new NotImplementedException();
        //}
        //public Task CheckLastTimeStatusAsync()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Annotation> GetByIdAsync(int id)
        {
            return await _unitOfWork.AnnotationRepository.GetByIdAsync(id);
        }
        public async Task<Annotation> AddAsync(Annotation entity)
        {
            await _unitOfWork.AnnotationRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        #endregion

    }
}
