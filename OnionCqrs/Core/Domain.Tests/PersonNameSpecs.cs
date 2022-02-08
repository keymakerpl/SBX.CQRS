using Domain.Persons;
using FluentAssertions;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
using Xunit;
using Xunit.Abstractions;

namespace Domain.Tests
{
    public sealed class PersonNameSpecs
    {
        private readonly ITestOutputHelper outputHelper;

        public PersonNameSpecs(ITestOutputHelper outputHelper) => this.outputHelper = outputHelper;

        [Fact]
        public void Create_should_return_success_result()
        {
            // arrange
            // act
            var customerName = PersonName.Create("Jan");

            // assert
            customerName.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Create_with_null_provided_should_return_fail_result()
        {
            // arrange
            // act
            var customerName = PersonName.Create(null);
            outputHelper.WriteLine(customerName.Error);

            // assert
            customerName.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void Create_with_fifty_chars_provided_should_return_success_result()
        {
            // arrange
            string providedName = RandomizerFactory.GetRandomizer(new FieldOptionsText()
            {
                Min = 50, Max = 50
            })
                .Generate();

            // act
            var customerName = PersonName.Create(providedName);

            // assert
            customerName.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Create_with_fiftyone_chars_provided_should_return_fail_result()
        {
            // arrange
            string providedName = RandomizerFactory.GetRandomizer(new FieldOptionsText()
            {
                Min = 51,
                Max = 51
            })
                .Generate();

            // act
            var customerName = PersonName.Create(providedName);
            outputHelper.WriteLine(customerName.Error);

            // assert
            customerName.IsSuccess.Should().BeFalse();
        }
    }
}
