using CSharpFunctionalExtensions;
using System.Net.Mail;

namespace Domain.Extensions
{
    public static class StringExtensions
    {
        public static Result<string> IsValidEmail(this string emailToValidate) =>
            Result.Try(() => new MailAddress(emailToValidate))
                  .Ensure(address => address.Address == emailToValidate, "Invalid email!")
                  .Map(address => address.Address);
    }
}
