using LearnSphere.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LearnSphere.API.Filters
{
    public class ErrorFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next();
            if(executedContext.Result is ObjectResult objectResult)
            {
                if (objectResult.Value is Result result)
                {
                    if (!result.IsSuccess)
                    {
                        objectResult.StatusCode = result.StatusCode;
                        objectResult.Value = new ProblemDetails
                        {
                            Status = result.StatusCode,
                            Detail = result.Error,
                            Instance = context.HttpContext.Request.Path
                        };
                    }
                    else
                    {
                        objectResult.Value = result.GetType().GetProperty("Value")?.GetValue(result);

                    }

                }
            }
        }
    }
}
