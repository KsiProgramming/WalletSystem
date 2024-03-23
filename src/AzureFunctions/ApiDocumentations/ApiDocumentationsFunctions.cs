//-----------------------------------------------------------------------
// <copyright file="ApiDocumentationsFunctions.cs" company="WalletSystem">
// Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.AzureFunctions
{
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;

    public static class ApiDocumentationsFunctions
    {
        [Function("ApiDocumentations")]
        public static async Task<HttpResponseData> GetApiDocumentationsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/api.yaml")] HttpRequestData req)
        {
            using var apiDocumentation = File.OpenRead("ApiDocumentations/API.yaml");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/yaml; charset=utf-8");

            await apiDocumentation.CopyToAsync(response.Body);

            return response;
        }

        [Function("ApiDocumentationsUI")]
        public static async Task<HttpResponseData> GetApiDocumentationsUIAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api")] HttpRequestData req)
        {
            using var swaggerUI = File.OpenRead("ApiDocumentations/swagger-ui.html");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/html; charset=utf-8");

            await swaggerUI.CopyToAsync(response.Body);

            return response;
        }
    }
}
