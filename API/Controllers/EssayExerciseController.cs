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
using WebAPI.Models.EssayExercise;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EssayExerciseController : BaseApiController
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IService _service;
        private readonly IMapper _mapper;
        #endregion

        #region ctor
        public EssayExerciseController(IUnitOfWork unitOfWork,
            IService service,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _service = service;
            _mapper = mapper;
        }
        #endregion
        #region check if existing, add, update, delete
        //get essay exercise in test by test id


        //create EssayExercise
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
        public async Task<IActionResult> CreateEssayExercise([FromBody] EssayExerciseRequestModel model)
        {
            var newEssayExercise = new EssayExercise();
            try
            {
                if (!ModelState.IsValid)
                {
                    ValidModel();
                }
                //delete base -> ','
                model.Image = model.Image.Split(',')[1];
                byte[] imageBytes = Convert.FromBase64String(model.Image);
                newEssayExercise = new EssayExercise()
                {
                    Result = model.Result,
                    TestId = model.TestId,
                    Title = model.Title,
                    Image = imageBytes
                };
                await _service.EssayExerciseService.AddAsync(newEssayExercise);
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
            return SuccessResult("Created Essay Exercise successfully.");
        }

        //get Essay Exercise by id
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEssayExerciseById([FromRoute] int id)
        {
            try
            {
                var essayExercise = await _service.EssayExerciseService.GetByIdAsync(id);
                if (essayExercise == null)
                {
                    return Unauthorized($"Can not found Essay Exercise with Id: {id}");
                }
                string base64String = Convert.ToBase64String(essayExercise.Image, 0,
                essayExercise.Image.Length);
                var essayExerciseRes = new EssayExerciseResponseModel()
                {
                    Id = essayExercise.Id,
                    Image = base64String,
                    Result = essayExercise.Result,
                    TestId = essayExercise.TestId,
                    Title = essayExercise.Title
                };
                return SuccessResult(essayExerciseRes, "Get Essay Exercise successfully.");
            }
            catch (Exception e)
            {
                return Unauthorized(e.ToString());
            }
        }

        /// <summary>
        /// update essay exercise
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEssayExerciseById([FromRoute] int id,
            [FromBody] EssayExerciseRequestModel essayExerciseRequestModel)
        {
            try
            {
                //get entity by id
                var essayExercise = await _service.EssayExerciseService.GetByIdAsync(id);
                if (essayExercise == null)
                {
                    return Unauthorized($"Can not found Essay Exercise with Id: {id}");
                }
                essayExerciseRequestModel.Image = essayExerciseRequestModel.Image.Split(',')[1];
                byte[] imageBytes = Convert.FromBase64String(essayExerciseRequestModel.Image);
                //map requestModel to entity -> update
                essayExercise.Image = imageBytes;
                essayExercise.Title = essayExerciseRequestModel.Title;
                essayExercise.Result = essayExerciseRequestModel.Result;

                //update
                await _service.EssayExerciseService.UpdateAsync(essayExercise);


                //check to return
                if (essayExercise == null)
                {
                    return Unauthorized($"Can not found Essay Exercise with Id: {id}");
                }

                var essayExerciseRes = new EssayExerciseResponseModel()
                {
                    Id = essayExercise.Id,
                    Image = essayExerciseRequestModel.Image,
                    Result = essayExercise.Result,
                    TestId = essayExercise.TestId,
                    Title = essayExercise.Title
                };
                return SuccessResult(essayExerciseRes, "Update Essay Exercise successfully.");
            }
            catch (Exception e)
            {
                return Unauthorized(e.ToString());
            }
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteById([FromRoute] int id)
        {
            try
            {
                //get entity by id
                var entity = await _service.EssayExerciseService.GetByIdAsync(id);
                if (entity == null)
                {
                    return Unauthorized($"Can not found essay exercise with Id: {id}");
                }

                await _service.EssayExerciseService.DeleteAsync(entity);

                return SuccessResult("Delete essay exercise successfully.");
            }
            catch (Exception e)
            {
                return Unauthorized(e.ToString());
            }
        }
        #endregion

        #region get all

        #endregion
    }
}
