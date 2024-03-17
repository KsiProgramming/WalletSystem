//-----------------------------------------------------------------------
// <copyright file="CompositionRoot.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using WalletSystem.Expenses;
    using WalletSystem.Expenses.Sql;
    using WalletSystem.Expenses.Users;
    using WalletSystem.Expenses.Users.Sql;
    using WalletSystem.Sql;

    public static class CompositionRoot
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(config["WalletSystem_SQL_CONNECTIONSTRING"]))
            {
                throw new InvalidOperationException("The WalletSystem connectionstring must be specified. (Missing setting: 'WalletSystem_SQL_CONNECTIONSTRING')");
            }

            services.AddDbContext<WalletSystemDbContext>(options =>
            options.UseSqlServer(config["WalletSystem_SQL_CONNECTIONSTRING"]));

            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IUserRepository, UsersRepository>();
            return services;
        }

        public static void InitialzeDatabase(this IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<WalletSystemDbContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}
