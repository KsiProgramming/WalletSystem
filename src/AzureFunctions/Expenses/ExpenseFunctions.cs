//-----------------------------------------------------------------------
// <copyright file="ExpenseFunctions.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.AzureFunctions
{
    using System.Net;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;

    public class ExpenseFunctions
    {
        private readonly IExpenseManager manager;

        public ExpenseFunctions(IExpenseManager manager)
        {
            this.manager = manager;
        }

        [Function("CreateExpense")]
        public async Task<HttpResponseData> CreateExpenseAsync([HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "api/Expenses/Create")] HttpRequestData req)
        {
            var expenseRequest = await req.ReadFromJsonAsync<CreateExpenseRequestJson>();

            var expenseCreation = new ExpenseCreation(
                amount: expenseRequest!.Amount,
                currency: (Currency)Enum.Parse(typeof(Currency), expenseRequest.CurrencyCode),
                date: expenseRequest.Date,
                description: expenseRequest.Description,
                userId: expenseRequest.UserId,
                type: (ExpenseType)Enum.Parse(typeof(ExpenseType), expenseRequest.ExpenseType));

            await this.manager.CreateAsync(expenseCreation);

            return req.CreateResponse(HttpStatusCode.OK);
        }

        [Function("SearchExpenses")]
        public async Task<HttpResponseData> SearchAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/Expenses/search")] HttpRequestData req)
        {
            var query = new ExpenseQuery()
            {
                UserId = int.TryParse(req.Query["UserId"], out var parsedResult) ? parsedResult : null,
                SortBy = Enum.TryParse(req.Query["SortBy"], true, out SortBy sortBy) ? sortBy : null,
                SortOption = req.Query["SortOption"],
            };

            var expenses = await this.manager.FindAsync(query);

            var result = expenses
                .Select(BuildSearchExpenseResponsJson)
                .ToArray();

            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }

        private static SearchExpenseResponseJson BuildSearchExpenseResponsJson(Expense expense)
        {
            return new SearchExpenseResponseJson(
                amountValue: expense.Amount.Value,
                amountCurrency: expense.Amount.Currency.ToString(),
                date: expense.Date,
                description: expense.Description,
                type: expense.Type.ToString(),
                userFulleName: expense.User);
        }
    }
}
