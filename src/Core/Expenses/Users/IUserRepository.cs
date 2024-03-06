//-----------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Users
{
    public interface IUserRepository
    {
        Task<IReadOnlyCollection<User>> FindAsync(UserQuery query);
    }
}