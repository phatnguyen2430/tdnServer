using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.AnswerAggregate;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Answer;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : BaseApiController
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IService _service;
        private readonly IMapper _mapper;
        #endregion

        #region ctor
        public AnswerController(IUnitOfWork unitOfWork,
            IService service,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _service = service;
            _mapper = mapper;
        }
        #endregion
        #region check if existing, add, update, delete
        //get answer total Score


        //create Answer
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
        public async Task<IActionResult> CreateAnswer([FromBody] AnswerModel model)
        {
            var newAnswer = new Answer();
            try
            {
                if (!ModelState.IsValid)
                {
                    ValidModel();
                }

                //check if isExisting ->studentId match TestId
                var existingAnswer = await _service.AnswerService.CheckExistingBasedOnTestId(model.StudentId,model.TestId);
                if (existingAnswer == true)
                {
                    return ErrorResult("You has already done this test.");
                }

                //else -> add answer
                newAnswer = new Answer()
                {
                    StudentId = model.StudentId,
                    TestId = model.TestId,
                    Score = model.Score
                };
                await _service.AnswerService.AddAsync(newAnswer);
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
            return SuccessResult(newAnswer, "Created Answer successfully.");
        }

        //get Answer by id
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAnswerById([FromRoute] int id)
        {
            var answer = await _service.AnswerService.GetByIdAsync(id);
            if (answer == null)
            {
                return ErrorResult($"Can not found Answer with Id: {id}");
            }
            var answerRes = new AnswerModel
            {
                Id = answer.Id,
                Score = answer.Score,
                StudentId = answer.StudentId,
                TestId = answer.TestId
            };
            return SuccessResult(answerRes, "Get Answer successfully.");
        }


        //get all answer based on test id


        #endregion

        #region get all

        #endregion
    }
}
