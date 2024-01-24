using System.Linq.Expressions;

namespace System.Validation;

public static class FleeValidatorExstensions
{
    #region Filtering

    /// <summary>Filters the <see cref="FleeValidationResult.Errors"/> collection with a Predicate.</summary>
    public static IReadOnlyList<FleeValidationError> Filter(this IEnumerable<FleeValidationError> errors, Func<FleeValidationError, bool> predicate)
        => errors.Where(predicate).ToList();

    #endregion // Filtering

    #region Grouping methods

    /// <summary>Converts to array of <see cref="System.ComponentModel.DataAnnotations.ValidationResult"/>.</summary>
    public static System.ComponentModel.DataAnnotations.ValidationResult[] ToValidationResults(this IEnumerable<FleeValidationError> errors) =>
        errors.GroupBy(x => x.ErrorMessage)
              .Select(group => new System.ComponentModel.DataAnnotations.ValidationResult(group.Key, group.Select(e => e.PropertyName).Distinct()))
              .ToArray();

    /// <summary>
    /// Converts the <see cref="FleeValidationResult.Errors"/> collection into a simple dictionary grouped by PropertyName.
    /// </summary>
    public static Dictionary<string, string[]> GroupedByPropertyName(this IEnumerable<FleeValidationError> errors) =>
        errors.GroupBy(x => x.PropertyName).ToDictionary(
            g => g.Key,
            g => g.Select(x => x.ErrorMessage).Distinct().ToArray()
        );

    /// <summary>
    /// Converts the <see cref="FleeValidationResult.Errors"/> collection into a simple dictionary grouped by ErrorMessage.
    /// </summary>
    public static Dictionary<string, string[]> GroupedByErrorMessage(this IEnumerable<FleeValidationError> errors) =>
        errors.GroupBy(x => x.ErrorMessage).ToDictionary(
            g => g.Key,
            g => g.Select(x => x.PropertyName).Distinct().ToArray()
        );

    public static IDictionary<string, string[]> ToDictionary<TModel>(this IEnumerable<FleeValidationResult> validationResults)
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

        //var failures = validationResults.SelectMany(r => r.Errors).ToArray();
        //if (failures.Length > 0)
        //{
        //    return failures
        //        .GroupBy(
        //            x => x.PropertyName,
        //            x => x.ErrorMessage,
        //            (propertyName, errorMessages) => new
        //            {
        //                Key = propertyName,
        //                Values = errorMessages.Distinct().ToArray()
        //            })
        //        .ToDictionary(x => x.Key, x => x.Values);
        //}
        //return new Dictionary<string, string[]>();
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

            case ExpressionType.ArrayLength:
                return $"{GetMemberPath(((UnaryExpression)expression).Operand)}.Length";

            default:
                throw new Exception("not a proper member selector");
        }
    }

    public static string GetMemberPath(this Expression expression) =>
        expression switch
        {
            LambdaExpression lambdaExpression
                => GetMemberPath(lambdaExpression.Body),

            MemberExpression memberExpression when memberExpression.Expression?.NodeType is ExpressionType.MemberAccess
                => $"{GetMemberPath(memberExpression.Expression!)}.{memberExpression.Member.Name}",

            MemberExpression memberExpression when memberExpression.Expression?.NodeType is ExpressionType.Call
                => $"{GetMemberPath(memberExpression.Expression!)}.{memberExpression.Member.Name}",

            MemberExpression memberExpression when memberExpression.NodeType is not ExpressionType.Parameter
                => memberExpression.Member.Name,

            MethodCallExpression methodCallExpression
                => methodCallExpression.Method.Name,
            
            UnaryExpression unaryExpression when unaryExpression.Operand is MethodCallExpression methodCallExpressionOperand
                => $"{GetMemberPath(methodCallExpressionOperand)}.{methodCallExpressionOperand.Method.Name}",

            UnaryExpression unaryExpression when unaryExpression.NodeType == ExpressionType.ArrayLength && unaryExpression.Operand is MemberExpression
                => $"{GetMemberPath(unaryExpression.Operand)}.Length",

            UnaryExpression unaryExpression when unaryExpression.Operand is MemberExpression memberExpressionOperand
                => memberExpressionOperand.Member.Name,

            BinaryExpression binaryExpression when binaryExpression.NodeType == ExpressionType.ArrayIndex
                => $"{GetMemberPath(binaryExpression.Left)}[{GetMemberPath(binaryExpression.Right)}]",

            ConstantExpression constantExpression
                => constantExpression?.Value is null ? string.Empty : constantExpression!.Value!.ToString() ?? string.Empty,

            _ => throw new ArgumentException("Unsupported expression format.", expression.ToString())
        };

    /// <summary>
    /// Extract from expression 'o => o.Thing1.Thing2' something like 'Thing1.Thing2'
    /// </summary>
    public static string GetExpressionPath(this Expression expression)
    {
        var str = expression.ToString().AsSpan();

        var pos = str.IndexOf(" => ");
        if (pos >= 0) str = str.Slice(pos + 4); // length of " => "

        pos = str.IndexOf("ArrayLength(");
        if (pos >= 0)
        {
            str = str.Slice(pos + 12); // length of "ArrayLength("
            pos = str.IndexOf('.');
            if (pos >= 0) str = str.Slice(pos + 1);
            return str.Slice(0, str.IndexOf(')')).ToString() + ".Length";
        }

        return str.Slice(str.IndexOf('.') + 1).ToString();
    }

    #endregion // Member selectors
}
