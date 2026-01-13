using CleanArchitectureRealEstate.Application.Features.Flats.Commands.CreateFlat;
using CleanArchitectureRealEstate.Application.Features.Flats.Commands.DeleteFlat;
using CleanArchitectureRealEstate.Application.Features.Flats.Commands.UpdateFlat;
using CleanArchitectureRealEstate.Application.Features.Flats.Queries.GetFlatById;
using CleanArchitectureRealEstate.Application.Features.Flats.Queries.GetFlatList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureRealEstate.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlatsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FlatsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var result = await _mediator.Send(new GetFlatListQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetFlatByIdQuery(id));
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateFlatCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateFlatCommand command)
        {
            if (id != command.Id)
                return BadRequest();

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteFlatCommand(id));
            return NoContent();
        }
    }
}
