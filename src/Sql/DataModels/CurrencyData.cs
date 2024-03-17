//-----------------------------------------------------------------------
// <copyright file="CurrencyData.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.Sql
{
    public class CurrencyData
    {
        public CurrencyData(string code, string name, string symbol)
        {
            this.Code = code;
            this.Name = name;
            this.Symbol = symbol;
        }

        public int Id { get; set; }

        public string Code { get; }

        public string Name { get; }

        public string Symbol { get; }
    }
}
