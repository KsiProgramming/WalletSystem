//-----------------------------------------------------------------------
// <copyright file="ISystemClock.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem
{
    public interface ISystemClock
    {
        DateTimeOffset UtcNow { get; }
    }
}
