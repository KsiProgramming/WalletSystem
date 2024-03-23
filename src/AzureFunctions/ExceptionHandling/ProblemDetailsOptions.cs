//-----------------------------------------------------------------------
// <copyright file="ProblemDetailsOptions.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.AzureFunctions
{
    public class ProblemDetailsOptions
    {
        private readonly IDictionary<Type, IExceptionProblemDetailsConverter> converters;

        public ProblemDetailsOptions()
        {
            this.converters = new Dictionary<Type, IExceptionProblemDetailsConverter>();
        }

        private interface IExceptionProblemDetailsConverter
        {
            ProblemDetails Convert(Exception exception);
        }

        public void AddExceptionConverter<TException>(IExceptionProblemDetailsConverter<TException> converter)
            where TException : Exception
        {
            if (this.converters.ContainsKey(typeof(TException)))
            {
                throw new ArgumentException($"A converter has already been registered for the '{typeof(TException).Name}' exception type.", nameof(converter));
            }

            this.converters.Add(typeof(TException), new Converter<TException>(converter));
        }

        public ProblemDetails? TryConvert(Exception exception)
        {
            if (this.converters.TryGetValue(exception.GetType(), out var converter))
            {
                return converter.Convert(exception);
            }

            return null;
        }

        private sealed class Converter<TException> : IExceptionProblemDetailsConverter
            where TException : Exception
        {
            private readonly IExceptionProblemDetailsConverter<TException> converter;

            public Converter(IExceptionProblemDetailsConverter<TException> converter)
            {
                this.converter = converter;
            }

            public ProblemDetails Convert(Exception exception)
            {
                return this.converter.Convert((TException)exception);
            }
        }
    }
}
