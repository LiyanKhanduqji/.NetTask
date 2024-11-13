//custom error-handling middleware catch any unexpected errors that occur during the processing of an HTTP request
using System;
using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
IHostEnvironment env) // env used to see whether we're running in production or development mode
{
    // the method must be called 'InvokeAstns'
    public async Task InvokeAsync(HttpContext context)
    {

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = env.IsDevelopment()
            ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace)
            : new ApiException(context.Response.StatusCode, ex.Message, "Internal server error");

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}
//RequestDelegate next: Represents the next part of the request pipeline. The middleware calls next(context) to move the request to the next middleware.
//ILogger: Used to log error details for debugging
// IHostEnvironment env: Checks if the app is in development or production mode. This helps customize error messages based on the environment.

//InvokeAstns : core function. here the middleware does its work, handling any errors that happen in the pipeline.