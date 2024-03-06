//-----------------------------------------------------------------------
// <copyright file="SystemClockTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem
{
    public class SystemClockTest
    {
        [Fact]
        public void UtcNow()
        {
            var systemClock = new SystemClock();

            systemClock.UtcNow.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }
    }
}
