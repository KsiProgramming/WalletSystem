//-----------------------------------------------------------------------
// <copyright file="UserFunctionsTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Users.AzureFunctions.Tests
{
    using System.Collections.Specialized;
    using System.Net;
    using Azure.Core.Serialization;
    using FluentAssertions;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Extensions.Options;
    using Moq;

    public class UserFunctionsTest
    {
        [Fact]
        public async Task SearchAsync_WithName()
        {
            var users = new[]
            {
                new User(firstName: "John", lastName: "Doe", currency: Currency.USD, id: 1),
                new User(firstName: "Johnny", lastName: "Smith", currency: Currency.RUB, id: 2),
            };

            var responseBody = Mock.Of<Stream>(MockBehavior.Strict);

            object? serialized = null;
            var serializer = new Mock<ObjectSerializer>(MockBehavior.Strict);
            serializer.Setup(s => s.SerializeAsync(responseBody, It.IsAny<object?>(), typeof(SearchUserResponseJson[]), CancellationToken.None))
                .Callback((Stream stream, object? value, Type type, CancellationToken token) => serialized = value)
                .Returns(ValueTask.CompletedTask);

            var functionContext = BuildFunctionContext(serializer);
            var response = BuildResponse(functionContext, responseBody);
            var request = BuildRequest(functionContext, response, new NameValueCollection() { { "name", "john" } });

            var manager = new Mock<IUserManager>(MockBehavior.Strict);
            manager.Setup(m => m.FindByNameAsync("john"))
                .ReturnsAsync(users);

            var functions = new UserFunctions(manager.Object);

            var result = await functions.SearchAsync(request.Object);

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Headers.Should().HaveCount(1);
            result.Headers.ElementAt(0).Key.Should().Be("Content-Type");
            result.Headers.ElementAt(0).Value.ElementAt(0).Should().Be("application/json; charset=utf-8");
            result.Body.Should().BeSameAs(responseBody);

            var items = serialized.Should().BeOfType<SearchUserResponseJson[]>().Subject;
            items.Should().HaveCount(2);
            items[0].Id.Should().Be(1);
            items[0].FirstName.Should().Be("John");
            items[0].LastName.Should().Be("Doe");
            items[0].Currency.Should().Be("USD");
            items[1].Id.Should().Be(2);
            items[1].FirstName.Should().Be("Johnny");
            items[1].LastName.Should().Be("Smith");
            items[1].Currency.Should().Be("RUB");

            serializer.VerifyAll();
            response.VerifyAll();
            request.VerifyAll();
            manager.VerifyAll();
        }

        [Fact]
        public async Task SearchAsync_EmptyResult()
        {
            var responseBody = Mock.Of<Stream>(MockBehavior.Strict);

            object? serialized = null;
            var serializer = new Mock<ObjectSerializer>(MockBehavior.Strict);
            serializer.Setup(s => s.SerializeAsync(responseBody, It.IsAny<object?>(), typeof(SearchUserResponseJson[]), CancellationToken.None))
                .Callback((Stream stream, object? value, Type type, CancellationToken token) => serialized = value)
                .Returns(ValueTask.CompletedTask);

            var functionContext = BuildFunctionContext(serializer);
            var response = BuildResponse(functionContext, responseBody);
            var request = BuildRequest(functionContext, response, new NameValueCollection() { { "name", "unknown" } });

            var manager = new Mock<IUserManager>(MockBehavior.Strict);
            manager.Setup(m => m.FindByNameAsync("unknown"))
                .ReturnsAsync(Array.Empty<User>());

            var functions = new UserFunctions(manager.Object);

            var result = await functions.SearchAsync(request.Object);

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Headers.Should().HaveCount(1);
            result.Headers.ElementAt(0).Key.Should().Be("Content-Type");
            result.Headers.ElementAt(0).Value.ElementAt(0).Should().Be("application/json; charset=utf-8");
            result.Body.Should().BeSameAs(responseBody);

            serialized.Should().BeOfType<SearchUserResponseJson[]>().Which.Should().BeEmpty();

            serializer.VerifyAll();
            response.VerifyAll();
            request.VerifyAll();
            manager.VerifyAll();
        }

        [Fact]
        public async Task SearchAsync_MissingName()
        {
            var serializer = new Mock<ObjectSerializer>(MockBehavior.Strict);

            var functionContext = BuildFunctionContext(serializer);
            var response = new Mock<HttpResponseData>(MockBehavior.Strict, functionContext.Object);
            response.SetupProperty(r => r.StatusCode, HttpStatusCode.OK);
            var request = BuildRequest(functionContext, response, new NameValueCollection());

            var manager = new Mock<IUserManager>(MockBehavior.Strict);

            var functions = new UserFunctions(manager.Object);

            var result = await functions.SearchAsync(request.Object);

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            response.VerifyAll();
            request.VerifyAll();
            manager.Verify(m => m.FindByNameAsync(It.IsAny<string>()), Times.Never);
            manager.VerifyAll();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public async Task SearchAsync_BlankName(string name)
        {
            var serializer = new Mock<ObjectSerializer>(MockBehavior.Strict);

            var functionContext = BuildFunctionContext(serializer);
            var response = new Mock<HttpResponseData>(MockBehavior.Strict, functionContext.Object);
            response.SetupProperty(r => r.StatusCode, HttpStatusCode.OK);
            var request = BuildRequest(functionContext, response, new NameValueCollection() { { "name", name } });

            var manager = new Mock<IUserManager>(MockBehavior.Strict);

            var functions = new UserFunctions(manager.Object);

            var result = await functions.SearchAsync(request.Object);

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            response.VerifyAll();
            request.VerifyAll();
            manager.Verify(m => m.FindByNameAsync(It.IsAny<string>()), Times.Never);
            manager.VerifyAll();
        }

        private static Mock<FunctionContext> BuildFunctionContext(Mock<ObjectSerializer> serializer)
        {
            var workerOptions = Options.Create(new WorkerOptions()
            {
                Serializer = serializer.Object,
            });

            var serviceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            serviceProvider.Setup(sp => sp.GetService(typeof(IOptions<WorkerOptions>)))
                .Returns(workerOptions);

            var functionContext = new Mock<FunctionContext>(MockBehavior.Strict);
            functionContext.Setup(fc => fc.InstanceServices)
                .Returns(serviceProvider.Object);

            return functionContext;
        }

        private static Mock<HttpResponseData> BuildResponse(Mock<FunctionContext> functionContext, Stream responseBody)
        {
            var response = new Mock<HttpResponseData>(MockBehavior.Strict, functionContext.Object);
            response.SetupProperty(r => r.StatusCode, HttpStatusCode.OK);
            response.Setup(r => r.Body).Returns(responseBody);
            response.Setup(h => h.Headers)
                .Returns(new HttpHeadersCollection());

            return response;
        }

        private static Mock<HttpRequestData> BuildRequest(Mock<FunctionContext> functionContext, Mock<HttpResponseData> response, NameValueCollection query)
        {
            var request = new Mock<HttpRequestData>(MockBehavior.Strict, functionContext.Object);
            request.Setup(r => r.Query)
                .Returns(query);
            request.Setup(r => r.CreateResponse())
                .Returns(response.Object);

            return request;
        }
    }
}
