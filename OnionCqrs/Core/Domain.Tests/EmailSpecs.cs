using Domain.SharedKernel;
using FluentAssertions;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
using Xunit;
using Xunit.Abstractions;

namespace Domain.Tests
{
    public sealed class EmailSpecs
    {
        private readonly ITestOutputHelper outputHelper;

        public EmailSpecs(ITestOutputHelper outputHelper) => this.outputHelper = outputHelper;

        [Fact]
        public void Create_with_null_provided_should_return_fail_result()
        {
            // arrange
            // act
            var email = Email.Create(null);
            outputHelper.WriteLine(email.Error);

            // assert
            email.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void Create_with_proper_email_provided_should_return_success_result()
        {
            // arrange
            var providedEmail = RandomizerFactory.GetRandomizer(new FieldOptionsEmailAddress())
                                                 .Generate();

            // act
            var email = Email.Create(providedEmail);

            // assert
            email.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Create_with_email_without_at_provided_should_return_fail_result()
        {
            // arrange
            var providedEmail = RandomizerFactory.GetRandomizer(new FieldOptionsEmailAddress())
                                                 .Generate()
                                                 .Replace("@", string.Empty);

            outputHelper.WriteLine(providedEmail);

            // act
            var email = Email.Create(providedEmail);
            outputHelper.WriteLine(email.Error);

            // assert
            email.IsSuccess.Should().BeFalse();
        }
    }
}
