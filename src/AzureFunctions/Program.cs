//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem
{
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using WalletSystem.AzureFunctions;
    using WalletSystem.Expenses.AzureFunctions;
    using WalletSystem.Expenses.RulesValidator;
    using WalletSystem.Expenses.Users;
    using WalletSystem.Expenses.Users.AzureFunctions;

    public static class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWebApplication(builder =>
                {
                    builder.UseMiddleware<ExceptionHandlingMiddleware>();
                })
                .ConfigureServices((ctx, services) =>
                {
                    services.AddApplicationInsightsTelemetryWorkerService();
                    services.ConfigureFunctionsApplicationInsights();
                    services.AddPersistence(ctx.Configuration);
                    services.AddCore();
                    services.AddSingleton<IExceptionProblemDetailsConverter<ExpenseCurrencyMissMatchException>, ExpenseCurrencyMissMatchExceptionProblemDetailsConverter>();
                    services.AddSingleton<IExceptionProblemDetailsConverter<ExpenseDateOlderThanThreeMonthsException>, ExpenseDateOlderThanThreeMonthsExceptionProblemDetailsConverter>();
                    services.AddSingleton<IExceptionProblemDetailsConverter<ExpenseDateInFutureException>, ExpenseDateInFutureExceptionProblemDetailsConverter>();
                    services.AddSingleton<IExceptionProblemDetailsConverter<ExpenseDuplicateFoundException>, ExpenseDuplicateFoundExceptionProblemDetailsConverter>();
                    services.AddSingleton<IExceptionProblemDetailsConverter<ExpenseMissingDescriptionException>, ExpenseMissingDescriptionExceptionProblemDetailsConverter>();
                    services.AddSingleton<IExceptionProblemDetailsConverter<UserNotFoundException>, UserNotFoundExceptionProblemDetailsConverter>();

                    services.Configure<ProblemDetailsOptions>(opt =>
                    {
                        opt.AddExceptionConverter(ExpenseCurrencyMissMatchExceptionProblemDetailsConverter.Instance);
                        opt.AddExceptionConverter(ExpenseDateOlderThanThreeMonthsExceptionProblemDetailsConverter.Instance);
                        opt.AddExceptionConverter(ExpenseDateInFutureExceptionProblemDetailsConverter.Instance);
                        opt.AddExceptionConverter(ExpenseDuplicateFoundExceptionProblemDetailsConverter.Instance);
                        opt.AddExceptionConverter(ExpenseMissingDescriptionExceptionProblemDetailsConverter.Instance);
                        opt.AddExceptionConverter(UserNotFoundExceptionProblemDetailsConverter.Instance);
                    });
                })
                .Build();

            using var scope = host.Services.CreateScope();
            scope.ServiceProvider.InitialzeDatabase();

            host.Run();
        }
    }
}
