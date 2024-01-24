using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Validation;

[Serializable]
public record class FleeValidationError(string PropertyName, string ErrorMessage, string Tag)
{
    public override string ToString() => string.IsNullOrWhiteSpace(Tag)
        ? $"{PropertyName}: {ErrorMessage}"
        : $"{PropertyName}: {ErrorMessage} ({Tag})";
}

[Serializable, DebuggerDisplay("{DebuggerDisplay(),nq}")]
public class FleeValidationResult
{
    #region Members

    private List<FleeValidationError> errors = new();
    //private List<FleeValidationWarning> warnings = new();

    /// <summary>
    /// A collection of errors
    /// </summary>
    public IReadOnlyList<FleeValidationError> Errors => errors;

    /// <summary>
    /// A collection of warnings
    /// </summary>
    //public IReadOnlyList<FleeValidationWarning> Warnings => warnings;

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
    public FleeValidationResult()
    { }

    /// <summary>
    /// Creates a new ValidationResult from a collection of errors
    /// </summary>
    /// <param name="error">Instance of <see cref="Exception"/> which is later available through the <see cref="Errors"/> property.</param>
    public FleeValidationResult(Exception exception) => Exception = exception;

    /// <summary>
    /// Creates a new ValidationResult from a collection of errors
    /// </summary>
    /// <param name="errors">Collection of <see cref="FleeValidationError"/> instances which is later available through the <see cref="Errors"/> property.</param>
    public FleeValidationResult(IEnumerable<FleeValidationError> errors) => AddErrorRange(errors);

    #endregion // Constructors

    #region Operators

    /// <summary>Extracts actual result for <see cref="TypedResults.ValidationProblem(...)"/>..</summary>
    public static implicit operator bool(in FleeValidationResult result) => result.IsValid;

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
    public void AddError(FleeValidationError error) => this.errors.Add(error);

    /// <summary>
    /// Adds a collection of validation error objects to the end of the Errors list.
    /// </summary>
    //MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddErrorRange(IEnumerable<FleeValidationError> errors) => this.errors.AddRange(errors);

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
