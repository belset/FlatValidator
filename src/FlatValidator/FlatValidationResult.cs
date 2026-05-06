using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Validation;

/// <summary>
/// Define a validation error contaner
/// </summary>

[Serializable, DebuggerDisplay("{DebuggerDisplay(),nq}")]
public class FlatValidationResult : IDisposable
{
    #region Members

    [ThreadStatic]
    private static List<FlatValidationError>? t_cachedErrors;
    [ThreadStatic]
    private static List<FlatValidationWarning>? t_cachedWarnings;

    private List<FlatValidationError> errors = null!;
    private List<FlatValidationWarning> warnings = null!;

    /// <summary>
    /// A collection of errors.
    /// </summary>
    public IReadOnlyList<FlatValidationError> Errors => errors;

    /// <summary>
    /// A collection of warnings.
    /// </summary>
    public IReadOnlyList<FlatValidationWarning> Warnings => warnings;

    /// <summary>
    /// A collections with meta data.
    /// </summary>
    public IReadOnlyDictionary<string, string?> MetaData { get; internal set; }

    /// <summary>
    /// Exception if it occured during validation.
    /// </summary>
    public Exception? Exception { get; internal set; } = null!;

    /// <summary>
    /// Whether validation succeeded.
    /// </summary>
    public bool IsValid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => errors.Count == 0 && Exception is null;
    }

    #endregion // Members

    #region Constructors

    /// <summary>
    /// Creates a new ValidationResult from a collection of errors.
    /// </summary>
    /// <param name="metaData">Custom contaner that may be used to store imtermediate info about validation process.</param>
    /// <param name="exception">Instance of <see cref="Exception"/> which is later available through the <see cref="Exception"/> property.</param>
    public FlatValidationResult(IReadOnlyDictionary<string, string?> metaData, Exception? exception = null)
    {
        errors = Interlocked.Exchange(ref t_cachedErrors, null) ?? [];
        warnings = Interlocked.Exchange(ref t_cachedWarnings, null) ?? [];
        MetaData = metaData;
        Exception = exception;
    }

    /// <summary>
    /// Creates a new ValidationResult from a collection of errors.
    /// </summary>
    /// <param name="result">Another instance of the <see cref="FlatValidationResult"/> that has to be cloned.</param>
    public FlatValidationResult(FlatValidationResult result)
    {
        errors = [.. result.errors];
        warnings = [.. result.warnings];
        MetaData = result.MetaData;
        Exception = result.Exception;
    }

    #endregion // Constructors

    #region Operators

    /// <summary>Extracts actual result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator bool(in FlatValidationResult result) => result.IsValid;

    #endregion // Operators

    #region IDisposable implementation
    public void Dispose()
    {
        if (errors.Count < 32)
        {
            errors.Clear();
            Interlocked.CompareExchange(ref t_cachedErrors, errors, null);
        }
        if (warnings.Count < 32)
        {
            warnings.Clear();
            Interlocked.CompareExchange(ref t_cachedWarnings, warnings, null);
        }
    }
    #endregion // IDisposable implementation

    #region Public methods

    /// <summary>
    /// Converts the ValidationResult's error collection into a simple dictionary representation  grouped by PropertyName.
    /// </summary>
    /// <returns>A dictionary that's grouped by PropertyName.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IDictionary<string, string[]> ToDictionary() => this.errors.GroupedByPropertyName();

    /// <summary>
    /// Converts the ValidationResult's warning collection into a simple dictionary representation  grouped by PropertyName.
    /// </summary>
    /// <returns>A dictionary after grouping by PropertyName.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IDictionary<string, string[]> WarningsToDictionary() => this.warnings.GroupedByPropertyName();

    /// <summary>
    /// Adds an validation error object to the end of the <see cref="FlatValidationResult.Errors"/> list.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddError(FlatValidationError error) => this.errors.Add(error);

    /// <summary>
    /// Adds an validation warning object to the end of the <see cref="FlatValidationResult.Warnings"/> list.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddWarning(FlatValidationWarning warning) => this.warnings.Add(warning);

    /// <summary>
    /// Generates a string representation of the error messages separated by new lines.
    /// </summary>
    public override string ToString() => ToString(Environment.NewLine);

    /// <summary>
    /// Generates a string representation of the error messages separated by the specified character.
    /// </summary>
    /// <param name="separator">The character to separate the error messages.</param>
    public string ToString(string separator) => string.Join(separator, errors);

    string DebuggerDisplay() => ToString(", ");

    #endregion // Public methods
}
