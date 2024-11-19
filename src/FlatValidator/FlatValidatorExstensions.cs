using System.Linq.Expressions;


namespace System.Validation;

public static partial class FlatValidatorExstensions
{
    #region Grouping methods

    /// <summary>Converts to array of <see cref="System.ComponentModel.DataAnnotations.ValidationResult"/>.</summary>
    public static System.ComponentModel.DataAnnotations.ValidationResult[] ToValidationResults(this IEnumerable<FlatValidationError> errors) =>
        errors.GroupBy(x => x.ErrorMessage)
              .Select(group => new System.ComponentModel.DataAnnotations.ValidationResult(group.Key, group.Select(e => e.PropertyName).Distinct()))
              .ToArray();

    /// <summary>
    /// Converts the <see cref="FlatValidationResult.Errors"/> collection into a simple dictionary grouped by PropertyName.
    /// </summary>
    public static IDictionary<string, string[]> GroupedByPropertyName(this IEnumerable<FlatValidationError> errors) =>
        errors.GroupBy(x => x.PropertyName).ToDictionary(
            g => g.Key,
            g => g.Select(x => x.ErrorMessage).Distinct().ToArray()
        );

    /// <summary>
    /// Converts the <see cref="FlatValidationResult.Errors"/> collection into a simple dictionary grouped by ErrorMessage.
    /// </summary>
    public static IDictionary<string, string[]> GroupedByErrorMessage(this IEnumerable<FlatValidationError> errors) =>
        errors.GroupBy(x => x.ErrorMessage).ToDictionary(
            g => g.Key,
            g => g.Select(x => x.PropertyName).Distinct().ToArray()
        );

    /// <summary>
    /// Converts the <see cref="FlatValidationResult.Warnings"/> collection into a simple dictionary grouped by PropertyName.
    /// </summary>
    public static IDictionary<string, string[]> GroupedByPropertyName(this IEnumerable<FlatValidationWarning> warnings) =>
        warnings.GroupBy(x => x.PropertyName).ToDictionary(
            g => g.Key,
            g => g.Select(x => x.WarningMessage).Distinct().ToArray()
        );

    /// <summary>
    /// Converts the <see cref="FlatValidationResult.Warnings"/> collection into a simple dictionary grouped by ErrorMessage.
    /// </summary>
    public static IDictionary<string, string[]> GroupedByErrorMessage(this IEnumerable<FlatValidationWarning> warnings) =>
        warnings.GroupBy(x => x.WarningMessage).ToDictionary(
            g => g.Key,
            g => g.Select(x => x.PropertyName).Distinct().ToArray()
        );

    /// <summary>
    /// Group collection of the FlatValidationResult into one dictionary.
    /// </summary>
    /// <param name="validationResults">Collection of the <see cref="FlatValidationResult"/></param>
    /// <returns>A dictionary that's grouped by PropertyName.</returns>
    public static IDictionary<string, string[]> ToDictionary(this IEnumerable<FlatValidationResult> validationResults)
    {
        ArgumentNullException.ThrowIfNull(validationResults);

        return validationResults
                .SelectMany(r => r.Errors)
                .GroupBy(
                    x => x.PropertyName,
                    x => x.ErrorMessage,
                    (propertyName, errorMessages) => new
                    {
                        Key = propertyName,
                        Values = errorMessages.Distinct().ToArray()
                    })
                .ToDictionary(x => x.Key, x => x.Values);
    }

    #endregion // Grouping methods

    #region Member selectors

    public static string GetMemberName(this Expression expression)
    {
        switch (expression.NodeType)
        {
            case ExpressionType.Lambda:
                return GetMemberName(((LambdaExpression)expression).Body);

            case ExpressionType.Parameter:
                return ((ParameterExpression)expression).Name!;

            case ExpressionType.MemberAccess:
                return (((MemberExpression)expression).Expression?.NodeType == ExpressionType.MemberAccess)
                    ? $"{GetMemberName(((MemberExpression)expression).Expression!)}.{((MemberExpression)expression).Member.Name}"
                    : ((MemberExpression)expression).Member.Name;
            
            case ExpressionType.Constant:
                return ((ConstantExpression)expression).Value?.ToString() ?? string.Empty;

            case ExpressionType.Call:
                return ((MethodCallExpression)expression).Method.Name;

            case ExpressionType.Convert:
            case ExpressionType.ConvertChecked:
                return GetMemberName(((UnaryExpression)expression).Operand);

            case ExpressionType.Invoke:
                return GetMemberName(((InvocationExpression)expression).Expression);

            case ExpressionType.ArrayIndex:
                return $"{GetMemberName(((BinaryExpression)expression).Left)}[{GetMemberName(((BinaryExpression)expression).Right)}]";

            case ExpressionType.ArrayLength:
                return $"{GetMemberName(((UnaryExpression)expression).Operand)}.Length";

            default:
                throw new Exception("not a proper member selector");
        }
    }

    #endregion // Member selectors
}
