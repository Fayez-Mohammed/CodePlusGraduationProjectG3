using LearnSphere.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Storage;

namespace LearnSphere.API.Filters
{
    public class ResponseWrapperFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if(context.Result is ObjectResult objectResult )
            {
                if(objectResult.Value is ProblemDetails)
                {
                    await next();
                    return;
                }
                bool isAlreadyWrapped = objectResult.Value is IApiResponse;
               // bool isProblemDetails = objectResult.Value is ProblemDetails;
                if (!isAlreadyWrapped /*&& !isProblemDetails*/)
                {
                    var response = new ApiResponse<object>
                    {
                        Data = objectResult.Value,
                        Message = "Success",
                        TraceId = context.HttpContext.TraceIdentifier,
                        StatusCode = objectResult.StatusCode ?? context.HttpContext.Response.StatusCode
                    };
                    context.Result = new ObjectResult(response)
                    {
                        StatusCode = objectResult.StatusCode ?? context.HttpContext.Response.StatusCode,
                    };
                }
            }
            else if(context.Result is StatusCodeResult statusCodeResult)
            {
                var response = new ApiResponse<object>
                {
                    Data = null,
                    Message = "Success",
                    TraceId = context.HttpContext.TraceIdentifier,
                    StatusCode = statusCodeResult.StatusCode 
                };
                context.Result = new ObjectResult(response)
                {
                    StatusCode = statusCodeResult.StatusCode
                };

            }
            await next();
        }
    }
}
