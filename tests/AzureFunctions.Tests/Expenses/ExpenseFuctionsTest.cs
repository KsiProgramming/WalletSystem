//-----------------------------------------------------------------------
// <copyright file="ExpenseFuctionsTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.AzureFunctions.Tests
{
    using System.Net;
    using Azure.Core.Serialization;
    using FluentAssertions;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Extensions.Options;
    using Moq;

    public class ExpenseFuctionsTest
    {
        [Fact]
        public async Task CreateExpenseAsync()
        {
            var requestBody = Mock.Of<Stream>();

            var requestExenseCreation = new CreateExpenseRequestJson(
                amount: 12_23m,
                currencyCode: "USD",
                date: new DateTime(2024, 02, 11, 0, 0, 0, DateTimeKind.Utc),
                description: "This is description section.",
                expenseType: "Hotel",
                userId: 3);

            var manager = new Mock<IExpenseManager>(MockBehavior.Strict);
            manager
                .Setup(m => m.CreateAsync(It.IsAny<ExpenseCreation>()))
                .Callback((ExpenseCreation expenseCreation) =>
                {
                    expenseCreation.Amount.Should().Be(12_23m);
                    expenseCreation.Currency.Should().Be(Currency.USD);
                    expenseCreation.Date.Should().Be(new DateTime(2024, 02, 11, 0, 0, 0, DateTimeKind.Utc));
                    expenseCreation.Description.Should().Be("This is description section.");
                    expenseCreation.Type.Should().Be(ExpenseType.Hotel);
                    expenseCreation.UserId.Should().Be(3);
                })
                .Returns(Task.CompletedTask);

            var serializer = new Mock<ObjectSerializer>(MockBehavior.Strict);
            serializer
                .Setup(s => s.DeserializeAsync(requestBody, typeof(CreateExpenseRequestJson), CancellationToken.None))
                .ReturnsAsync(requestExenseCreation);

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

            var response = new Mock<HttpResponseData>(MockBehavior.Strict, functionContext.Object);
            response.SetupSet(r => r.StatusCode = HttpStatusCode.OK);

            var request = new Mock<HttpRequestData>(functionContext.Object);
            request
                .Setup(r => r.Body)
                .Returns(requestBody);
            request
                .Setup(r => r.CreateResponse())
                .Returns(response.Object);

            var expenseFunctions = new ExpenseFunctions(manager.Object);

            await expenseFunctions.CreateExpenseAsync(request.Object);

            request.VerifyAll();
            response.VerifyAll();
            manager.VerifyAll();
        }

        [Fact]
        public async Task SearchAsync()
        {
            var expenses = Array.Empty<Expense>();

            var responseBody = Mock.Of<Stream>(MockBehavior.Strict);

            var serializer = new Mock<ObjectSerializer>(MockBehavior.Strict);
            serializer.Setup(s => s.SerializeAsync(responseBody, expenses, typeof(SearchExpenseResponseJson[]), CancellationToken.None))
                .Returns(ValueTask.CompletedTask);

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

            var response = new Mock<HttpResponseData>(MockBehavior.Strict, functionContext.Object);
            response.SetupProperty(r => r.StatusCode, HttpStatusCode.OK);
            response.Setup(r => r.Body).Returns(responseBody);
            response.Setup(h => h.Headers)
                .Returns(new HttpHeadersCollection());

            var request = new Mock<HttpRequestData>(MockBehavior.Strict, functionContext.Object);
            request.Setup(r => r.Query)
                .Returns(new System.Collections.Specialized.NameValueCollection());
            request.Setup(r => r.CreateResponse())
                .Returns(response.Object);

            var manager = new Mock<IExpenseManager>(MockBehavior.Strict);
            manager.Setup(m => m.FindAsync(It.IsAny<ExpenseQuery>()))
                .Callback((ExpenseQuery query) =>
                {
                    query.Amount.Should().BeNull();
                    query.Date.Should().BeNull();
                    query.UserId.Should().BeNull();
                    query.SortBy.Should().BeNull();
                    query.SortOption.Should().BeNull();
                })
                .ReturnsAsync(expenses);

            var functions = new ExpenseFunctions(manager.Object);

            var result = await functions.SearchAsync(request.Object);

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Headers.Should().HaveCount(1);
            result.Headers.ElementAt(0).Key.Should().Be("Content-Type");
            result.Headers.ElementAt(0).Value.ElementAt(0).Should().Be("application/json; charset=utf-8");
            result.Body.Should().BeSameAs(responseBody);

            response.VerifyAll();
            request.VerifyAll();
            manager.VerifyAll();
        }

        [Fact]
        public async Task SearchAsync_WithUserId()
        {
            var expenses = Array.Empty<Expense>();

            var responseBody = Mock.Of<Stream>(MockBehavior.Strict);

            var serializer = new Mock<ObjectSerializer>(MockBehavior.Strict);
            serializer.Setup(s => s.SerializeAsync(responseBody, expenses, typeof(SearchExpenseResponseJson[]), CancellationToken.None))
                .Returns(ValueTask.CompletedTask);

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

            var response = new Mock<HttpResponseData>(MockBehavior.Strict, functionContext.Object);
            response.SetupProperty(r => r.StatusCode, HttpStatusCode.OK);
            response.Setup(r => r.Body).Returns(responseBody);
            response.Setup(h => h.Headers)
                .Returns(new HttpHeadersCollection());

            var request = new Mock<HttpRequestData>(MockBehavior.Strict, functionContext.Object);
            request.Setup(r => r.Query)
                .Returns(new System.Collections.Specialized.NameValueCollection() { { "userId", "5" } });
            request.Setup(r => r.CreateResponse())
                .Returns(response.Object);

            var manager = new Mock<IExpenseManager>(MockBehavior.Strict);
            manager.Setup(m => m.FindAsync(It.IsAny<ExpenseQuery>()))
                .Callback((ExpenseQuery query) =>
                {
                    query.Amount.Should().BeNull();
                    query.Date.Should().BeNull();
                    query.UserId.Should().Be(5);
                    query.SortBy.Should().BeNull();
                    query.SortOption.Should().BeNull();
                })
                .ReturnsAsync(expenses);

            var functions = new ExpenseFunctions(manager.Object);

            var result = await functions.SearchAsync(request.Object);

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Headers.Should().HaveCount(1);
            result.Headers.ElementAt(0).Key.Should().Be("Content-Type");
            result.Headers.ElementAt(0).Value.ElementAt(0).Should().Be("application/json; charset=utf-8");
            result.Body.Should().BeSameAs(responseBody);

            response.VerifyAll();
            request.VerifyAll();
            manager.VerifyAll();
        }

        [Fact]
        public async Task SearchAsync_WithSortBy()
        {
            var expenses = Array.Empty<Expense>();

            var responseBody = Mock.Of<Stream>(MockBehavior.Strict);

            var serializer = new Mock<ObjectSerializer>(MockBehavior.Strict);
            serializer.Setup(s => s.SerializeAsync(responseBody, expenses, typeof(SearchExpenseResponseJson[]), CancellationToken.None))
                .Returns(ValueTask.CompletedTask);

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

            var response = new Mock<HttpResponseData>(MockBehavior.Strict, functionContext.Object);
            response.SetupProperty(r => r.StatusCode, HttpStatusCode.OK);
            response.Setup(r => r.Body).Returns(responseBody);
            response.Setup(h => h.Headers)
                .Returns(new HttpHeadersCollection());

            var request = new Mock<HttpRequestData>(MockBehavior.Strict, functionContext.Object);
            request.Setup(r => r.Query)
                .Returns(new System.Collections.Specialized.NameValueCollection() { { "sortBy", "amount" } });
            request.Setup(r => r.CreateResponse())
                .Returns(response.Object);

            var manager = new Mock<IExpenseManager>(MockBehavior.Strict);
            manager.Setup(m => m.FindAsync(It.IsAny<ExpenseQuery>()))
                .Callback((ExpenseQuery query) =>
                {
                    query.Amount.Should().BeNull();
                    query.Date.Should().BeNull();
                    query.UserId.Should().BeNull();
                    query.SortBy.Should().Be(SortBy.Amount);
                    query.SortOption.Should().BeNull();
                })
                .ReturnsAsync(expenses);

            var functions = new ExpenseFunctions(manager.Object);

            var result = await functions.SearchAsync(request.Object);

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Headers.Should().HaveCount(1);
            result.Headers.ElementAt(0).Key.Should().Be("Content-Type");
            result.Headers.ElementAt(0).Value.ElementAt(0).Should().Be("application/json; charset=utf-8");
            result.Body.Should().BeSameAs(responseBody);

            response.VerifyAll();
            request.VerifyAll();
            manager.VerifyAll();
        }

        [Fact]
        public async Task SearchAsync_WithSortOption()
        {
            var expenses = Array.Empty<Expense>();

            var responseBody = Mock.Of<Stream>(MockBehavior.Strict);

            var serializer = new Mock<ObjectSerializer>(MockBehavior.Strict);
            serializer.Setup(s => s.SerializeAsync(responseBody, expenses, typeof(SearchExpenseResponseJson[]), CancellationToken.None))
                .Returns(ValueTask.CompletedTask);

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

            var response = new Mock<HttpResponseData>(MockBehavior.Strict, functionContext.Object);
            response.SetupProperty(r => r.StatusCode, HttpStatusCode.OK);
            response.Setup(r => r.Body).Returns(responseBody);
            response.Setup(h => h.Headers)
                .Returns(new HttpHeadersCollection());

            var request = new Mock<HttpRequestData>(MockBehavior.Strict, functionContext.Object);
            request.Setup(r => r.Query)
                .Returns(new System.Collections.Specialized.NameValueCollection() { { "sortOption", "ASC" } });
            request.Setup(r => r.CreateResponse())
                .Returns(response.Object);

            var manager = new Mock<IExpenseManager>(MockBehavior.Strict);
            manager.Setup(m => m.FindAsync(It.IsAny<ExpenseQuery>()))
                .Callback((ExpenseQuery query) =>
                {
                    query.Amount.Should().BeNull();
                    query.Date.Should().BeNull();
                    query.UserId.Should().BeNull();
                    query.SortBy.Should().BeNull();
                    query.SortOption.Should().Be("ASC");
                })
                .ReturnsAsync(expenses);

            var functions = new ExpenseFunctions(manager.Object);

            var result = await functions.SearchAsync(request.Object);

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Headers.Should().HaveCount(1);
            result.Headers.ElementAt(0).Key.Should().Be("Content-Type");
            result.Headers.ElementAt(0).Value.ElementAt(0).Should().Be("application/json; charset=utf-8");
            result.Body.Should().BeSameAs(responseBody);

            response.VerifyAll();
            request.VerifyAll();
            manager.VerifyAll();
        }
    }
}
