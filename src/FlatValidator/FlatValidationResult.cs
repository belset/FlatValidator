using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Validation;

[Serializable]
public record class FlatValidationError(string PropertyName, string ErrorMessage)
{
    public override string ToString() => $"{PropertyName}: {ErrorMessage}";
}

[Serializable, DebuggerDisplay("{DebuggerDisplay(),nq}")]
public class FlatValidationResult
{
    #region Members

    private List<FlatValidationError> errors = new();

    /// <summary>
    /// A collection of errors
    /// </summary>
    public IReadOnlyList<FlatValidationError> Errors => errors;

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
    /// <param name="error">Instance of <see cref="Exception"/> which is later available through the <see cref="Errors"/> property.</param>
    public FlatValidationResult(Exception exception) => Exception = exception;

    /// <summary>
    /// Creates a new ValidationResult from a collection of errors
    /// </summary>
    /// <param name="errors">Collection of <see cref="FlatValidationError"/> instances which is later available through the <see cref="Errors"/> property.</param>
    public FlatValidationResult(IEnumerable<FlatValidationError> errors) => AddErrorRange(errors);

    #endregion // Constructors

    #region Operators

    /// <summary>Extracts actual result for <see cref="TypedResults.ValidationProblem(...)"/>..</summary>
    public static implicit operator bool(in FlatValidationResult result) => result.IsValid;

    #endregion // Operators

    #region Public methods

    /// <summary>
    /// Converts the ValidationResult's errors collection into a simple dictionary representation  grouped by PropertyName.
    /// </summary>
    /// <param name="groupedBy">Ensure to grouping by property name.</param>
    /// <returns>A dictionary after grouping by PropertyName.</returns>
    public Dictionary<string, string[]> ToDictionary() => Errors.GroupedByPropertyName();

    /// <summary>
    /// Adds an validation error object to the end of the Errors list.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddError(FlatValidationError error) => this.errors.Add(error);

    /// <summary>
    /// Adds a collection of validation error objects to the end of the Errors list.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddErrorRange(IEnumerable<FlatValidationError> errors) => this.errors.AddRange(errors);

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
