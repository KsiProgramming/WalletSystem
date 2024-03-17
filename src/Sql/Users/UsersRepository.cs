//-----------------------------------------------------------------------
// <copyright file="UsersRepository.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Users.Sql
{
    using Microsoft.EntityFrameworkCore;
    using WalletSystem.Sql;

    public class UsersRepository : IUserRepository
    {
        private readonly WalletSystemDbContext context;

        public UsersRepository(WalletSystemDbContext context)
        {
            this.context = context;
        }

        public async Task<IReadOnlyCollection<User>> FindAsync(UserQuery query)
        {
            var users = this.context.Users
                .Include(x => x.Currency)
                .AsNoTracking();

            if (query.Id.HasValue)
            {
                users = users.Where(u => u.Id == query.Id);
            }

            var result = await users.ToArrayAsync();

            return result
                .Select(BuildUser)
                .ToArray();
        }

        private static User BuildUser(UserData model)
        {
            return new User(
                firstName: model.FirstName,
                lastName: model.LastName,
                (Currency)model.Currency!.Id,
                id: model.Id);
        }
    }
}
