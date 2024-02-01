using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Validation;

/// <summary>
/// Define a validation error contaner
/// </summary>

[Serializable, DebuggerDisplay("{DebuggerDisplay(),nq}")]
public class FlatValidationResult
{
    #region Members

    private List<FlatValidationError> errors = new();
    private List<FlatValidationWarning> warnings = new();

    /// <summary>
    /// A collection of errors
    /// </summary>
    public IReadOnlyList<FlatValidationError> Errors => errors;

    /// <summary>
    /// A collection of warnings
    /// </summary>
    public IReadOnlyList<FlatValidationWarning> Warnings => warnings;

    /// <summary>
    /// Exception if it occured during validation
    /// </summary>
    public Exception? Exception { get; internal set; } = null!;

    /// <summary>
    /// Whether validation succeeded
    /// </summary>
    public bool IsValid => errors.Count == 0 && Exception is null;

    #endregion // Members

    #region Constructors

    /// <summary>
    /// Creates a new ValidationResult
    /// </summary>
    public FlatValidationResult()
    { }

    /// <summary>
    /// Creates a new ValidationResult from a collection of errors
    /// </summary>
    /// <param name="exception">Instance of <see cref="Exception"/> which is later available through the <see cref="Exception"/> property.</param>
    public FlatValidationResult(Exception exception) => Exception = exception;

    /// <summary>
    /// Creates a new ValidationResult from a collection of errors
    /// </summary>
    /// <param name="result">Another instance of the <see cref="FlatValidationResult"/> that has to be cloned.</param>
    public FlatValidationResult(FlatValidationResult result)
    {
        errors.AddRange(result.errors);
        warnings.AddRange(result.warnings);
        Exception = result.Exception;
    }

    #endregion // Constructors

    #region Operators

    /// <summary>Extracts actual result for <see cref="TypedResults.ValidationProblem()"/>..</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator bool(in FlatValidationResult result) => result.IsValid;

    #endregion // Operators

    #region Public methods

    /// <summary>
    /// Converts the ValidationResult's error collection into a simple dictionary representation  grouped by PropertyName.
    /// </summary>
    /// <returns>A dictionary that's grouped by PropertyName.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Dictionary<string, string[]> ToDictionary() => this.errors.GroupedByPropertyName();

    /// <summary>
    /// Converts the ValidationResult's warning collection into a simple dictionary representation  grouped by PropertyName.
    /// </summary>
    /// <returns>A dictionary after grouping by PropertyName.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Dictionary<string, string[]> WarningToDictionary() => this.warnings.GroupedByPropertyName();

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
    /// <returns></returns>
    public override string ToString() => ToString(Environment.NewLine);

    /// <summary>
    /// Generates a string representation of the error messages separated by the specified character.
    /// </summary>
    /// <param name="separator">The character to separate the error messages.</param>
    /// <returns></returns>
    public string ToString(string separator) => string.Join(separator, errors);

    string DebuggerDisplay() => ToString(", ");

    #endregion // Public methods
}
