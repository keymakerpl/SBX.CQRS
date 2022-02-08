using CSharpFunctionalExtensions;
using Domain.Common;
using System.Collections.Generic;

namespace Domain.Persons
{
    public sealed class PersonName : ValueObject
    {
        private readonly string value;

        private PersonName(string customerName) => this.value = customerName;

        public static Result<PersonName> Create(string customerName) => 
            Result.SuccessIf(string.IsNullOrWhiteSpace(customerName) is false, customerName, "Empty customer name!")
                  .Ensure(name => name.Length <= 50, "Customer name exceed 50 characters!")
                  .Map(name => new PersonName(name));

        public static implicit operator string(PersonName customertName) => customertName.value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return value;
        }
    }
}
