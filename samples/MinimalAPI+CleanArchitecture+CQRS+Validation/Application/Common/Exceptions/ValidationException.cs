namespace Application.Common.Exceptions;

using System;

public class ValidationException(IDictionary<string, string[]> errors, string message = "") : ApplicationException(message)
{
    public IDictionary<string, string[]> Errors = errors ?? throw new ArgumentNullException(nameof(errors));
}
