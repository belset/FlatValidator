using System.Diagnostics.CodeAnalysis;

using Application.Common;
using Application.Common.Exceptions;


namespace Presentation._Startup;

[ExcludeFromCodeCoverage]
public static class ResultOrErrorExtensions
{
    public static IResult MatchToTypedResult<TResult>(this ResultOrError<TResult, Exception> result,
            Func<TResult, IResult> successAction = null!)
    {
        {
            successAction ??= (r) => TypedResults.Ok(r);
            return result.Match(
                success => successAction(success),
                failure => failure switch
                {
                    NotFoundException ex => TypedResults.NotFound(ex),
                    AlreadyExistsException ex => TypedResults.BadRequest(ex),
                    ValidationException ex => TypedResults.ValidationProblem(ex.Errors),
                    _ => TypedResults.Problem(failure.StackTrace, failure.Message, StatusCodes.Status500InternalServerError)
                }
            );
        }
    }
    public static IResult MatchToTypedResult<TResult>(this ValueTask<ResultOrError<TResult, Exception>> task, CancellationToken cancellationToken = default)
    {
        return task.AsTask().WaitAsync(cancellationToken).Result.MatchToTypedResult();
    }
    public static IResult MatchToTypedResult<TResult>(this ValueTask<ResultOrError<TResult, Exception>> task,
        Func<TResult, IResult> successAction, CancellationToken cancellationToken = default)
    {
        return task.AsTask().WaitAsync(cancellationToken).Result.MatchToTypedResult(successAction);
    }

    public static IResult MatchToTypedResult<TResult>(this ValueTask<QueryResult<TResult>> task, CancellationToken cancellationToken = default)
    {
        return task.AsTask().WaitAsync(cancellationToken).Result.MatchToTypedResult();
    }
    public static IResult MatchToTypedResult<TResult>(this ValueTask<QueryResult<TResult>> task,
        Func<TResult, IResult> successAction, CancellationToken cancellationToken = default)
    {
        return task.AsTask().WaitAsync(cancellationToken).Result.MatchToTypedResult(successAction);
    }

    public static IResult MatchToTypedResult<TResult>(this ValueTask<CommandResult<TResult>> task, CancellationToken cancellationToken = default)
    {
        return task.AsTask().WaitAsync(cancellationToken).Result.MatchToTypedResult();
    }
    public static IResult MatchToTypedResult<TResult>(this ValueTask<CommandResult<TResult>> task,
        Func<TResult, IResult> successAction, CancellationToken cancellationToken = default)
    {
        return task.AsTask().WaitAsync(cancellationToken).Result.MatchToTypedResult(successAction);
    }
}
