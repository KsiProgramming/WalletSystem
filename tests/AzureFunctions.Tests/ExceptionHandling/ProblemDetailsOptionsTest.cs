//-----------------------------------------------------------------------
// <copyright file="ProblemDetailsOptionsTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.AzureFunctions.Tests
{
    using Moq;

    public class ProblemDetailsOptionsTest
    {
        [Fact]
        public void TryConvert()
        {
            var formatException = new FormatException();
            var problemDetails = new ProblemDetails();

            var converter = new Mock<IExceptionProblemDetailsConverter<FormatException>>(MockBehavior.Strict);
            converter.Setup(c => c.Convert(formatException))
                .Returns(problemDetails);

            var options = new ProblemDetailsOptions();

            options.AddExceptionConverter(converter.Object);

            options.TryConvert(formatException).Should().Be(problemDetails);
            options.TryConvert(formatException).Should().Be(problemDetails);
            options.TryConvert(new AppDomainUnloadedException()).Should().BeNull();
        }

        [Fact]
        public void AddExceptionConverter_AlreadyRegistered()
        {
            var converter1 = new Mock<IExceptionProblemDetailsConverter<FormatException>>(MockBehavior.Strict);
            var converter2 = new Mock<IExceptionProblemDetailsConverter<FormatException>>(MockBehavior.Strict);

            var options = new ProblemDetailsOptions();

            options.AddExceptionConverter(converter1.Object);

            options.Invoking(o => o.AddExceptionConverter(converter2.Object)).Should().ThrowExactly<ArgumentException>()
                .WithMessage("A converter has already been registered for the 'FormatException' exception type. (Parameter 'converter')")
                .And.ParamName.Should().Be("converter");
        }
    }
}
