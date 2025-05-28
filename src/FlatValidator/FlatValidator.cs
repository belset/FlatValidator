using System.Linq.Expressions;
using System.Runtime.CompilerServices;


namespace System.Validation;

/// <summary>
/// FlatValidator base class.
/// </summary>
public class FlatValidator<TModel> : IFlatValidator<TModel>
{
    #region Members

    private int semaphoreState = 0; // Allowed==0, Raised==1
    private FlatValidatorRules<TModel> rules = new();

    /// <summary>
    /// A collection with meta data.
    /// </summary>
    public Dictionary<string, string?> MetaData { get; } = new();

    #endregion // Members

    #region If methods

    [Obsolete]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void If(Func<TModel, bool> conditions, Action<TModel> @then, Action<TModel> @else = null!)
        => rules.Add(RuleType.WhenSynch, conditions, @then, @else, null!, null!, null!, null!, null!);

    [Obsolete]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void If(Func<TModel, ValueTask<bool>> conditions, Action<TModel> @then, Action<TModel> @else = null!)
        => rules.Add(RuleType.WhenAsync, conditions, @then, @else, null!, null!, null!, null!, null!);

    [Obsolete]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void If(Func<TModel, CancellationToken, ValueTask<bool>> conditions, 
                            Action<TModel, CancellationToken> @then, Action<TModel, CancellationToken> @else = null!)
        => rules.Add(RuleType.WhenCancelledAsync, conditions, @then, @else, null!, null!, null!, null!, null!);

    #endregion // If methods

    #region When methods

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void When(Func<TModel, bool> conditions, Action<TModel> @then, Action<TModel> @else = null!)
        => rules.Add(RuleType.WhenSynch, conditions, @then, @else, null!, null!, null!, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void When(Func<TModel, ValueTask<bool>> conditions, Action<TModel> @then, Action<TModel> @else = null!)
        => rules.Add(RuleType.WhenAsync, conditions, @then, @else, null!, null!, null!, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void When(Func<TModel, CancellationToken, ValueTask<bool>> conditions,
                            Action<TModel, CancellationToken> @then, Action<TModel, CancellationToken> @else = null!)
        => rules.Add(RuleType.WhenCancelledAsync, conditions, @then, @else, null!, null!, null!, null!, null!);

    #endregion // When methods

    #region Constant Error

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void Error<T1>(Func<TModel, string> error, Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(RuleType.ErrorConst, null!, null!, null!, error, null!, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void Error<T1, T2>(Func<TModel, string> error,
                                Expression<Func<TModel, T1>> memberSelector1,
                                Expression<Func<TModel, T2>> memberSelector2)
    => rules.Add(RuleType.ErrorConst, null!, null!, null!, error, null!, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void Error<T1, T2, T3>(Func<TModel, string> error,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(RuleType.ErrorConst, null!, null!, null!, error, null!, memberSelector1, memberSelector2, memberSelector3);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void Error<T1>(string errorMessage, Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(RuleType.ErrorConst, null!, null!, null!, null!, errorMessage, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void Error<T1, T2>(string errorMessage, Expression<Func<TModel, T1>> memberSelector1, Expression<Func<TModel, T2>> memberSelector2) 
        => rules.Add(RuleType.ErrorConst, null!, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void Error<T1, T2, T3>(string errorMessage, Expression<Func<TModel, T1>> memberSelector1, Expression<Func<TModel, T2>> memberSelector2, Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(RuleType.ErrorConst, null!, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, memberSelector3);

    #endregion // Constant Error

    #region Synchronous ErrorIf

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ErrorIf<T1>(Func<TModel, bool> conditions, Func<TModel, string> error, 
                                    Expression<Func<TModel, T1>> memberSelector) 
        => rules.Add(RuleType.ErrorSynch, conditions, null!, null!, error, null!, memberSelector.Body, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ErrorIf<T1, T2>(Func<TModel, bool> conditions, Func<TModel, string> error,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2) 
        => rules.Add(RuleType.ErrorSynch, conditions, null!, null!, error, null!, memberSelector1.Body, memberSelector2.Body, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ErrorIf<T1, T2, T3>(Func<TModel, bool> conditions, Func<TModel, string> error,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3) 
        => rules.Add(RuleType.ErrorSynch, conditions, null!, null!, error, null!, memberSelector1.Body, memberSelector2.Body, memberSelector3.Body);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ErrorIf<T1>(Func<TModel, bool> conditions, string errorMessage, 
                                    Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(RuleType.ErrorSynch, conditions, null!, null!, null!, errorMessage, memberSelector.Body, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ErrorIf<T1, T2>(Func<TModel, bool> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(RuleType.ErrorSynch, conditions, null!, null!, null!, errorMessage, memberSelector1.Body, memberSelector2.Body, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ErrorIf<T1, T2, T3>(Func<TModel, bool> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(RuleType.ErrorSynch, conditions, null!, null!, null!, errorMessage, memberSelector1.Body, memberSelector2.Body, memberSelector3.Body);

    #endregion // Synchronous ErrorIf

    #region Asynchronous ErrorIf

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public void ErrorIf<T1>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error, 
                                 Expression<Func<TModel, T1>> memberSelector)
    => rules.Add(RuleType.ErrorAsync, conditions, null!, null!, error, null!, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public void ErrorIf<T1,T2>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error,
                               Expression<Func<TModel, T1>> memberSelector1,
                               Expression<Func<TModel, T2>> memberSelector2)
    => rules.Add(RuleType.ErrorAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public void ErrorIf<T1,T2,T3>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error,
                                  Expression<Func<TModel, T1>> memberSelector1,
                                  Expression<Func<TModel, T2>> memberSelector2,
                                  Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(RuleType.ErrorAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, memberSelector3);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ErrorIf<T1>(Func<TModel, ValueTask<bool>> conditions, string errorMessage, 
                            Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(RuleType.ErrorAsync, conditions, null!, null!, null!, errorMessage, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ErrorIf<T1, T2>(Func<TModel, ValueTask<bool>> conditions, string errorMessage,
                                Expression<Func<TModel, T1>> memberSelector1,
                                Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(RuleType.ErrorAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ErrorIf<T1, T2, T3>(Func<TModel, ValueTask<bool>> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(RuleType.ErrorAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, memberSelector3);

    #endregion // Asynchronous ErrorIf

    #region Asynchronous ErrorIf with Cancellation
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async with CancellationToken
    public void ErrorIf<T1>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, Func<TModel, string> error,
                               Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(RuleType.ErrorCancelledAsync, conditions, null!, null!, error, null!, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async with CancellationToken
    public void ErrorIf<T1,T2>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, Func<TModel, string> error, 
                               Expression<Func<TModel, T1>> memberSelector1,
                               Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(RuleType.ErrorCancelledAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async with CancellationToken
    public void ErrorIf<T1,T2,T3>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, Func<TModel, string> error,
                                  Expression<Func<TModel, T1>> memberSelector1,
                                  Expression<Func<TModel, T2>> memberSelector2,
                                  Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(RuleType.ErrorCancelledAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, memberSelector3);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ErrorIf<T1>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, string errorMessage,
                            Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(RuleType.ErrorCancelledAsync, conditions, null!, null!, null!, errorMessage, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ErrorIf<T1, T2>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, string errorMessage,
                                Expression<Func<TModel, T1>> memberSelector1,
                                Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(RuleType.ErrorCancelledAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ErrorIf<T1, T2, T3>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(RuleType.ErrorCancelledAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, memberSelector3);

    #endregion // Asynchronous ErrorIf with Cancellation

    #region Synchronous ValidIf

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ValidIf<T1>(Func<TModel, bool> conditions, Func<TModel, string> error, 
                                    Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(RuleType.ValidSynch, conditions, null!, null!, error, null!, memberSelector.Body, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ValidIf<T1,T2>(Func<TModel, bool> conditions, Func<TModel, string> error,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(RuleType.ValidSynch, conditions, null!, null!, error, null!, memberSelector1.Body, memberSelector2.Body, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ValidIf<T1,T2,T3>(Func<TModel, bool> conditions, Func<TModel, string> error,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(RuleType.ValidSynch, conditions, null!, null!, error, null!, memberSelector1.Body, memberSelector2.Body, memberSelector3.Body);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ValidIf<T1>(Func<TModel, bool> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(RuleType.ValidSynch, conditions, null!, null!, null!, errorMessage, memberSelector.Body, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ValidIf<T1, T2>(Func<TModel, bool> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(RuleType.ValidSynch, conditions, null!, null!, null!, errorMessage, memberSelector1.Body, memberSelector2.Body, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ValidIf<T1, T2, T3>(Func<TModel, bool> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(RuleType.ValidSynch, conditions, null!, null!, null!, errorMessage, memberSelector1.Body, memberSelector2.Body, memberSelector3.Body);

    #endregion // Synchronous ValidIf

    #region Asynchronous ValidIf

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public void ValidIf<T1>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error,
                            Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(RuleType.ValidAsync, conditions, null!, null!, error, null!, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public void ValidIf<T1,T2>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error,
                               Expression<Func<TModel, T1>> memberSelector1,
                               Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(RuleType.ValidAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public void ValidIf<T1,T2,T3>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error,
                               Expression<Func<TModel, T1>> memberSelector1,
                               Expression<Func<TModel, T2>> memberSelector2,
                               Expression<Func<TModel, T3>> memberSelector3)
       => rules.Add(RuleType.ValidAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, memberSelector3);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ValidIf<T1>(Func<TModel, ValueTask<bool>> conditions, string errorMessage,
                            Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(RuleType.ValidAsync, conditions, null!, null!, null!, errorMessage, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ValidIf<T1, T2>(Func<TModel, ValueTask<bool>> conditions, string errorMessage,
                            Expression<Func<TModel, T1>> memberSelector1,
                            Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(RuleType.ValidAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ValidIf<T1, T2, T3>(Func<TModel, ValueTask<bool>> conditions, string errorMessage,
                                  Expression<Func<TModel, T1>> memberSelector1,
                                  Expression<Func<TModel, T2>> memberSelector2,
                                  Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(RuleType.ValidAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, memberSelector3);

    #endregion // Asynchronous ValidIf

    #region Asynchronous ValidIf with Cancellation

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async with CancellationToken
    public void ValidIf<T1>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, Func<TModel, string> error,
                            Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(RuleType.ValidCancelledAsync, conditions, null!, null!, error, null!, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async with CancellationToken
    public void ValidIf<T1,T2>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, Func<TModel, string> error,
                               Expression<Func<TModel, T1>> memberSelector1,
                               Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(RuleType.ValidCancelledAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async with CancellationToken
    public void ValidIf<T1,T2,T3>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, Func<TModel, string> error,
                                  Expression<Func<TModel, T1>> memberSelector1,
                                  Expression<Func<TModel, T2>> memberSelector2,
                                  Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(RuleType.ValidCancelledAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, memberSelector3);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ValidIf<T1>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, string errorMessage,
                            Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(RuleType.ValidCancelledAsync, conditions, null!, null!, null!, errorMessage, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ValidIf<T1, T2>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, string errorMessage,
                            Expression<Func<TModel, T1>> memberSelector1,
                            Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(RuleType.ValidCancelledAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void ValidIf<T1, T2, T3>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, string errorMessage,
                                  Expression<Func<TModel, T1>> memberSelector1,
                                  Expression<Func<TModel, T2>> memberSelector2,
                                  Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(RuleType.ValidCancelledAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, memberSelector3);

    #endregion // Asynchronous ValidIf with Cancellation

    #region Synchronous WarnigIf

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void WarningIf<T1>(Func<TModel, bool> conditions, Func<TModel, string> warning,
                                    Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(RuleType.WarningSynch, conditions, null!, null!, warning, null!, memberSelector.Body, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void WarningIf<T1, T2>(Func<TModel, bool> conditions, Func<TModel, string> warning,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(RuleType.WarningSynch, conditions, null!, null!, warning, null!, memberSelector1.Body, memberSelector2.Body, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void WarningIf<T1, T2, T3>(Func<TModel, bool> conditions, Func<TModel, string> warning,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(RuleType.WarningSynch, conditions, null!, null!, warning, null!, memberSelector1.Body, memberSelector2.Body, memberSelector3.Body);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void WarningIf<T1>(Func<TModel, bool> conditions, string warningMessage,
                                    Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(RuleType.WarningSynch, conditions, null!, null!, null!, warningMessage, memberSelector.Body, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void WarningIf<T1, T2>(Func<TModel, bool> conditions, string warningMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(RuleType.WarningSynch, conditions, null!, null!, null!, warningMessage, memberSelector1.Body, memberSelector2.Body, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void WarningIf<T1, T2, T3>(Func<TModel, bool> conditions, string warningMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(RuleType.WarningSynch, conditions, null!, null!, null!, warningMessage, memberSelector1.Body, memberSelector2.Body, memberSelector3.Body);

    #endregion // Synchronous WarningIf

    #region Asynchronous WarningIf

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public void WarningIf<T1>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error,
                                 Expression<Func<TModel, T1>> memberSelector)
    => rules.Add(RuleType.WarningAsync, conditions, null!, null!, error, null!, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public void WarningIf<T1, T2>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error,
                               Expression<Func<TModel, T1>> memberSelector1,
                               Expression<Func<TModel, T2>> memberSelector2)
    => rules.Add(RuleType.WarningAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public void WarningIf<T1, T2, T3>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error,
                                  Expression<Func<TModel, T1>> memberSelector1,
                                  Expression<Func<TModel, T2>> memberSelector2,
                                  Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(RuleType.WarningAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, memberSelector3);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void WarningIf<T1>(Func<TModel, ValueTask<bool>> conditions, string errorMessage,
                            Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(RuleType.WarningAsync, conditions, null!, null!, null!, errorMessage, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void WarningIf<T1, T2>(Func<TModel, ValueTask<bool>> conditions, string errorMessage,
                                Expression<Func<TModel, T1>> memberSelector1,
                                Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(RuleType.WarningAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public void WarningIf<T1, T2, T3>(Func<TModel, ValueTask<bool>> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(RuleType.WarningAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, memberSelector3);

    #endregion // Asynchronous WarningIf


    #region Validate methods

    /// <summary>
    /// Validates the specified instance.
    /// </summary>
    /// <param name="model">The instance of the model to validate</param>
    /// <returns>A ValidationResult instance may contain some validation failures.</returns>
    public FlatValidationResult Validate(in TModel model) 
        => Validate(model, Timeout.InfiniteTimeSpan);

    /// <summary>
    /// Validates the specified instance.
    /// </summary>
    /// <param name="model">The instance of the model to validate</param>
    /// <param name="timeout">A <see cref="TimeSpan"/> that represents the number of milliseconds
    /// to wait, or a <see cref="TimeSpan"/> that represents -1 milliseconds to wait indefinitely.
    /// </param>
    /// <returns>A ValidationResult instance may contain some validation failures.</returns>
    public FlatValidationResult Validate(in TModel model, TimeSpan timeout)
    {
        var valueTask = ValidateAsync(model, timeout);
        if (valueTask.IsCompleted)
        {
            return valueTask.Result;
        }
        return valueTask.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Validate the specified instance asynchronously
    /// </summary>
    /// <param name="model">The instance of the model to validate</param>
    /// <param name="cancellation">The <see cref="CancellationToken"/> token to observe.</param>
    /// <returns>A ValidationResult instance may contain some validation failures.</returns>
    public ValueTask<FlatValidationResult> ValidateAsync(TModel model, CancellationToken cancellation = default)
        => ValidateAsync(model, Timeout.InfiniteTimeSpan, cancellation);

    /// <summary>
    /// Validate the specified instance asynchronously
    /// </summary>
    /// <param name="model">The instance of the model to validate</param>
    /// <param name="timeout">A <see cref="TimeSpan"/> that represents the number of milliseconds
    /// to wait, or a <see cref="TimeSpan"/> that represents -1 milliseconds to wait indefinitely.
    /// </param>
    /// <param name="cancellation">The <see cref="CancellationToken"/> token to observe.</param>
    /// <returns>A ValidationResult instance may contain some validation failures.</returns>
    public async ValueTask<FlatValidationResult> ValidateAsync(TModel model, TimeSpan timeout, CancellationToken cancellation = default)
    {
        if (!SpinWait.SpinUntil(() => Interlocked.CompareExchange(ref semaphoreState, 1, 0) == 1, timeout))
        {
            throw new TimeoutException();
        }
        cancellation.ThrowIfCancellationRequested();

        var snapshot = rules.MakeSnapshot();
        try
        {
            var validationResult = new FlatValidationResult(MetaData);
            await ProcessRules(0, snapshot.Count, validationResult);
            return validationResult;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return new FlatValidationResult(MetaData, ex);
        }
        finally
        {
            rules.RestoreSnapshot(snapshot);
            Interlocked.Exchange(ref semaphoreState, 0);
        }

        async ValueTask ProcessRules(int fromIndex, int toIndex, FlatValidationResult validationResult)
        {
            for (int i = fromIndex; i < toIndex; i++)
            {
                var rule = rules.GetRule(i);
                switch (rule.RuleType)
                {
                    case RuleType.ErrorSynch:
                        if (((Func<TModel, bool>)rule.Conditions)(model))
                        {
                            ProcessRuleErrors(rule, validationResult);
                        }
                        break;

                    case RuleType.ValidSynch:
                        if (!((Func<TModel, bool>)rule.Conditions)(model))
                        {
                            ProcessRuleErrors(rule, validationResult);
                        }
                        break;

                    case RuleType.ErrorAsync:
                        if (await ((Func<TModel, ValueTask<bool>>)rule.Conditions)(model))
                        {
                            ProcessRuleErrors(rule, validationResult);
                        }
                        break;

                    case RuleType.ErrorCancelledAsync:
                        if (await ((Func<TModel, CancellationToken, ValueTask<bool>>)rule.Conditions)(model, cancellation))
                        {
                            ProcessRuleErrors(rule, validationResult);
                        }
                        break;

                    case RuleType.ValidAsync:
                        if (!await ((Func<TModel, ValueTask<bool>>)rule.Conditions)(model))
                        {
                            ProcessRuleErrors(rule, validationResult);
                        }
                        break;

                    case RuleType.ValidCancelledAsync:
                        if (!await ((Func<TModel, CancellationToken, ValueTask<bool>>)rule.Conditions)(model, cancellation))
                        {
                            ProcessRuleErrors(rule, validationResult);
                        }
                        break;

                    case RuleType.ErrorConst:
                        ProcessRuleErrors(rule, validationResult);
                        break;

                    case RuleType.WarningSynch:
                        if (((Func<TModel, bool>)rule.Conditions)(model))
                        {
                            ProcessRuleWarnings(rule, validationResult);
                        }
                        break;

                    case RuleType.WarningAsync:
                        if (await ((Func<TModel, ValueTask<bool>>)rule.Conditions)(model))
                        {
                            ProcessRuleWarnings(rule, validationResult);
                        }
                        break;

                    case RuleType.WhenSynch:
                        if (((Func<TModel, bool>)rule.Conditions)(model))
                        {
                            if (rule.WhenThen is not null)
                            {
                                var startIndex = rules.Count;
                                ((Action<TModel>)rule.WhenThen)(model);
                                await ProcessRules(startIndex, rules.Count, validationResult);
                            }
                        }
                        else if (rule.WhenElse is not null)
                        {
                            var startIndex = rules.Count;
                            ((Action<TModel>)rule.WhenElse)(model);
                            await ProcessRules(startIndex, rules.Count, validationResult);
                        }
                        break;

                    case RuleType.WhenAsync:
                        if (await ((Func<TModel, ValueTask<bool>>)rule.Conditions)(model))
                        {
                            if (rule.WhenThen is not null)
                            {
                                var startIndex = rules.Count;
                                ((Action<TModel>)rule.WhenThen)(model);
                                await ProcessRules(startIndex, rules.Count, validationResult);
                            }
                        }
                        else if (rule.WhenElse is not null)
                        {
                            var startIndex = rules.Count;
                            ((Action<TModel>)rule.WhenElse)(model);
                            await ProcessRules(startIndex, rules.Count, validationResult);
                        }
                        break;

                    case RuleType.WhenCancelledAsync:
                        if (await ((Func<TModel, CancellationToken, ValueTask<bool>>)rule.Conditions)(model, cancellation))
                        {
                            if (rule.WhenThen is not null)
                            {
                                var startIndex = rules.Count;
                                ((Action<TModel, CancellationToken>)rule.WhenThen)(model, cancellation);
                                await ProcessRules(startIndex, rules.Count, validationResult);
                            }
                        }
                        else if (rule.WhenElse is not null)
                        {
                            var startIndex = rules.Count;
                            ((Action<TModel, CancellationToken>)rule.WhenElse)(model, cancellation);
                            await ProcessRules(startIndex, rules.Count, validationResult);
                        }
                        break;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ProcessRuleErrors(in Rule rule, FlatValidationResult validationResult)
        {
            var errorMessage = rule.FuncMessage is not null ? ((Func<TModel, string>)rule.FuncMessage)(model) : rule.ConstMessage;
            if (errorMessage is not null)
            {
                if (rule.MemberSelector1 is not null)
                {
                    validationResult.AddError(new FlatValidationError(rule.MemberSelector1.GetMemberName(), errorMessage));
                }
                if (rule.MemberSelector2 is not null)
                {
                    validationResult.AddError(new FlatValidationError(rule.MemberSelector2.GetMemberName(), errorMessage));
                }
                if (rule.MemberSelector3 is not null)
                {
                    validationResult.AddError(new FlatValidationError(rule.MemberSelector3.GetMemberName(), errorMessage));
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ProcessRuleWarnings(in Rule rule, FlatValidationResult validationResult)
        {
            var errorMessage = rule.FuncMessage is not null ? ((Func<TModel, string>)rule.FuncMessage)(model) : rule.ConstMessage;
            if (errorMessage is not null)
            {
                if (rule.MemberSelector1 is not null)
                {
                    validationResult.AddWarning(new FlatValidationWarning(rule.MemberSelector1.GetMemberName(), errorMessage));
                }
                if (rule.MemberSelector2 is not null)
                {
                    validationResult.AddWarning(new FlatValidationWarning(rule.MemberSelector2.GetMemberName(), errorMessage));
                }
                if (rule.MemberSelector3 is not null)
                {
                    validationResult.AddWarning(new FlatValidationWarning(rule.MemberSelector3.GetMemberName(), errorMessage));
                }
            }
        }
    }
    #endregion // Validate methods
}

public static partial class FlatValidator
{
    #region Static inline validation methods
    public static FlatValidationResult Validate<TModel>(in TModel model, Action<FlatValidator<TModel>> action)
    {
        var validator = new FlatValidator<TModel>();
        validator.When((_) => true, m => action(validator));
        return validator.Validate(model);
    }
    public static ValueTask<FlatValidationResult> ValidateAsync<TModel>(in TModel model, Action<FlatValidator<TModel>> action, CancellationToken cancellationToken = default)
    {
        var validator = new FlatValidator<TModel>();
        validator.When((_) => true, m => action(validator));
        return validator.ValidateAsync(model, cancellationToken);
    }
    #endregion // Static inline validation methods
}
