using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace WooliesXTechChallengeApi.Middlewares
{
	public class TracingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<TracingMiddleware> _logger;

		public TracingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
		{
			_next = next;
			_logger = loggerFactory?.CreateLogger<TracingMiddleware>();
		}

		public async Task InvokeAsync(HttpContext context)
		{
			context.Request.EnableBuffering(bufferLimit: 1024 * 1000);
			_logger.LogInformation("TracingMiddleware: this is a hit when Invoked");
			_logger.LogInformation($"TracingMiddleware: Request => {context.Request.Path.ToString()}");
			_logger.LogInformation($"TracingMiddleware: Method => {context.Request.Method}");
			_logger.LogInformation($"TracingMiddleware: Headers =>{JsonConvert.SerializeObject(context.Request.Headers.ToList(),Formatting.Indented)}");

			await this._next(context);

			if (context.Request.Method.ToUpper() == "POST")
			{
				using (var requestReader = new StreamReader(context.Request.Body, Encoding.UTF8))
				{ 
					var requestContent = await requestReader.ReadToEndAsync();
					_logger.LogInformation($"TracingMiddleware: Request Body => {requestContent}");
				}
			}
		}
	}
}
