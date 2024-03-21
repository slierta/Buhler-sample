using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Sample.Contracts;
using Sample.Models;
using Sample.Models.Requests;

namespace SampleAPI.Controllers
{
    /// <summary>
    /// Declares the public endpoints for the API sample
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TrucksController(ILogger<TrucksController> logger, ITruckFinder finder) : ControllerBase
    {
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType<IEnumerable<FoodTruck>>((int)HttpStatusCode.OK)]
        [ProducesResponseType<ProblemDetails>((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType<ProblemDetails>((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Query([NotNull][FromQuery] TruckFindRequest model)
        {
            logger.LogDebug("Request received {r}", model);
            if (model == null)
                return BadRequest(new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "Bad request",
                    Detail = "The request can not be empty"
                });
            if (!model.IsValid())
                return BadRequest(new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "Bad request",
                    Detail = "The model must contains at least some food and one result"
                });

            try
            {
                var data = await finder.Search(model);
                logger.LogDebug("Received ´{c} trucks", data.Count());
                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ProblemDetails()
                {
                    Detail = e.Message,
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = $"{nameof(Query)}"
                });
            }
        }
    }
}
