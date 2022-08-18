using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.Middlewere
{
    public class RequestTimeMiddle : IMiddleware
    {
        private readonly ILogger<RequestTimeMiddle> _logger;
        private Stopwatch _time;

        public RequestTimeMiddle(ILogger<RequestTimeMiddle> logger)
        {
            _time = new Stopwatch();
            this._logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _time.Start();
            await next.Invoke(context);
            _time.Stop();

            var elapsedSec = _time.ElapsedMilliseconds;

            if(elapsedSec /1000 > 3)
            {
                var message = $"Request[{context.Request.Method}] at {context.Request.Path} took {elapsedSec}";
                _logger.LogInformation(message);
            }
        }
    }
}
