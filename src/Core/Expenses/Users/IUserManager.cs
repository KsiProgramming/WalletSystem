//-----------------------------------------------------------------------
// <copyright file="IUserManager.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Expenses.Users
{
    public interface IUserManager
    {
        Task<User> FindByIdAsync(int id);
    }
}