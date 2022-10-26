using System.Net;

namespace Dukkantek.Test.Application.Common.Exceptions
{
    internal class ValidationException : CustomException
    {
        public ValidationException(List<string>? errors = null)
            : base("One or more validations failed.", errors, HttpStatusCode.BadRequest)
        {
        }
    }
}
