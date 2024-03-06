//-----------------------------------------------------------------------
// <copyright file="UserManager.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Users
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository repository;

        public UserManager(IUserRepository userRepository)
        {
            this.repository = userRepository;
        }

        public async Task<User> FindByIdAsync(int id)
        {
            var query = new UserQuery()
            {
                Id = id,
            };

            var users = await this.repository.FindAsync(query);
            if (!users.Any())
            {
                throw new UserNotFoundException($"No user has been found with the specified '{id}' identifier.");
            }

            return users.Single();
        }
    }
}
