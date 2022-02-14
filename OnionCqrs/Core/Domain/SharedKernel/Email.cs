using CSharpFunctionalExtensions;
using Domain.Common;
using Domain.Extensions;
using System.Collections.Generic;

namespace Domain.Utils
{
    public sealed class Email : ValueObject
    {
        private readonly string value;

        private Email(string email) => this.value = email;

        public static Result<Email> Create(string email) =>
            Result.SuccessIf(string.IsNullOrWhiteSpace(email) is false, email, "Email is empty!")
                  .Ensure(emailToValidate => emailToValidate.IsValidEmail())
                  .Map(validEmail => new Email(validEmail));

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.value;
        }

        public static implicit operator string(Email email) => email.value;
    }
}
