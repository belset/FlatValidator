namespace System.Validation;

/// <summary>
/// Define a validation warning contaner
/// </summary>
[Serializable]
public readonly record struct FlatValidationWarning(string PropertyName, string WarningMessage)
{
    /// <summary>
    /// Userfriendly representation of the warning
    /// </summary>
    public override string ToString() => $"{PropertyName}: {WarningMessage}";
}
