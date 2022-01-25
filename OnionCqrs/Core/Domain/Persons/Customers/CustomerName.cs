using CSharpFunctionalExtensions;
using Domain.Common;
using System.Collections.Generic;

namespace Domain.Persons.Customers
{
    public class CustomerName : ValueObject
    {
        private readonly string value;

        private CustomerName(string customerName) => this.value = customerName;

        public static Result<CustomerName> Create(string customerName) => 
            Result.SuccessIf(string.IsNullOrWhiteSpace(customerName) is false, customerName, "Empty customer name!")
                  .Ensure(name => name.Length <= 50, "Customer name exceed 50 characters!")
                  .Map(name => new CustomerName(name));

        public static implicit operator string(CustomerName customertName) => customertName.value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return value;
        }
    }
}
