namespace Application.Common.Exceptions;

using System;

public class AlreadyExistsException(string message) : ApplicationException(message)
{
}
