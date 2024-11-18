using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TaskApi.Filter
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorDetails = context.ModelState
                                .Where(r => r.Value.Errors.Count > 0)
                                .Select(r => new
                                {
                                    Field = r.Key,
                                    message = r.Value.Errors.FirstOrDefault()?.ErrorMessage
                                })
                                .ToList();

                var errorResponse = new
                {
                    Status = false,
                    Message = "Validation failed.",
                    Errors = errorDetails
                };

                //context.Result = new BadRequestObjectResult(errorResponse);
                context.Result = new JsonResult(errorResponse)
                {
                    StatusCode = 400
                };


            }
        }

    }
}


