//-----------------------------------------------------------------------
// <copyright file="UserFunctions.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Users.AzureFunctions
{
    using System.Net;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;

    public class UserFunctions
    {
        private readonly IUserManager manager;

        public UserFunctions(IUserManager manager)
        {
            this.manager = manager;
        }

        [Function("SearchUsers")]
        public async Task<HttpResponseData> SearchAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/Users/search")] HttpRequestData req)
        {
            var name = req.Query["name"];

            if (string.IsNullOrWhiteSpace(name))
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var users = await this.manager.FindByNameAsync(name);

            var result = users
                .Select(BuildSearchUserResponseJson)
                .ToArray();

            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }

        private static SearchUserResponseJson BuildSearchUserResponseJson(User user)
        {
            return new SearchUserResponseJson(
                id: user.Id,
                firstName: user.FirstName,
                lastName: user.LastName,
                currency: user.Currency.ToString());
        }
    }
}
