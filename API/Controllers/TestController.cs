using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.EssayExerciseAggregate;
using ApplicationCore.Entities.MultipleChoicesExerciseAggregate;
using ApplicationCore.Entities.TestAggregate;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.EssayExercise;
using WebAPI.Models.MultipleChoicesExercise;
using WebAPI.Models.Test;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseApiController
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IService _service;
        private readonly IMapper _mapper;
        #endregion

        #region ctor
        public TestController(IUnitOfWork unitOfWork,
            IService service,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _service = service;
            _mapper = mapper;
        }
        #endregion
        #region check if existing, add, update, delete
        //get  Test in test by test id


        //get  Test by id
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTestById([FromRoute] int id)
        {
            var test = await _service.TestService.GetByIdAsync(id);
            if (test == null)
            {
                return ErrorResult($"Can not found test with Id: {id}");
            }
            var testRes = new TestModel
            {
                Name = test.Name,
                Id = test.Id,
                Type = (Models.Test.TestType)test.Type
            };
            return SuccessResult(testRes, "Get test with Id = "+ test.Id.ToString() +" successfully.");
        }

        //get  Test by id
        [HttpPut("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTestById([FromRoute] int id,[FromBody] TestRequestModel testRequestModel)
        {
            try
            {
                //get entity by id
                var test = await _service.TestService.GetByIdAsync(id);
                if (test == null)
                {
                    return ErrorResult($"Can not found test with Id: {id}");
                }

                //map requestModel to entity -> update
                test.Name = testRequestModel.Name;
                test.Type = (ApplicationCore.Entities.TestAggregate.TestType)testRequestModel.Type;

                //update
                await _service.TestService.UpdateAsync(test);

                //check to return
                if (test == null)
                {
                    return ErrorResult($"Can not found test with Id: {id}");
                }
                var testRes = new TestModel
                {
                    Name = test.Name,
                    Id = test.Id,
                    Type = (Models.Test.TestType)test.Type
                };
                return SuccessResult(testRes, "Update Test successfully.");
            }
            catch (Exception e)
            {
                return ErrorResult(e.ToString());
            }
        }




        //get  Test by id
        [HttpGet("{id}/contains")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTestByIdContains([FromRoute] int id)
        {
            var test = await _service.TestService.GetByIdAsync(id);
            if (test == null)
            {
                return ErrorResult($"Can not found test with Id: {id}");
            }
            //create TestContainerModel
            var testRes = new TestContainerResponseModel
            {
                Name = test.Name,
                Id = test.Id,
                Type = (Models.Test.TestType)test.Type
            };

            //create multiple test
            var multipleChoicesExercisesList = await _service.MultipleChoicesExerciseService.GetByTestIdAsync(id);
            var mulList = new List<MultipleChoicesExerciseResponseModel>();
            foreach (var multipleChoicesExercise in multipleChoicesExercisesList)
            {
                var newMultipleChoiceExercise = new MultipleChoicesExerciseResponseModel()
                {
                    Id = multipleChoicesExercise.Id,
                    TestId = multipleChoicesExercise.TestId,
                    Title = multipleChoicesExercise.Title,
                    Image = multipleChoicesExercise.Image,
                    RightResult = multipleChoicesExercise.RightResult,
                    FalseResult1 = multipleChoicesExercise.FalseResult1,
                    FalseResult2 = multipleChoicesExercise.FalseResult2,
                    FalseResult3 = multipleChoicesExercise.FalseResult3
                };
                mulList.Add(newMultipleChoiceExercise);
            }
            testRes.MultipleChoicesExerciseResponseModels = mulList;

            //create essay test
            var essayExercisesList = await _service.EssayExerciseService.GetByTestIdAsync(id);
            var essList = new List<EssayExerciseResponseModel>();
            foreach (var essayExercise in essayExercisesList)
            {
                var newEssayExercise = new EssayExerciseResponseModel()
                {
                    Id = essayExercise.Id,
                    Image = essayExercise.Image,
                    Result = essayExercise.Result,
                    Title = essayExercise.Title,
                    TestId = essayExercise.TestId
                };
                essList.Add(newEssayExercise);
            }
            testRes.EssayExerciseResponseModels = essList;

            //return TestContainerModel
            return SuccessResult(testRes, "Get test with Id = " + testRes.Id.ToString() + " successfully.");
        }

        //add new  Test
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
        public async Task<IActionResult> CreateTest([FromBody] TestCreateModel model)
        {
            var newTest = new Test();
            try
            {
                if (!ModelState.IsValid)
                {
                    ValidModel();
                }

                newTest = new Test()
                {
                    Type = (ApplicationCore.Entities.TestAggregate.TestType)model.Type,
                    Name = model.Name
                };
                await _service.TestService.AddAsync(newTest);
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
            return SuccessResult(newTest, "Created Test successfully.");
        }

        //Add Test Model -> contains 28 multiple choices & 8 essay
        [AllowAnonymous]
        [HttpPost("create/contains")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTestContainsExercises([FromBody] TestContainerRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ValidModel();
                }

                //Add Test
                var newTest = new Test()
                {
                    Type = (ApplicationCore.Entities.TestAggregate.TestType)model.Type,
                    Name = model.Name
                };
                await _service.TestService.AddAsync(newTest);

                //create response model
                var responseModel = new TestContainerResponseModel()
                {
                    Id = newTest.Id,
                    Name = model.Name,
                    Type = model.Type
                };
                //add essay exercises
                var responseEssayExercises = new List<EssayExerciseResponseModel>();
                //add multiply choices exercises
                var responseMulChoicesExercises = new List<MultipleChoicesExerciseResponseModel>();

                //add multiple exercise
                foreach (var multipleExercise in model.MultipleChoicesExerciseRequestModels)
                {
                    var newMultipleExercise = new MultipleChoicesExercise()
                    {
                        TestId = newTest.Id,
                        RightResult = multipleExercise.RightResult,
                        FalseResult1 = multipleExercise.FalseResult1,
                        FalseResult2 = multipleExercise.FalseResult2,
                        FalseResult3 = multipleExercise.FalseResult3,
                        Title = multipleExercise.Title,
                        Image = multipleExercise.Image
                    };
                    await _service.MultipleChoicesExerciseService.AddAsync(newMultipleExercise);

                    //add to response model
                    var newMultipleExerciseResponse = new MultipleChoicesExerciseResponseModel()
                    {
                        Id = newMultipleExercise.Id,
                        TestId = newTest.Id,
                        RightResult = multipleExercise.RightResult,
                        FalseResult1 = multipleExercise.FalseResult1,
                        FalseResult2 = multipleExercise.FalseResult2,
                        FalseResult3 = multipleExercise.FalseResult3,
                        Title = multipleExercise.Title,
                        Image = multipleExercise.Image
                    };
                    responseMulChoicesExercises.Add(newMultipleExerciseResponse);
                }

                //add essay exercise
                foreach (var essayExercise in model.EssayExerciseRequestModels)
                {
                    var newEssayExerecise = new EssayExercise()
                    {
                        TestId = newTest.Id,
                        Title = essayExercise.Title,
                        Result = essayExercise.Result,
                        Image = essayExercise.Image
                    };
                    await _service.EssayExerciseService.AddAsync(newEssayExerecise);

                    //add to response model
                    var newEssayExereciseResponse = new EssayExerciseResponseModel()
                    {
                        Id = newEssayExerecise.Id,
                        TestId = newTest.Id,
                        Title = essayExercise.Title,
                        Result = essayExercise.Result,
                        Image = essayExercise.Image
                    };
                    responseEssayExercises.Add(newEssayExereciseResponse);
                }

                //conclusion response model
                responseModel.MultipleChoicesExerciseResponseModels = responseMulChoicesExercises;
                responseModel.EssayExerciseResponseModels = responseEssayExercises;
                return SuccessResult(responseModel, "Created Test with Exercises successfully.");
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
        }

        /// <summary>
        /// <response code=200>Request completed Successfully</response>
        /// <response code=400>Bad Request</response>
        /// <response code=401>Don't have permission</response>
        /// <response code=500>Internal server error</response>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("get/all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var newTests = new List<TestModel>();
            try
            {
                if (!ModelState.IsValid)
                {
                    ValidModel();
                }
                var newTestList = await _service.TestService.GetAllAsync();
                foreach (var test in newTestList)
                {
                    var newTestModel = new TestModel()
                    {
                        Type = (Models.Test.TestType)test.Type,
                        Id = test.Id,
                        Name = test.Name
                    };
                    newTests.Add(newTestModel);
                }
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
            return SuccessResult(newTests, "Get all Test successfully.");
        }


        /// <summary>
        /// <response code=200>Request completed Successfully</response>
        /// <response code=400>Bad Request</response>
        /// <response code=401>Don't have permission</response>
        /// <response code=500>Internal server error</response>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("get/all/{pageIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPaging([FromRoute] int pageindex)
        {
            var newTests = new List<TestModel>();
            try
            {
                //get totalCount
                var totalCount = await _service.TestService.CountTotalTest();

                //run code
                var queryRes = await _service.TestService.GetAllTestsPaging(pageindex);
                if (totalCount >0)
                {
                    //render page
                    newTests = queryRes.Select(x => new TestModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Type = (Models.Test.TestType)x.Type
                    }).ToList();
                    return SuccessResult(newTests, "Get all Tests with paging successfully.");
                }
                else
                {
                    return SuccessResult(newTests, "Get all Tests with paging successfully.");
                }
            }
            catch (Exception e)
            {
                return ErrorResult(e.Message);
            }
        }


        #endregion
    }
}
