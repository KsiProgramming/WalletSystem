//-----------------------------------------------------------------------
// <copyright file="ProblemDetailsTest.cs" company="WalletSystem">
//     Copyright (c) WalletSystem. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WalletSystem.AzureFunctions.Tests
{
    public class ProblemDetailsTest
    {
        [Fact]
        public void Constructor()
        {
            var details = new ProblemDetails();

            details.Detail.Should().BeNull();
            details.Extensions.Should().NotBeNull();
            details.Instance.Should().BeNull();
            details.Status.Should().BeNull();
            details.Title.Should().BeNull();
            details.Type.Should().BeNull();
        }

        [Fact]
        public void Detail_ValueChanged()
        {
            var details = new ProblemDetails();

            details.Detail = "The detail";
            details.Detail.Should().Be("The detail");
        }

        [Fact]
        public void Extensions_ValueChanged()
        {
            var extensions = new Dictionary<string, object>();
            var details = new ProblemDetails();
            details.Extensions = extensions!;

            details.Extensions.Should().BeSameAs(extensions!);
        }

        [Fact]
        public void Instance_ValueChanged()
        {
            var details = new ProblemDetails();
            details.Instance = "The instance";

            details.Instance.Should().Be("The instance");
        }

        [Fact]
        public void Status_ValueChanged()
        {
            var details = new ProblemDetails();
            details.Status = 1234;

            details.Status.Should().Be(1234);
        }

        [Fact]
        public void Title_ValueChanged()
        {
            var details = new ProblemDetails();
            details.Title = "The title";

            details.Title.Should().Be("The title");
        }

        [Fact]
        public void Type_ValueChanged()
        {
            var details = new ProblemDetails();
            details.Type = "The type";

            details.Type.Should().Be("The type");
        }
    }
}
