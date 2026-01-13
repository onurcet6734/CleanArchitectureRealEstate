using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CleanArchitectureRealEstate.WebAPI.Models;

namespace CleanArchitectureRealEstate.WebAPI.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value!.Errors.Count > 0)
                    .Select(x => new ApiError
                    {
                        Field = x.Key,
                        Message = x.Value!.Errors.First().ErrorMessage
                    })
                    .ToList();

                context.Result = new BadRequestObjectResult(errors);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
