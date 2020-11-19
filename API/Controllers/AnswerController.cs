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
using WebAPI.Models.EssayAnswer;
using WebAPI.Models.MultipleChoicesAnswer;

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
        public async Task<IActionResult> CreateAnswer([FromBody] AnswerRequestModel model)
        {
            try
            {
                var newAnswer = new Answer();
                if (!ModelState.IsValid)
                {
                    ValidModel();
                }

                //check if isExisting ->UserId match TestId
                var existingAnswer = await _service.AnswerService.CheckExistingBasedOnTestId(model.UserId,model.TestId);
                if (existingAnswer == true)
                {
                    return ErrorResult("You has already done this test.");
                }

                //else -> add answer
                newAnswer = new Answer()
                {
                    UserId = model.UserId,
                    TestId = model.TestId,
                    Score = model.Score
                };
                await _service.AnswerService.AddAsync(newAnswer);

                var responseModel = new AnswerResponseModel()
                {
                    Id = newAnswer.Id,
                    Score = newAnswer.Score,
                    TestId = newAnswer.TestId,
                    UserId = newAnswer.UserId
                };


                return SuccessResult(responseModel, "Created Answer successfully.");
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
        }

        /// <summary>
        /// Create Answer Contains Answers
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("create/contains")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAnswerContainer([FromBody] AnswerContainerRequestModel model)
        {
            try
            {
                var newAnswer = new Answer();
                if (!ModelState.IsValid)
                {
                    ValidModel();
                }
                //check if isExisting ->UserId match TestId
                var existingAnswer = await _service.AnswerService.CheckExistingBasedOnTestId(model.UserId, model.TestId);
                if (existingAnswer == true)
                {
                    return ErrorResult("You has already done this test.");
                }

                int score = 0;
                //check answer is right
                foreach (var multipleChoicesAnswer in model.MultipleChoicesAnswerRequestModels )
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
                    UserId = model.UserId,
                    TestId = model.TestId,
                    Score = model.Score
                };
                await _service.AnswerService.AddAsync(newAnswer);

                //create answer response model
                var answerResponseModel = new AnswerContainerResponseModel()
                {
                    Id = newAnswer.Id,
                    Score = newAnswer.Score,
                    TestId = newAnswer.TestId,
                    UserId = newAnswer.UserId
                };
                //create response MultipleChoices Answer 
                var multipleChoicesAnswerResponses = new List<MultipleChoicesAnswerResponseModel>();
                //creatae response Essay Answer
                var essayAnswerResponses = new List<EssayAnswerResponseModel>();

                //add new multiple answer
                foreach (var multipleChoicesAnswer in model.MultipleChoicesAnswerRequestModels)
                {
                    var newMultitpleChoicesAnswer = new MultipleChoicesAnswer()
                    {
                        AnswerId = newAnswer.Id,
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

                    //add to list response
                    var newMultipleChoiceResponse = new MultipleChoicesAnswerResponseModel()
                    {
                        AnswerId = newMultitpleChoicesAnswer.AnswerId,
                        Id = newMultitpleChoicesAnswer.Id,
                        IsBingo = newMultitpleChoicesAnswer.IsBingo,
                        MultipleChoicesExerciseId = newMultitpleChoicesAnswer.MultipleChoicesExerciseId,
                        Result = newMultitpleChoicesAnswer.Result
                    };
                    multipleChoicesAnswerResponses.Add(newMultipleChoiceResponse);
                }

                //add new essay answer 
                foreach (var essayAnswer in model.EssayAnswerRequestModels)
                {
                    var newEssayAnswer = new EssayAnswer()
                    {
                        AnswerId = newAnswer.Id,
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

                    //add to list response
                    var newEssayResponse = new EssayAnswerResponseModel()
                    {
                        AnswerId = newEssayAnswer.AnswerId,
                        EssayExerciseId = newEssayAnswer.EssayExerciseId,
                        Id = newEssayAnswer.Id,
                        IsBingo = newEssayAnswer.IsBingo,
                        Result = newEssayAnswer.Result
                    };
                    essayAnswerResponses.Add(newEssayResponse);
                }

                answerResponseModel.MultipleChoicesAnswerResponseModels = multipleChoicesAnswerResponses;
                answerResponseModel.EssayAnswerResponseModels = essayAnswerResponses;

                var newLog = new Log()
                {
                    Status = true,
                    Action = "Add Answer id : " + newAnswer.Id + " complete at "+ newAnswer.CreatedOnUtc
                };
                await _service.LogService.AddAsync(newLog);

                return SuccessResult(answerResponseModel, "Created Answer Contains successfully.");
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
        }

        //fix the answer of student 
        [HttpPut("fix")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FixEssayAnswer([FromRoute] AnswerEditorModel model)
        {
            try
            {
                //get Essay Answer by Id
                var essayAnswer = await _service.EssayAnswerService.GetByIdAsync(model.EssayAnswerId);

                //add fixed result to answer
                essayAnswer.ResultFixed = model.ResultFixed;

                //update answer 
                await _service.EssayAnswerService.UpdateAsync(essayAnswer);

                return SuccessResult(model, "Update Essay Answer successfully.");
            }
            catch (Exception e)
            {
                return ErrorResult(e.ToString());
            }
        }

        //get all by userId
        [AllowAnonymous]
        [HttpGet("get/all/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllByUserId([FromRoute] int userId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ValidModel();
                }
                //get answer list on server based on userId
                var answerList = await _service.AnswerService.GetAllByUserIdAsync(userId);
                //create answer list
                var newAnswersList = new List<AnswerResponseModel>();
                //map to response model
                foreach (var answer in answerList)
                {
                    var newAnswer = new AnswerResponseModel()
                    {
                        Id = answer.Id,
                        Score = answer.Score,
                        TestId = answer.TestId,
                        UserId = answer.UserId
                    };
                    newAnswersList.Add(newAnswer);
                }

                return SuccessResult(newAnswersList, "Get all Answers based on user Id = " + userId.ToString() + " successfully.");
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
        }

        //get by userId paging
        [AllowAnonymous]
        [HttpPost("{userId}/get/all/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllByUserIdPaging([FromRoute]int userId, [FromRoute] int pageIndex)
        {
            var newTests = new List<AnswerResponseModel>();
            try
            {
                //get totalCount
                var totalCount = await _service.AnswerService.CountTotalAnswerByUserId(userId);

                //run code
                var queryRes = await _service.AnswerService.GetAllAnswersPagingByUserId(25, pageIndex, userId);
                if (totalCount > 0)
                {
                    //render page
                    newTests = queryRes.Select(x => new AnswerResponseModel
                    {
                        Id = x.Id,
                        Score = x.Score,
                        TestId = x.TestId,
                        UserId = x.UserId
                    }).ToList();
                    return SuccessResult(newTests, "Get all Answers with paging successfully.");
                }
                else
                {
                    return SuccessResult(newTests, "Get all Answers with paging successfully.");
                }
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
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
            try
            {
                var answer = await _service.AnswerService.GetByIdAsync(id);
                if (answer == null)
                {
                    return ErrorResult($"Can not found Answer with Id: {id}");
                }
                var answerRes = new AnswerResponseModel
                {
                    Id = answer.Id,
                    Score = answer.Score,
                    UserId = answer.UserId,
                    TestId = answer.TestId
                };
                return SuccessResult(answerRes, "Get Answer with AnswerId = "+ answerRes.Id.ToString() +" successfully.");
            }
            catch (Exception e)
            {

                return ErrorResult(e.ToString());
            }
        }


        //get answer by Id Contains
        [HttpGet("{id}/contains")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAnswerByIdContains([FromRoute] int id)
        {
            try
            {
                var answer = await _service.AnswerService.GetByIdAsync(id);
                if (answer == null)
                {
                    return ErrorResult($"Can not found Answer with Id: {id}");
                }
                var answerRes = new AnswerContainerResponseModel
                {
                    Id = answer.Id,
                    Score = answer.Score,
                    UserId = answer.UserId,
                    TestId = answer.TestId
                };

                //get multiple choices answer -> mapp
                var multipleChoicesAnswersList = await _service.MultipleChoicesAnswerService.GetByAnswerIdAsync(answer.Id);
                var resMultipleChoicesAnwerList = new List<MultipleChoicesAnswerResponseModel>();
                foreach (var multipleChoicesAnswer in multipleChoicesAnswersList)
                {
                    var resMul = new MultipleChoicesAnswerResponseModel()
                    {
                        AnswerId = multipleChoicesAnswer.AnswerId,
                        Id = multipleChoicesAnswer.Id,
                        IsBingo = multipleChoicesAnswer.IsBingo,
                        MultipleChoicesExerciseId = multipleChoicesAnswer.MultipleChoicesExerciseId,
                        Result = multipleChoicesAnswer.Result
                    };
                    resMultipleChoicesAnwerList.Add(resMul);
                }
                answerRes.MultipleChoicesAnswerResponseModels = resMultipleChoicesAnwerList;

                //get essay answer -> mapp
                var essayAnswersList = await _service.EssayAnswerService.GetByAnswerIdAsync(answer.Id);
                var resEssayAnswersList = new List<EssayAnswerResponseModel>();
                foreach (var essayAnswer in essayAnswersList)
                {
                    var resEss = new EssayAnswerResponseModel()
                    {
                        AnswerId = essayAnswer.AnswerId,
                        EssayExerciseId = essayAnswer.EssayExerciseId,
                        Id = essayAnswer.Id,
                        IsBingo = essayAnswer.IsBingo,
                        Result = essayAnswer.Result
                    };
                    resEssayAnswersList.Add(resEss);
                }
                answerRes.EssayAnswerResponseModels = resEssayAnswersList;

                //return success result
                return SuccessResult(answerRes, "Get Answer with AnswerId = "+ answerRes.Id.ToString() +" successfully.");
            }
            catch (Exception e)
            {
                return ErrorResult(e.ToString());
            }
        }
        #endregion
    }
}
