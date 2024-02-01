namespace System.Validation;

/// <summary>
/// Define a validation error contaner
/// </summary>
[Serializable]
public record class FlatValidationError(string PropertyName, string ErrorMessage)
{
    /// <summary>
    /// Userfriendly representation of the error
    /// </summary>
    public override string ToString() => $"{PropertyName}: {ErrorMessage}";
}
