using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.MultipleChoicesExerciseAggregate;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.MultipleChoicesAnswer;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MultipleChoicesAnswerController : BaseApiController
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IService _service;
        private readonly IMapper _mapper;
        #endregion

        #region ctor
        public MultipleChoicesAnswerController(IUnitOfWork unitOfWork,
            IService service,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _service = service;
            _mapper = mapper;
        }
        #endregion
        #region check if existing, add, update, delete
        //get MultipleChoicesAnswer in test by test id


        //create MultipleChoicesAnswer
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
        public async Task<IActionResult> CreateMultipleChoicesAnswer([FromBody] MultipleChoicesAnswerModel model)
        {
            var newMultipleChoicesAnswer = new MultipleChoicesAnswer();
            try
            {
                if (!ModelState.IsValid)
                {
                    ValidModel();
                }

                //check if this question has been done yet
                var existingMultipleChoicesAnswer = await _service.MultipleChoicesAnswerService.CheckExistingBasedOnAnswerIdAndExerciseId(model.AnswerId, model.MultipleChoicesExerciseId);
                if (existingMultipleChoicesAnswer == true)
                {
                    return ErrorResult("You has already done this question.");
                }

                //check if multiple choices answer is existing
                var multipleChoicesExercise = await _service.MultipleChoicesExerciseService.GetByIdAsync(model.MultipleChoicesExerciseId);
                if (multipleChoicesExercise == null)
                {
                    return ErrorResult($"The Exercise with Id :{model.MultipleChoicesExerciseId} does not exist.");
                }

                newMultipleChoicesAnswer = new MultipleChoicesAnswer()
                {
                    AnswerId = model.AnswerId,
                    MultipleChoicesExerciseId = model.MultipleChoicesExerciseId,
                    Result = model.Result
                };
                await _service.MultipleChoicesAnswerService.AddAsync(newMultipleChoicesAnswer);
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
            return SuccessResult(newMultipleChoicesAnswer, "Created Multiple Choices Answer successfully.");
        }

        //get MultipleChoicesAnswer by id
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMultipleChoicesAnswerById([FromRoute] int id)
        {
            var multipleChoicesAnswer = await _service.MultipleChoicesAnswerService.GetByIdAsync(id);
            if (multipleChoicesAnswer == null)
            {
                return ErrorResult($"Can not found Multiple Choices Answer with Id: {id}");
            }
            var multipleChoicesAnswerRes = new MultipleChoicesAnswerModel
            {
                AnswerId = multipleChoicesAnswer.AnswerId,
                Id = multipleChoicesAnswer.Id,
                MultipleChoicesExerciseId = multipleChoicesAnswer.MultipleChoicesExerciseId,
                Result = multipleChoicesAnswer.Result
            };
            return SuccessResult(multipleChoicesAnswerRes, "Get Multiple Choices Answer successfully.");
        }
        #endregion
    }
}
