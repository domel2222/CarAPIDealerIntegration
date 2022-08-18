using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CarDealerAPI.Exceptions;


namespace CarDealerAPI.Middlewere
{
    public  class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            this._logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                 await next.Invoke(context);
            }
            catch (BadRequestException badRequest)
            {

                context.Response.StatusCode = 400;
                 await context.Response.WriteAsync(badRequest.Message);
            }
            catch (ForbiddenExc forbiddenExc)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(forbiddenExc.Message);
            }
            catch (NotFoundException notFoundException)
            {

                context.Response.StatusCode = 404;
                 await context.Response.WriteAsync(notFoundException.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Server error, try again");
            }
        }
    
    }
}
