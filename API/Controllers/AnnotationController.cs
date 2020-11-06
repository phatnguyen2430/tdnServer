using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.AnnotationAggregate;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Annotation;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnotationController : BaseApiController
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IService _service;
        private readonly IMapper _mapper;
        #endregion
        #region ctor
        public AnnotationController(IUnitOfWork unitOfWork,
            IService service,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _service = service;
            _mapper = mapper;
        }
        #endregion

        #region check if existing, add, update, delete
        //get all annotation based on user id



        //create Annotation
        /// <summary>
        /// <response code=200>Request completed Successfully</response>
        /// <response code=400>Bad Request</response>
        /// <response code=401>Don't have permission</response>
        /// <response code=500>Internal server error</response>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAnnotation([FromBody] AnnotationModel model)
        {
            var newAnnotation = new Annotation();
            try
            {
                if (!ModelState.IsValid)
                {
                    ValidModel();
                }
                newAnnotation = new Annotation()
                {
                    IsChecked = model.IsChecked,
                    StudentId = model.StudentId,
                    Type = model.Type,
                    Content = model.Content
                };
                await _service.AnnotationService.AddAsync(newAnnotation);
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
            return SuccessResult(newAnnotation, "Created Annotation successfully.");
        }

        //get Annotation by id
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAnnotationById([FromRoute] int id)
        {
            var annotation = await _service.AnnotationService.GetByIdAsync(id);
            if (annotation == null)
            {
                return ErrorResult($"Can not found annotation with Id: {id}");
            }
            var annotationRes = new AnnotationModel
            {
                Id = annotation.Id,
                Content = annotation.Content,
                IsChecked = annotation.IsChecked,
                StudentId = annotation.StudentId,
                Type  = annotation.Type,
                CreatedOnUtc = annotation.CreatedOnUtc,
                UpdatedOnUtc = annotation.UpdatedOnUtc
            };
            return SuccessResult(annotationRes, "Get annotation successfully.");
        }

        #endregion

        #region get all

        #endregion
    }
}
