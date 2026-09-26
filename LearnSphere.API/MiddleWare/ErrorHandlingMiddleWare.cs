//using OpenQA.Selenium;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
using System.Net;

namespace LearnSphere.API.MiddleWare
{
    public class ErrorHandlingMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleWare> _logger;
        private readonly IHostEnvironment _env;
        public ErrorHandlingMiddleWare(RequestDelegate next, ILogger<ErrorHandlingMiddleWare> logger
            , IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Request was canceled by the client " +
                    "method: {method} path: {path} traceId: {traceId} "
                    , context.Request.Method, context.Request.Path, context.TraceIdentifier);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Unhandled Exception while processing " +
                   "method: {method} path: {path} traceId: {traceId} "
                   , context.Request.Method, context.Request.Path, context.TraceIdentifier);
              await  HandleExceptionAsync(context, ex);
            }

        }
        public async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (context.Response.HasStarted)
                return;
            var statusCode=ExceptionStatusCodeMapper.GetStatusCode(ex);
            context.Response.StatusCode= statusCode;
            var problemDetails = new ProblemDetails
            {
                Status=statusCode,
                Title=GetTitleForStatusCode(statusCode),
                Detail=ex.Message,
                Instance=context.Request.Path
                
            };
            var isDevelopment = _env.IsDevelopment();
            problemDetails.Extensions["traceId"] = context.TraceIdentifier;
            if (isDevelopment)
            {
                problemDetails.Extensions["exception"]=ex.GetType().Name;
                problemDetails.Extensions["stackTrace"]=ex.StackTrace;
            }
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        private static string GetTitleForStatusCode(int statusCode) => statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Resource Not Found",
            409 => "Conflict",
            _ => "An error occurred while processing your request."
        };
        public static class ExceptionStatusCodeMapper
        {
            public static int GetStatusCode(Exception ex) => ex switch
            {
               NotFoundException =>(int)HttpStatusCode.NotFound,
                BadRequestException =>(int)HttpStatusCode.BadRequest,
                UnauthorizedException =>(int)HttpStatusCode.Unauthorized,
                ForbiddenException =>(int)HttpStatusCode.Forbidden,
                _=>(int)HttpStatusCode.InternalServerError



            };
        }
    }
}
