namespace Application.Common.Exceptions;

using System;

public class NotFoundException(string message) : ApplicationException(message)
{
}
