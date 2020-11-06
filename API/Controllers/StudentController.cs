using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.StudentAggregate;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Student;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : BaseApiController
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IService _service;
        private readonly IMapper _mapper;
        #endregion

        #region ctor
        public StudentController(IUnitOfWork unitOfWork,
            IService service,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _service = service;
            _mapper = mapper;
        }
        #endregion
        #region check if existing, add, update, delete
        //create Student
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
        public async Task<IActionResult> CreateStudent([FromBody] StudentModel model)
        {
            var newStudent = new Student();
            try
            {
                if (!ModelState.IsValid)
                {
                    ValidModel();
                }
                newStudent = new Student()
                {
                    Address = model.Address,
                    Age = model.Age,
                    Email = model.Email,
                    Name = model.Name,
                    Password = model.Password,
                    PhoneNumber = model.PhoneNumber
                };
                await _service.StudentService.AddAsync(newStudent);
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
            return SuccessResult(newStudent, "Created Student successfully.");
        }

        //get student by id
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStudentById([FromRoute] int id)
        {
            var student = await _service.StudentService.GetByIdAsync(id);
            if (student == null)
            {
                return ErrorResult($"Can not found student with Id: {id}");
            }
            var stuRes = new StudentModel
            {
                Address = student.Address,
                Age = student.Age,
                Email = student.Email,
                Name = student.Name,
                Password = student.Password,
                PhoneNumber = student.PhoneNumber
            };
            return SuccessResult(student, "Get student successfully.");
        }

        #endregion

        #region get all

        #endregion
    }
}
