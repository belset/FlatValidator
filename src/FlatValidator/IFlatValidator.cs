namespace System.Validation;

/// <summary>
/// Defines a validator for a particular type.
/// </summary>
/// <typeparam name="TModel"></typeparam>
public interface IFlatValidator<TModel>
{
    /// <summary>
    /// Validates the specified instance.
    /// </summary>
    /// <param name="model">The instance to validate</param>
    /// <returns>A ValidationResult object containing any validation failures.</returns>
    FlatValidationResult Validate(in TModel model);

    /// <summary>
    /// Validates the specified instance.
    /// </summary>
    /// <param name="model">The instance to validate</param>
    /// <param name="timeout">A <see cref="TimeSpan"/> that represents the number of milliseconds
    /// to wait, or a <see cref="TimeSpan"/> that represents -1 milliseconds to wait indefinitely.
    /// </param>
    /// <returns>A ValidationResult object containing any validation failures.</returns>
    FlatValidationResult Validate(in TModel model, TimeSpan timeout);

    /// <summary>
    /// Validate the specified instance asynchronously
    /// </summary>
    /// <param name="model">The instance to validate</param>
    /// <param name="cancellation">The <see cref="CancellationToken"/> token to observe.</param>
    /// <returns>A ValidationResult object containing any validation failures.</returns>
    ValueTask<FlatValidationResult> ValidateAsync(TModel model, CancellationToken cancellation = default);

    /// <summary>
    /// Validate the specified instance asynchronously
    /// </summary>
    /// <param name="model">The instance to validate</param>
    /// <param name="timeout">A <see cref="TimeSpan"/> that represents the number of milliseconds
    /// to wait, or a <see cref="TimeSpan"/> that represents -1 milliseconds to wait indefinitely.
    /// </param>
    /// <param name="cancellation">The <see cref="CancellationToken"/> token to observe.</param>
    /// <returns>A ValidationResult object containing any validation failures.</returns>
    ValueTask<FlatValidationResult> ValidateAsync(TModel model, TimeSpan timeout, CancellationToken cancellation = default);
}
