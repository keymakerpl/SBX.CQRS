using Domain.Common;
using System.Collections.Generic;

namespace Domain.Errors.Base
{
    public sealed class Error : ValueObject
    {
        public string Code { get; }
        public string Message { get; }

        internal Error(string code, string message)
        {
            Code = code ?? throw new System.ArgumentNullException(nameof(code));
            Message = message ?? throw new System.ArgumentNullException(nameof(message));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}
