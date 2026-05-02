using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiController : ControllerBase
    {
        private readonly IDescriptionGeneratorService _generator;
        public AiController(IDescriptionGeneratorService generator)
        {
            _generator = generator;
        }

        [HttpPost("generate-description")]
        [ProducesResponseType(typeof(GenerateDescriptionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]    // re-used for AI errors via ExceptionMiddleware
        public async Task<IActionResult> GenerateDescription(
        [FromBody] GenerateDescriptionRequest request)
        {
            var result = await _generator.GenerateAsync(request);
            return Ok(result);
        }
    }
}
