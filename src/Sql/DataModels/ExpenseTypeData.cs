//-----------------------------------------------------------------------
// <copyright file="ExpenseTypeData.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Sql
{
    public class ExpenseTypeData
    {
        public ExpenseTypeData(string label)
        {
            this.Label = label;
        }

        public int Id { get; set; }

        public string Label { get; }
    }
}
