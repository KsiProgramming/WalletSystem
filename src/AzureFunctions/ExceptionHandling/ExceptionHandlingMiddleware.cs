//-----------------------------------------------------------------------
// <copyright file="ExceptionHandlingMiddleware.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.AzureFunctions
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Azure.Functions.Worker.Middleware;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    [ExcludeFromCodeCoverage]
    public class ExceptionHandlingMiddleware : IFunctionsWorkerMiddleware
    {
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly
        private static readonly Dictionary<int, (string Type, string Title)> DefaultsErrorCodes = new ()
        {
            [400] =
            (
                "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                "Bad Request"
            ),
            [401] =
            (
                "https://tools.ietf.org/html/rfc9110#section-15.5.2",
                "Unauthorized"
            ),
            [403] =
            (
                "https://tools.ietf.org/html/rfc9110#section-15.5.4",
                "Forbidden"
            ),
            [404] =
            (
                "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                "Not Found"
            ),
            [405] =
            (
                "https://tools.ietf.org/html/rfc9110#section-15.5.6",
                "Method Not Allowed"
            ),
            [406] =
            (
                "https://tools.ietf.org/html/rfc9110#section-15.5.7",
                "Not Acceptable"
            ),
            [408] =
            (
                "https://tools.ietf.org/html/rfc9110#section-15.5.9",
                "Request Timeout"
            ),
            [409] =
            (
                "https://tools.ietf.org/html/rfc9110#section-15.5.10",
                "Conflict"
            ),
            [412] =
            (
                "https://tools.ietf.org/html/rfc9110#section-15.5.13",
                "Precondition Failed"
            ),
            [415] =
            (
                "https://tools.ietf.org/html/rfc9110#section-15.5.16",
                "Unsupported Media Type"
            ),
            [422] =
            (
                "https://tools.ietf.org/html/rfc4918#section-11.2",
                "Unprocessable Entity"
            ),
            [426] =
            (
                "https://tools.ietf.org/html/rfc9110#section-15.5.22",
                "Upgrade Required"
            ),
            [500] =
            (
                "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                "An error occurred while processing your request."
            ),
            [502] =
            (
                "https://tools.ietf.org/html/rfc9110#section-15.6.3",
                "Bad Gateway"
            ),
            [503] =
            (
                "https://tools.ietf.org/html/rfc9110#section-15.6.4",
                "Service Unavailable"
            ),
            [504] =
            (
                "https://tools.ietf.org/html/rfc9110#section-15.6.5",
                "Gateway Timeout"
            ),
        };
#pragma warning restore SA1009 // Closing parenthesis should be spaced correctly

        private readonly ProblemDetailsOptions options;

        private readonly IHostEnvironment hostEnvironment;

        public ExceptionHandlingMiddleware(IOptions<ProblemDetailsOptions> options, IHostEnvironment hostEnvironment)
        {
            this.options = options.Value;
            this.hostEnvironment = hostEnvironment;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                var request = await context.GetHttpRequestDataAsync();

                if (request is null)
                {
                    throw;
                }

                var problemDetails = this.options.TryConvert(exception!);
                if (problemDetails is null)
                {
                    // If no converter was found raise a error 500.
                    string message;

                    if (this.hostEnvironment.IsDevelopment())
                    {
                        message = exception.ToString();
                    }
                    else
                    {
                        message = "An internal error has occurred. Please contact the support of the Active Assurances for more informations.";
                    }

                    problemDetails = new ProblemDetails()
                    {
                        Title = "InternalServerError",
                        Detail = message,
                    };
                }

                // Gets the default title and type.
                var statusCode = problemDetails.Status.GetValueOrDefault(500);

                if (DefaultsErrorCodes.TryGetValue(statusCode, out var defaultTitleAndType))
                {
                    if (problemDetails.Title is null)
                    {
                        problemDetails.Title = defaultTitleAndType.Title;
                    }

                    if (problemDetails.Type is null)
                    {
                        problemDetails.Type = defaultTitleAndType.Type;
                    }
                }

                // From the https://github.com/Azure/azure-functions-dotnet-worker/blob/main/samples/CustomMiddleware/ExceptionHandlingMiddleware.cs example.
                var response = request.CreateResponse((HttpStatusCode)statusCode);

                await response.WriteAsJsonAsync(problemDetails, "application/problem+json", response.StatusCode);

                var invocationResult = context.GetInvocationResult();

                var httpOutputBindingFromMultipleOutputBindings = context.GetOutputBindings<HttpResponseData>()
                    .FirstOrDefault(b => b.BindingType == "http" && b.Name != "$return");

                if (httpOutputBindingFromMultipleOutputBindings is not null)
                {
                    httpOutputBindingFromMultipleOutputBindings.Value = response;
                }
                else
                {
                    invocationResult.Value = response;
                }
            }
        }
    }
}
