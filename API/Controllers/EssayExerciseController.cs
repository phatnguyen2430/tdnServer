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
        public async Task<IActionResult> CreateEssayExercise([FromBody] EssayExerciseModel model)
        {
            var newEssayExercise = new EssayExercise();
            try
            {
                if (!ModelState.IsValid)
                {
                    ValidModel();
                }
                newEssayExercise = new EssayExercise()
                {
                    Result = model.Result,
                    TestId = model.TestId,
                    Title = model.Title
                };
                await _service.EssayExerciseService.AddAsync(newEssayExercise);
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
            return SuccessResult(newEssayExercise, "Created Essay Exercise successfully.");
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
            var essayExercise = await _service.EssayExerciseService.GetByIdAsync(id);
            if (essayExercise == null)
            {
                return ErrorResult($"Can not found Essay Exercise with Id: {id}");
            }
            var essayExerciseRes = new EssayExerciseModel()
            {
                
            };
            return SuccessResult(essayExerciseRes, "Get Essay Exercise successfully.");
        }


        #endregion

        #region get all

        #endregion
    }
}
