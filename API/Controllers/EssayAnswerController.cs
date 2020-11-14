using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.EssayExerciseAggregate;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.EssayAnswer;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EssayAnswerController : BaseApiController
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IService _service;
        private readonly IMapper _mapper;
        #endregion

        #region ctor
        public EssayAnswerController(IUnitOfWork unitOfWork,
            IService service,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _service = service;
            _mapper = mapper;
        }
        #endregion
        #region check if existing, add, update, delete
        //get essay answers in test by test id


        //create Essay Answer
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
        public async Task<IActionResult> CreateEssayAnswer([FromBody] EssayAnswerRequestModel model)
        {
            var essayAnswer = new EssayAnswer();
            try
            {
                if (!ModelState.IsValid)
                {
                    ValidModel();
                }

                //check if this question has been done yet
                var existingEssayAnswer = await _service.EssayAnswerService.CheckExistingBasedOnAnswerIdAndExerciseId(model.AnswerId, model.EssayExerciseId);
                if (existingEssayAnswer == true)
                {
                    return ErrorResult("You has already done this question.");
                }

                //check if multiple choices answer is existing
                var essayExercise = await _service.EssayExerciseService.GetByIdAsync(model.EssayExerciseId);
                if (essayExercise == null)
                {
                    return ErrorResult($"The Exercise with Id :{model.EssayExerciseId} does not exist.");
                }

                essayAnswer = new EssayAnswer()
                {
                    AnswerId = model.AnswerId,
                    EssayExerciseId = model.EssayExerciseId,
                    Result = model.Result
                };
                await _service.EssayAnswerService.AddAsync(essayAnswer);
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
            return SuccessResult(essayAnswer, "Created Essay Answer successfully.");
        }

        //get EssayAnswer by id
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEssayAnswerById([FromRoute] int id)
        {
            var essayAnswer = await _service.EssayAnswerService.GetByIdAsync(id);
            if (essayAnswer == null)
            {
                return ErrorResult($"Can not found Essay Answer with Id: {id}");
            }
            var essayAnswerRes = new EssayAnswerResponseModel
            {
                Id = essayAnswer.Id,
                AnswerId = essayAnswer.AnswerId,
                EssayExerciseId = essayAnswer.EssayExerciseId,
                Result = essayAnswer.Result
            };
            return SuccessResult(essayAnswerRes, "Get Essay Answer successfully.");
        }


        #endregion

        #region get all

        #endregion
    }
}
