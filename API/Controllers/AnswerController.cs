using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.AnswerAggregate;
using ApplicationCore.Entities.EssayExerciseAggregate;
using ApplicationCore.Entities.LogAggregate;
using ApplicationCore.Entities.MultipleChoicesExerciseAggregate;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
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

        /// <summary>
        /// Create Answer Contains Answers
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAnswerContainer([FromBody] AnswerContainerModel model)
        {
            var newAnswer = new Answer();
            try
            {
                if (!ModelState.IsValid)
                {
                    ValidModel();
                }
                //check if isExisting ->studentId match TestId
                var existingAnswer = await _service.AnswerService.CheckExistingBasedOnTestId(model.StudentId, model.TestId);
                if (existingAnswer == true)
                {
                    return ErrorResult("You has already done this test.");
                }

                int score = 0;
                //check answer is right
                foreach (var multipleChoicesAnswer in model.MultipleChoicesAnswerModels )
                {
                    //get right result
                    var result = await _service.MultipleChoicesExerciseService.GetByIdAsync(multipleChoicesAnswer.MultipleChoicesExerciseId);
                    //compare result
                    if (multipleChoicesAnswer.Result.ToString() == result.RightResult.ToString())
                    {
                        multipleChoicesAnswer.IsBingo = true;
                        //update result
                        score++;
                    }
                    else
                    {
                        multipleChoicesAnswer.IsBingo = false;
                    }
                }

                //else -> add answer
                newAnswer = new Answer()
                {
                    StudentId = model.StudentId,
                    TestId = model.TestId,
                    Score = model.Score
                };
                await _service.AnswerService.AddAsync(newAnswer);

                //add new multiple answer
                foreach (var multipleChoicesAnswer in model.MultipleChoicesAnswerModels)
                {
                    var newMultitpleChoicesAnswer = new MultipleChoicesAnswer()
                    {
                        AnswerId = multipleChoicesAnswer.AnswerId,
                        IsBingo = multipleChoicesAnswer.IsBingo,
                        Result = multipleChoicesAnswer.Result,
                        MultipleChoicesExerciseId = multipleChoicesAnswer.MultipleChoicesExerciseId
                    };
                    await _service.MultipleChoicesAnswerService.AddAsync(newMultitpleChoicesAnswer);

                    //add log
                    var multipleChoicesLog = new Log()
                    {
                        Status = true,
                        Action = "Add Multiple Choices Answer id : " + newMultitpleChoicesAnswer.Id.ToString() + " complete at " + newMultitpleChoicesAnswer.CreatedOnUtc.ToString()
                    };
                    await _service.LogService.AddAsync(multipleChoicesLog);
                }

                //add new essay answer 
                foreach (var essayAnswer in model.EssayAnswerModels)
                {
                    var newEssayAnswer = new EssayAnswer()
                    {
                        AnswerId = essayAnswer.AnswerId,
                        IsBingo = false,
                        Result = essayAnswer.Result,
                        EssayExerciseId = essayAnswer.EssayExerciseId
                    };
                    await _service.EssayAnswerService.AddAsync(newEssayAnswer);

                    //add log
                    var essayLog = new Log()
                    {
                        Status = true,
                        Action = "Add Essay Answer id : " + newEssayAnswer.Id + " complete at " + newEssayAnswer.CreatedOnUtc
                    };
                    await _service.LogService.AddAsync(essayLog);
                }

                var newLog = new Log()
                {
                    Status = true,
                    Action = "Add Answer id : " + newAnswer.Id + " complete at "+ newAnswer.CreatedOnUtc
                };
                await _service.LogService.AddAsync(newLog);
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


        //fix the answer of student 
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FixEssayAnswer([FromRoute] AnswerEditorModel model)
        {
            //get Essay Answer by Id
            var essayAnswer = await _service.EssayAnswerService.GetByIdAsync(model.EssayAnswerId);

            //add fixed result to answer
            essayAnswer.ResultFixed = model.ResultFixed;

            //update answer 
            await _service.EssayAnswerService.UpdateAsync(essayAnswer);

            return SuccessResult(essayAnswer, "Update Essay Answer successfully.");
        }


        #endregion

        #region get all

        #endregion
    }
}
