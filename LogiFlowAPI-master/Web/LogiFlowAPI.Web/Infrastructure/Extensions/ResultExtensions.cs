namespace LogiFlowAPI.Web.Infrastructure.Extensions
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;

    using LogiFlowAPI.Services.Common.Result;

    using Microsoft.AspNetCore.Mvc;

    public static class ResultExtensions
    {
        /// <summary>
        /// Converts a <see cref="Result"/> to an <see cref="ActionResult"/>.
        /// The HTTP status code is inferred from the <see cref="Result"/>'s application status code.
        /// </summary>
        /// <remarks>
        /// If the application status code is invalid, default values will be used: 200 for success and 500 for error.
        /// An application status code property is automatically appended to the response.
        /// </remarks>
        /// <param name="result">The result to convert.</param>
        /// <returns>An <see cref="ActionResult"/> representing the <see cref="Result"/>.</returns>
        /// <exception cref="Exception">Thrown if an unsupported <see cref="Result"/> type is provided.</exception>
        public static ActionResult ToActionResult<T>(this Result<T> result)
        {
            var httpCodes = Enum.GetValues(typeof(HttpStatusCode)).Cast<int>();

            int? httpStatusCode = httpCodes.Contains(result.StatusCode) ? result.StatusCode : null;

            if (result.IsSuccess)
            {
                object resultObject = new
                {
                    Status = result.StatusCode,
                    Data = result.Value
                };

                return new JsonResult(resultObject)
                {
                    StatusCode = httpStatusCode ?? (int)HttpStatusCode.OK
                };
            }
            else
            {
                var problemDetails = new ProblemDetails()
                {
                    Status = result.StatusCode, // Application status code
                    Extensions = { { "traceId", Activity.Current?.Id } },
                };

                problemDetails.Title = result.ErrorMessage;

                return new ObjectResult(problemDetails)
                {
                    StatusCode = httpStatusCode ?? (int)HttpStatusCode.InternalServerError,
                    ContentTypes = { "application/problem+json" }
                };
            }
        }

        public static ActionResult ToActionResult(this Result result)
        {
            return Result<object>.ToGenericResult(result).ToActionResult();
        }
    }
}
