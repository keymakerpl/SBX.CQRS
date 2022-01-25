using Domain.Errors.Base;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Errors
{
    public static class Errors
    {
        public static class General
        {
            public static Error NotFound(string entityName, int id) =>
                new Error("record.not.found", $"'{entityName}' not found for Id '{id}'");
        }

        public static class Customer
        {
            public static Error CustomerRegistrationError(IEnumerable<string> errors) =>
                new Error("customer.validation.error",
                    $"Cannot register customer due to an validation errors: {errors.Aggregate((e1, e2) => $"{e1}, {e2}")}");
        }
    }
}
