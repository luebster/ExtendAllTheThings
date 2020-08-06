using Microsoft.AspNetCore.Http;

using System;
using System.Net;
using System.Threading.Tasks;

namespace ExtendAllTheThings.Exceptions
{
	public class StatusCodeException : Exception
	{
		public StatusCodeException(HttpStatusCode statusCode)
		{
			StatusCode = statusCode;
		}

		public HttpStatusCode StatusCode { get; set; }

		public StatusCodeException()
		{
		}

		public StatusCodeException(string message) : base(message)
		{
		}

		public StatusCodeException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

	public class StatusCodeExceptionHandler
	{
		private readonly RequestDelegate _request;

		public StatusCodeExceptionHandler(RequestDelegate pipeline)
		{
			_request = pipeline;
		}

		public Task Invoke(HttpContext context)
		{
			return InvokeAsync(context); // Stops VS from nagging about async method without ...Async suffix.
		}

		private async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _request(context);
			}
			catch (StatusCodeException exception)
			{
				context.Response.StatusCode = (int)exception.StatusCode;
				context.Response.Headers.Clear();
			}
		}
	}
}