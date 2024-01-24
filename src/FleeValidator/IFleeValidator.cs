namespace System.Validation;

/// <summary>
/// Defines a validator for a particular type.
/// </summary>
/// <typeparam name="TModel"></typeparam>
public interface IFleeValidator<TModel>
{
    /// <summary>
    /// Validates the specified instance.
    /// </summary>
    /// <param name="instance">The instance to validate</param>
    /// <returns>A ValidationResult object containing any validation failures.</returns>
    FleeValidationResult Validate(in TModel model);

    /// <summary>
    /// Validate the specified instance asynchronously
    /// </summary>
    /// <param name="instance">The instance to validate</param>
    /// <param name="cancellation"></param>
    /// <returns>A ValidationResult object containing any validation failures.</returns>
    ValueTask<FleeValidationResult> ValidateAsync(TModel model, CancellationToken cancellation);
}
