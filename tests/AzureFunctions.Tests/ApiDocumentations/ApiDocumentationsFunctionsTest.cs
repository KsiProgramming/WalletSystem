//-----------------------------------------------------------------------
// <copyright file="ApiDocumentationsFunctionsTest.cs" company="WalletSystem">
// Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem
{
    using System.Net;
    using System.Text;
    using FluentAssertions;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Moq;
    using WalletSystem.AzureFunctions;

    public class ApiDocumentationsFunctionsTest
    {
        [Fact]
        public async Task GetApiDocumentationsAsync()
        {
            var body = new MemoryStream();

            var context = Mock.Of<FunctionContext>();

            var response = new Mock<HttpResponseData>(MockBehavior.Strict, context);
            response.Setup(r => r.Headers)
                .Returns(new HttpHeadersCollection());
            response.Setup(r => r.Body)
                .Returns(body);
            response.SetupSet(r => r.StatusCode = HttpStatusCode.OK);

            var request = new Mock<HttpRequestData>(MockBehavior.Strict, context);
            request.Setup(r => r.CreateResponse())
                .Returns(response.Object);

            var result = await ApiDocumentationsFunctions.GetApiDocumentationsAsync(request.Object);

            result.Headers.GetValues("Content-Type").Single().Should().Be("text/yaml; charset=utf-8");

            var bodyAsText = Encoding.UTF8.GetString(body.ToArray()).TrimStart('\uFEFF');
            bodyAsText.Should().Be(File.ReadAllText("ApiDocumentations/API.yaml"));

            response.VerifyAll();
            request.VerifyAll();
        }

        [Fact]
        public async Task GetApiDocumentationsUIAsync()
        {
            var body = new MemoryStream();

            var context = Mock.Of<FunctionContext>();

            var response = new Mock<HttpResponseData>(MockBehavior.Strict, context);
            response.Setup(r => r.Headers)
                .Returns(new HttpHeadersCollection());
            response.Setup(r => r.Body)
                .Returns(body);
            response.SetupSet(r => r.StatusCode = HttpStatusCode.OK);

            var request = new Mock<HttpRequestData>(MockBehavior.Strict, context);
            request.Setup(r => r.CreateResponse())
                .Returns(response.Object);

            var result = await ApiDocumentationsFunctions.GetApiDocumentationsUIAsync(request.Object);

            result.Headers.GetValues("Content-Type").Single().Should().Be("text/html; charset=utf-8");

            var bodyAsText = Encoding.UTF8.GetString(body.ToArray()).TrimStart('\uFEFF');
            bodyAsText.Should().Be(File.ReadAllText("ApiDocumentations/swagger-ui.html"));

            response.VerifyAll();
            request.VerifyAll();
        }
    }
}
