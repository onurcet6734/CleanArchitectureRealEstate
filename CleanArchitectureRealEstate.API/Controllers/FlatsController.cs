using CleanArchitectureRealEstate.Application.Features.Flats.Commands.CreateFlat;
using CleanArchitectureRealEstate.Application.Features.Flats.Commands.DeleteFlat;
using CleanArchitectureRealEstate.Application.Features.Flats.Commands.UpdateFlat;
using CleanArchitectureRealEstate.Application.Features.Flats.Commands.UpdateFlatPartial;
using CleanArchitectureRealEstate.Application.Features.Flats.Commands.UploadFlatImage;
using CleanArchitectureRealEstate.Application.Features.Flats.Queries.GetFlatById;
using CleanArchitectureRealEstate.Application.Features.Flats.Queries.GetFlatList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureRealEstate.WebAPI.Controllers
{
    [ApiController]
    [Route("api/flats")]
    public class FlatsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FlatsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] GetFlatListQuery query)
        {
            var result = await _mediator.Send(query);
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

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateFlatCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new { error = "Id mismatch" });
            }

            await _mediator.Send(command);
            return NoContent();
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] UpdateFlatPartialCommand command)
        {
            command.Id = id; //command.Id = id; yapılmazsa ,  ASP.NET Core model binding Id değerini payload’dan almaya çalışır. Bunun sebebi model binder’ın birden fazla kaynaktan veri bağlayabilmesidir.
            var result = await _mediator.Send(command);

            if (!result.Succeeded)
            {
                return BadRequest(new { error = result.Error });
            }

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteFlatCommand(id));
            return NoContent();
        }

        [Authorize]
        [HttpPost("upload-images")]
        [Consumes("multipart/form-data")] // ✅ ÖNEMLİ: Multipart olduğunu net söylüyoruz
        public async Task<IActionResult> UploadImages([FromForm] UploadImageCommand request)
        {

            var uploadedImages = await _mediator.Send(new UploadImageCommand(request.Files));

            return Ok(new
            {
                images = uploadedImages,   // handler’dan gelen liste
                count = uploadedImages.Count
            });
        }
    }
}
