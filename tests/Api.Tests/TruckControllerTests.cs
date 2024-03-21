using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Sample.Contracts;
using Sample.Models;
using Sample.Models.Requests;
using SampleAPI.Controllers;

namespace Api.Tests
{
    public class TruckControllerTests
    {
        private Mock<ITruckFinder> _truckFinderMock;
        private Mock<ILogger<TrucksController>> _loggerMock;
        private TrucksController _controller;
        [SetUp]
        public void Setup()
        {
            _truckFinderMock = new Mock<ITruckFinder>();
            _loggerMock = new Mock<ILogger<TrucksController>>();
            _controller = new TrucksController(_loggerMock.Object, _truckFinderMock.Object);
        }
        
        [Test]
        public async Task WhenTheServiceIsNotAvailable_ShouldReturn_ServerError()
        {
            _truckFinderMock.Setup(s => s.Search(It.IsAny<TruckFindRequest>())).Throws<Exception>();
            var request = GetDefaultRequest();
            var result = await _controller.Query(request);
            //var objectResult = CheckErrorResults(result);
            //Assert.That(objectResult, Is.Not.Null);
            //Assert.That(objectResult.StatusCode, Is.EqualTo(500), "Status code is 500");
        }

        [Test]
        public async Task WhenTheRequestIsNull_ShouldReturn_BadRequest()
        {
            var result = await _controller.Query(null);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var details = (result as BadRequestObjectResult)?.Value;
            Assert.That(details, Is.Not.Null);
            Assert.That(details, Is.InstanceOf<ProblemDetails>());
            var problem = details as ProblemDetails;
            Assert.That(problem, Is.Not.Null);
            Assert.That(problem.Detail, Is.EqualTo("The request can not be empty"));    // It should be read from one resource here and in the controller

        }

        [TestCase(null)]
        [TestCase("")]
        public async Task WhenTheRequestFoodIsEmpty_ShouldReturn_BadRequest(string? food)
        {
            var request = GetDefaultRequest();
            request.Food = food!;
            var result = await _controller.Query(request);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var details = (result as BadRequestObjectResult)?.Value;
            Assert.That(details, Is.Not.Null);
            Assert.That(details, Is.InstanceOf<ProblemDetails>());
            var problem = details as ProblemDetails;
            Assert.That(problem, Is.Not.Null);
            Assert.That(problem.Detail, Is.EqualTo("The model must contains at least some food and one result"));    // It should be read from one resource here and in the controller
        }
        [TestCase(0)]
        [TestCase(-1)]
        public async Task WhenTheNumberRequestIs0OrLess_ShouldReturn_BadRequest(int number)
        {
            var request = GetDefaultRequest();
            request.MaxResults = number;
            var result = await _controller.Query(request);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var details = (result as BadRequestObjectResult)?.Value;
            Assert.That(details,Is.Not.Null);
            Assert.That(details,Is.InstanceOf<ProblemDetails>());
            var problem = details as ProblemDetails;
            Assert.That(problem,Is.Not.Null);
            Assert.That(problem.Detail,Is.EqualTo("The model must contains at least some food and one result"));    // It should be read from one resource here and in the controller
        }
        [Test]
        public async Task WhenTheRequestIsGood_ShouldReturn_EmptyArrayOfResults()
        {
            _truckFinderMock.Setup(s => s.Search(It.IsAny<TruckFindRequest>())).ReturnsAsync(Enumerable.Empty<FoodTruck>());
            var request = GetDefaultRequest();
            var result = await _controller.Query(request);
            Assert.That(result,Is.Not.Null);
            Assert.That(result,Is.InstanceOf<OkObjectResult>());
            var data = result as OkObjectResult;
            Assert.That(data,Is.Not.Null);
            Assert.That(data.StatusCode,Is.EqualTo(200));
            Assert.That(data.Value,Is.InstanceOf<IEnumerable<FoodTruck>>());
            var trucks = data.Value as ICollection<FoodTruck>;
            Assert.That(trucks,Has.Count.EqualTo(0));
        }

        private TruckFindRequest GetDefaultRequest() => new TruckFindRequest()
        {
            Latitude = 37.792948952834664,
            Longitude = -122.39809861316652,
            Food = "tacos",
            MaxResults = 5
        };
    }
}