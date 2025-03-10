﻿using System.Diagnostics;
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
    public bool IsValid => errors.Count == 0 && Exception is null;

    #endregion // Members

    #region Constructors

    /// <summary>
    /// Creates a new ValidationResult.
    /// </summary>
    /// <param name="metaData">Custom contaner that may be used to store imtermediate info about validation process.</param>
    public FlatValidationResult(IReadOnlyDictionary<string, string?> metaData)
    {
        MetaData = metaData;
    }

    /// <summary>
    /// Creates a new ValidationResult from a collection of errors.
    /// </summary>
    /// <param name="metaData">Custom contaner that may be used to store imtermediate info about validation process.</param>
    /// <param name="exception">Instance of <see cref="Exception"/> which is later available through the <see cref="Exception"/> property.</param>
    public FlatValidationResult(IReadOnlyDictionary<string, string?> metaData, Exception exception)
    {
        MetaData = metaData;
        Exception = exception;
    }

    /// <summary>
    /// Creates a new ValidationResult from a collection of errors.
    /// </summary>
    /// <param name="result">Another instance of the <see cref="FlatValidationResult"/> that has to be cloned.</param>
    public FlatValidationResult(FlatValidationResult result)
    {
        errors.AddRange(result.errors);
        warnings.AddRange(result.warnings);
        MetaData = result.MetaData;
        Exception = result.Exception;
    }

    #endregion // Constructors

    #region Operators

    /// <summary>Extracts actual result.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator bool(in FlatValidationResult result) => result.IsValid;

    #endregion // Operators

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
