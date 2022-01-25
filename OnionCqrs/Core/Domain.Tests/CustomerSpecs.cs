using Domain.Persons;
using FluentAssertions;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
using Xunit;
using Xunit.Abstractions;

namespace Domain.Tests
{
    public class CustomerSpecs
    {
        private readonly ITestOutputHelper outputHelper;

        public CustomerSpecs(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }

        [Fact]
        public void CustomerName_Create_should_return_success_result()
        {
            // arrange
            // act
            var customerName = CustomerName.Create("Jan");

            // assert
            customerName.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void CustomerName_Create_with_null_provided_should_return_fail_result()
        {
            // arrange
            // act
            var customerName = CustomerName.Create(null);
            outputHelper.WriteLine(customerName.Error);

            // assert
            customerName.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void CustomerName_Create_with_fifty_chars_provided_should_return_success_result()
        {
            // arrange
            string providedName = RandomizerFactory.GetRandomizer(new FieldOptionsText()
            {
                Min = 50, Max = 50
            })
                .Generate();

            // act
            var customerName = CustomerName.Create(providedName);

            // assert
            customerName.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void CustomerName_Create_with_fiftyone_chars_provided_should_return_fail_result()
        {
            // arrange
            string providedName = RandomizerFactory.GetRandomizer(new FieldOptionsText()
            {
                Min = 51,
                Max = 51
            })
                .Generate();

            // act
            var customerName = CustomerName.Create(providedName);
            outputHelper.WriteLine(customerName.Error);

            // assert
            customerName.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void Email_Create_with_null_provided_should_return_fail_result()
        {
            // arrange
            // act
            var email = Email.Create(null);
            outputHelper.WriteLine(email.Error);

            // assert
            email.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void Email_Create_with_proper_email_provided_should_return_success_result()
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
        public void Email_Create_with_email_without_at_provided_should_return_fail_result()
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
