using CleanArchitectureRealEstate.Application.Features.FlatImages.Commands.CreateFlatImage;
using CleanArchitectureRealEstate.Application.Features.FlatImages.Queries.GetById;
using CleanArchitectureRealEstate.Application.Features.FlatImages.Queries.GetList;
using CleanArchitectureRealEstate.Application.Features.FlatImagess.Commands.UpdateFlatImage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureRealEstate.WebAPI.Controllers
{
    [ApiController]
    [Route("api/flat-images")]
    public class FlatImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FlatImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetFlatImageListQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFlatImageCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(
            int id,
            [FromBody] UpdateFlatImageCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new { error = "Id mismatch" });
            }

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(
                new GetFlatImageByIdQuery(id));

            if (result is null)
            {
                return NotFound(new { error = "Flat image not found" });
            }

            return Ok(result);
        }

    }

}
