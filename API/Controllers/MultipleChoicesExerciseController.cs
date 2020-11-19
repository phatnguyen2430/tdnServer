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
using WebAPI.Models.MultipleChoicesExercise;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MultipleChoicesExerciseController : BaseApiController
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IService _service;
        private readonly IMapper _mapper;
        #endregion

        #region ctor
        public MultipleChoicesExerciseController(IUnitOfWork unitOfWork,
            IService service,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _service = service;
            _mapper = mapper;
        }
        #endregion
        #region check if existing, add, update, delete
        //get MultipleChoicesExercise in test by test id


        //create MultipleChoicesExercise
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
        public async Task<IActionResult> CreateMultipleChoicesExercise([FromBody] MultipleChoicesExerciseRequestModel model)
        {
            var newMultipleChoicesExercise = new MultipleChoicesExercise();
            try
            {
                if (!ModelState.IsValid)
                {
                    ValidModel();
                }
                newMultipleChoicesExercise = new MultipleChoicesExercise()
                {
                    TestId = model.TestId,
                    Title = model.Title,
                    RightResult = model.RightResult,
                    FalseResult1 = model.FalseResult1,
                    FalseResult2 = model.FalseResult2,
                    FalseResult3 = model.FalseResult3
                };
                await _service.MultipleChoicesExerciseService.AddAsync(newMultipleChoicesExercise);
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
            return SuccessResult(model, "Created MultipleChoicesExercise successfully.");
        }

        //get MultipleChoicesExercise by id
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMultipleChoicesExerciseById([FromRoute] int id)
        {
            var multipleChoicesExercise = await _service.MultipleChoicesExerciseService.GetByIdAsync(id);
            if (multipleChoicesExercise == null)
            {
                return ErrorResult($"Can not found Multiple Choices Exercise with Id: {id}");
            }
            var multipleChoicesExerciseRes = new MultipleChoicesExerciseResponseModel
            {
                Id = multipleChoicesExercise.Id,
                TestId = multipleChoicesExercise.TestId,
                Title = multipleChoicesExercise.Title,
                RightResult = multipleChoicesExercise.RightResult,
                FalseResult1 = multipleChoicesExercise.FalseResult1,
                FalseResult2 = multipleChoicesExercise.FalseResult2,
                FalseResult3 = multipleChoicesExercise.FalseResult3
            };
            return SuccessResult(multipleChoicesExerciseRes, "Get Multiple Choices Exercise successfully.");
        }


        /// <summary>
        /// update multipe choices exercise by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMultipleChoicesExerciseById([FromRoute] int id, 
            [FromBody] MultipleChoicesExerciseRequestModel multipleChoicesExerciseRequestModel)
        {
            try
            {
                //get entity by id
                var multipleChoicesExercise = await _service.MultipleChoicesExerciseService.GetByIdAsync(id);
                if (multipleChoicesExercise == null)
                {
                    return ErrorResult($"Can not found Multiple Choices Exercise with Id: {id}");
                }

                //map requestModel to entity -> update
                multipleChoicesExercise.Image = multipleChoicesExerciseRequestModel.Image;
                multipleChoicesExercise.Title = multipleChoicesExerciseRequestModel.Title;
                multipleChoicesExercise.RightResult = multipleChoicesExerciseRequestModel.RightResult;
                multipleChoicesExercise.FalseResult1 = multipleChoicesExerciseRequestModel.FalseResult1;
                multipleChoicesExercise.FalseResult2 = multipleChoicesExerciseRequestModel.FalseResult2;
                multipleChoicesExercise.FalseResult3 = multipleChoicesExerciseRequestModel.FalseResult3;

                //update
                await _service.MultipleChoicesExerciseService.UpdateAsync(multipleChoicesExercise);

                //check to return
                if (multipleChoicesExercise == null)
                {
                    return ErrorResult($"Can not found Multiple Choices Exercise with Id: {id}");
                }

                var multipleChoicesExerciseRes = new MultipleChoicesExerciseResponseModel
                {
                    Id = multipleChoicesExercise.Id,
                    TestId = multipleChoicesExercise.TestId,
                    Title = multipleChoicesExercise.Title,
                    RightResult = multipleChoicesExercise.RightResult,
                    FalseResult1 = multipleChoicesExercise.FalseResult1,
                    FalseResult2 = multipleChoicesExercise.FalseResult2,
                    FalseResult3 = multipleChoicesExercise.FalseResult3
                };
                return SuccessResult(multipleChoicesExerciseRes, "Update Choices Exercise successfully.");
            }
            catch (Exception e)
            {
                return ErrorResult(e.ToString());
            }
        }

        #endregion
    }
}