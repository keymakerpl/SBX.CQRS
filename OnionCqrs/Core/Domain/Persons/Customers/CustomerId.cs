using Domain.Common;
using System.Collections.Generic;

namespace Domain.Persons.Customers
{
    public class CustomerId : ValueObject
    {
        public CustomerId(int id) => this.Value = id;

        public int Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}