using System.Linq.Expressions;
using System.Runtime.CompilerServices;


namespace System.Validation;

public class FlatValidator<TModel> : IFlatValidator<TModel>
{
    #region Members

    private int semaphoreCount = 0;
    private int semaphoreState = 0; // 0 means unset, 1 means set.
    private SemaphoreSlim? semaphore = null;

    private RuleList<TModel> rules = new();

    #endregion // Members

    #region Grouped methods

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Grouped(Func<TModel, bool> conditions, Action<TModel> @then, Action<TModel> @else = null!)
        => rules.Add(RuleType.GroupSynch, conditions, @then, @else, null!, null!, null!, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Grouped(Func<TModel, ValueTask<bool>> conditions, Action<TModel> @then, Action<TModel> @else = null!)
        => rules.Add(RuleType.GroupAsync, conditions, @then, @else, null!, null!, null!, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Grouped(Func<TModel, CancellationToken, ValueTask<bool>> conditions, 
                            Action<TModel, CancellationToken> @then, Action<TModel, CancellationToken> @else = null!)
        => rules.Add(RuleType.GroupCancelledAsync, conditions, @then, @else, null!, null!, null!, null!, null!);

    #endregion // Grouped methods

    #region Constant Error

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule Error<T1>(Func<TModel, string> error, Expression<Func<TModel, T1>> memberSelector)
        => ref rules.Add(RuleType.ErrorConst, null!, null!, null!, error, null!, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule Error<T1, T2>(Func<TModel, string> error,
                                Expression<Func<TModel, T1>> memberSelector1,
                                Expression<Func<TModel, T2>> memberSelector2)
    => ref rules.Add(RuleType.ErrorConst, null!, null!, null!, error, null!, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule Error<T1, T2, T3>(Func<TModel, string> error,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => ref rules.Add(RuleType.ErrorConst, null!, null!, null!, error, null!, memberSelector1, memberSelector2, memberSelector3);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule Error<T1>(string errorMessage, Expression<Func<TModel, T1>> memberSelector)
        => ref rules.Add(RuleType.ErrorConst, null!, null!, null!, null!, errorMessage, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule Error<T1, T2>(string errorMessage, Expression<Func<TModel, T1>> memberSelector1, Expression<Func<TModel, T2>> memberSelector2) 
        => ref rules.Add(RuleType.ErrorConst, null!, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule Error<T1, T2, T3>(string errorMessage, Expression<Func<TModel, T1>> memberSelector1, Expression<Func<TModel, T2>> memberSelector2, Expression<Func<TModel, T3>> memberSelector3)
        => ref rules.Add(RuleType.ErrorConst, null!, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, memberSelector3);

    #endregion // Constant Error

    #region Synchronous ErrorIf

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ErrorIf<T1>(Func<TModel, bool> conditions, Func<TModel, string> error, 
                                    Expression<Func<TModel, T1>> memberSelector) 
        => ref rules.Add(RuleType.ErrorSynch, conditions, null!, null!, error, null!, memberSelector.Body, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ErrorIf<T1, T2>(Func<TModel, bool> conditions, Func<TModel, string> error,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2) 
        => ref rules.Add(RuleType.ErrorSynch, conditions, null!, null!, error, null!, memberSelector1.Body, memberSelector2.Body, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ErrorIf<T1, T2, T3>(Func<TModel, bool> conditions, Func<TModel, string> error,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3) 
        => ref rules.Add(RuleType.ErrorSynch, conditions, null!, null!, error, null!, memberSelector1.Body, memberSelector2.Body, memberSelector3.Body);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ErrorIf<T1>(Func<TModel, bool> conditions, string errorMessage, 
                                    Expression<Func<TModel, T1>> memberSelector)
        => ref rules.Add(RuleType.ErrorSynch, conditions, null!, null!, null!, errorMessage, memberSelector.Body, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ErrorIf<T1, T2>(Func<TModel, bool> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2)
        => ref rules.Add(RuleType.ErrorSynch, conditions, null!, null!, null!, errorMessage, memberSelector1.Body, memberSelector2.Body, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ErrorIf<T1, T2, T3>(Func<TModel, bool> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => ref rules.Add(RuleType.ErrorSynch, conditions, null!, null!, null!, errorMessage, memberSelector1.Body, memberSelector2.Body, memberSelector3.Body);

    #endregion // Synchronous ErrorIf

    #region Asynchronous ErrorIf

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public ref Rule ErrorIf<T1>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error, 
                                 Expression<Func<TModel, T1>> memberSelector)
    => ref rules.Add(RuleType.ErrorAsync, conditions, null!, null!, error, null!, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public ref Rule ErrorIf<T1,T2>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error,
                               Expression<Func<TModel, T1>> memberSelector1,
                               Expression<Func<TModel, T2>> memberSelector2)
    => ref rules.Add(RuleType.ErrorAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public ref Rule ErrorIf<T1,T2,T3>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error,
                                  Expression<Func<TModel, T1>> memberSelector1,
                                  Expression<Func<TModel, T2>> memberSelector2,
                                  Expression<Func<TModel, T3>> memberSelector3)
        => ref rules.Add(RuleType.ErrorAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, memberSelector3);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ErrorIf<T1>(Func<TModel, ValueTask<bool>> conditions, string errorMessage, 
                            Expression<Func<TModel, T1>> memberSelector)
        => ref rules.Add(RuleType.ErrorAsync, conditions, null!, null!, null!, errorMessage, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ErrorIf<T1, T2>(Func<TModel, ValueTask<bool>> conditions, string errorMessage,
                                Expression<Func<TModel, T1>> memberSelector1,
                                Expression<Func<TModel, T2>> memberSelector2)
        => ref rules.Add(RuleType.ErrorAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ErrorIf<T1, T2, T3>(Func<TModel, ValueTask<bool>> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => ref rules.Add(RuleType.ErrorAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, memberSelector3);

    #endregion // Asynchronous ErrorIf

    #region Asynchronous ErrorIf with Cancellation
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async with CancellationToken
    public ref Rule ErrorIf<T1>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, Func<TModel, string> error,
                               Expression<Func<TModel, T1>> memberSelector)
        => ref rules.Add(RuleType.ErrorCancelledAsync, conditions, null!, null!, error, null!, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async with CancellationToken
    public ref Rule ErrorIf<T1,T2>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, Func<TModel, string> error, 
                               Expression<Func<TModel, T1>> memberSelector1,
                               Expression<Func<TModel, T2>> memberSelector2)
        => ref rules.Add(RuleType.ErrorCancelledAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async with CancellationToken
    public ref Rule ErrorIf<T1,T2,T3>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, Func<TModel, string> error,
                                  Expression<Func<TModel, T1>> memberSelector1,
                                  Expression<Func<TModel, T2>> memberSelector2,
                                  Expression<Func<TModel, T3>> memberSelector3)
        => ref rules.Add(RuleType.ErrorCancelledAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, memberSelector3);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ErrorIf<T1>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, string errorMessage,
                            Expression<Func<TModel, T1>> memberSelector)
        => ref rules.Add(RuleType.ErrorCancelledAsync, conditions, null!, null!, null!, errorMessage, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ErrorIf<T1, T2>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, string errorMessage,
                                Expression<Func<TModel, T1>> memberSelector1,
                                Expression<Func<TModel, T2>> memberSelector2)
        => ref rules.Add(RuleType.ErrorCancelledAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ErrorIf<T1, T2, T3>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => ref rules.Add(RuleType.ErrorCancelledAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, memberSelector3);

    #endregion // Asynchronous ErrorIf with Cancellation

    #region Synchronous ValidIf

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ValidIf<T1>(Func<TModel, bool> conditions, Func<TModel, string> error, 
                                    Expression<Func<TModel, T1>> memberSelector)
        => ref rules.Add(RuleType.ValidSynch, conditions, null!, null!, error, null!, memberSelector.Body, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ValidIf<T1,T2>(Func<TModel, bool> conditions, Func<TModel, string> error,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2)
        => ref rules.Add(RuleType.ValidSynch, conditions, null!, null!, error, null!, memberSelector1.Body, memberSelector2.Body, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ValidIf<T1,T2,T3>(Func<TModel, bool> conditions, Func<TModel, string> error,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => ref rules.Add(RuleType.ValidSynch, conditions, null!, null!, error, null!, memberSelector1.Body, memberSelector2.Body, memberSelector3.Body);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ValidIf<T1>(Func<TModel, bool> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector)
        => ref rules.Add(RuleType.ValidSynch, conditions, null!, null!, null!, errorMessage, memberSelector.Body, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ValidIf<T1, T2>(Func<TModel, bool> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2)
        => ref rules.Add(RuleType.ValidSynch, conditions, null!, null!, null!, errorMessage, memberSelector1.Body, memberSelector2.Body, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ValidIf<T1, T2, T3>(Func<TModel, bool> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => ref rules.Add(RuleType.ValidSynch, conditions, null!, null!, null!, errorMessage, memberSelector1.Body, memberSelector2.Body, memberSelector3.Body);

    #endregion // Synchronous ValidIf

    #region Asynchronous ValidIf

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public ref Rule ValidIf<T1>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error,
                            Expression<Func<TModel, T1>> memberSelector)
        => ref rules.Add(RuleType.ValidAsync, conditions, null!, null!, error, null!, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public ref Rule ValidIf<T1,T2>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error,
                               Expression<Func<TModel, T1>> memberSelector1,
                               Expression<Func<TModel, T2>> memberSelector2)
        => ref rules.Add(RuleType.ValidAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public ref Rule ValidIf<T1,T2,T3>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error,
                               Expression<Func<TModel, T1>> memberSelector1,
                               Expression<Func<TModel, T2>> memberSelector2,
                               Expression<Func<TModel, T3>> memberSelector3)
       => ref rules.Add(RuleType.ValidAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, memberSelector3);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ValidIf<T1>(Func<TModel, ValueTask<bool>> conditions, string errorMessage,
                            Expression<Func<TModel, T1>> memberSelector)
        => ref rules.Add(RuleType.ValidAsync, conditions, null!, null!, null!, errorMessage, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ValidIf<T1, T2>(Func<TModel, ValueTask<bool>> conditions, string errorMessage,
                            Expression<Func<TModel, T1>> memberSelector1,
                            Expression<Func<TModel, T2>> memberSelector2)
        => ref rules.Add(RuleType.ValidAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ValidIf<T1, T2, T3>(Func<TModel, ValueTask<bool>> conditions, string errorMessage,
                                  Expression<Func<TModel, T1>> memberSelector1,
                                  Expression<Func<TModel, T2>> memberSelector2,
                                  Expression<Func<TModel, T3>> memberSelector3)
        => ref rules.Add(RuleType.ValidAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, memberSelector3);

    #endregion // Asynchronous ValidIf

    #region Asynchronous ValidIf with Cancellation

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async with CancellationToken
    public ref Rule ValidIf<T1>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, Func<TModel, string> error,
                            Expression<Func<TModel, T1>> memberSelector)
        => ref rules.Add(RuleType.ValidCancelledAsync, conditions, null!, null!, error, null!, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async with CancellationToken
    public ref Rule ValidIf<T1,T2>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, Func<TModel, string> error,
                               Expression<Func<TModel, T1>> memberSelector1,
                               Expression<Func<TModel, T2>> memberSelector2)
        => ref rules.Add(RuleType.ValidCancelledAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async with CancellationToken
    public ref Rule ValidIf<T1,T2,T3>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, Func<TModel, string> error,
                                  Expression<Func<TModel, T1>> memberSelector1,
                                  Expression<Func<TModel, T2>> memberSelector2,
                                  Expression<Func<TModel, T3>> memberSelector3)
        => ref rules.Add(RuleType.ValidCancelledAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, memberSelector3);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ValidIf<T1>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, string errorMessage,
                            Expression<Func<TModel, T1>> memberSelector)
        => ref rules.Add(RuleType.ValidCancelledAsync, conditions, null!, null!, null!, errorMessage, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ValidIf<T1, T2>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, string errorMessage,
                            Expression<Func<TModel, T1>> memberSelector1,
                            Expression<Func<TModel, T2>> memberSelector2)
        => ref rules.Add(RuleType.ValidCancelledAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public ref Rule ValidIf<T1, T2, T3>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, string errorMessage,
                                  Expression<Func<TModel, T1>> memberSelector1,
                                  Expression<Func<TModel, T2>> memberSelector2,
                                  Expression<Func<TModel, T3>> memberSelector3)
        => ref rules.Add(RuleType.ValidCancelledAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, memberSelector3);

    #endregion // Asynchronous ValidIf with Cancellation

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
        bool needToRelease = await InitializeValidation(timeout, cancellation);
        var snapshot = rules.MakeSnapshot();
        try
        {
            var validationResult = new FlatValidationResult();
            await ProcessRules(0, snapshot.Count, validationResult);
            return validationResult;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return new FlatValidationResult(ex);
        }
        finally
        {
            rules.RestoreSnapshot(snapshot);
            FinalizeValidation(needToRelease);
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

                    case RuleType.ErrorConst:
                        ProcessRuleErrors(rule, validationResult);
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

                    case RuleType.GroupSynch:
                        var groupSyncResult = ((Func<TModel, bool>)rule.Conditions)(model);
                        if (groupSyncResult && rule.GroupThen is not null)
                        {
                            var groupIndex = rules.Count;
                            ((Action<TModel>)rule.GroupThen)(model);
                            await ProcessRules(groupIndex, rules.Count, validationResult);
                        }
                        else if (!groupSyncResult && rule.GroupElse is not null)
                        {
                            var groupIndex = rules.Count;
                            ((Action<TModel>)rule.GroupElse)(model);
                            await ProcessRules(groupIndex, rules.Count, validationResult);
                        }
                        break;

                    case RuleType.GroupAsync:
                        var groupAsyncResult = await ((Func<TModel, ValueTask<bool>>)rule.Conditions)(model);
                        if (groupAsyncResult && rule.GroupThen is not null)
                        {
                            var groupIndex = rules.Count;
                            ((Action<TModel>)rule.GroupThen)(model);
                            await ProcessRules(groupIndex, rules.Count, validationResult);
                        }
                        else if (!groupAsyncResult && rule.GroupElse is not null)
                        {
                            var groupIndex = rules.Count;
                            ((Action<TModel>)rule.GroupElse)(model);
                            await ProcessRules(groupIndex, rules.Count, validationResult);
                        }
                        break;

                    case RuleType.GroupCancelledAsync:
                        var groupCancelledAsyncResult = await ((Func<TModel, CancellationToken, ValueTask<bool>>)rule.Conditions)(model, cancellation);
                        if (groupCancelledAsyncResult && rule.GroupThen is not null)
                        {
                            var groupIndex = rules.Count;
                            ((Action<TModel, CancellationToken>)rule.GroupThen)(model, cancellation);
                            await ProcessRules(groupIndex, rules.Count, validationResult);
                        }
                        else if (!groupCancelledAsyncResult && rule.GroupElse is not null)
                        {
                            var groupIndex = rules.Count;
                            ((Action<TModel, CancellationToken>)rule.GroupElse)(model, cancellation);
                            await ProcessRules(groupIndex, rules.Count, validationResult);
                        }
                        break;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ProcessRuleErrors(in Rule rule, FlatValidationResult validationResult)
        {
            var errorMessage = rule.Error is not null ? ((Func<TModel, string>)rule.Error)(model) : rule.ErrorMessage;
            if (errorMessage is not null)
            {
                ProcessError(errorMessage, rule.MemberSelector1, validationResult);
                ProcessError(errorMessage, rule.MemberSelector2, validationResult);
                ProcessError(errorMessage, rule.MemberSelector3, validationResult);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ProcessError(string errorMessage, Expression memberSelector, FlatValidationResult validationResult)
        {
            if (memberSelector is not null)
            {
                var memberPath = memberSelector.GetMemberName();
                validationResult.AddError(new FlatValidationError(memberPath, errorMessage));
            }
        }
    }
    #endregion // Validate methods

    #region Validation processing

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    async ValueTask<bool> InitializeValidation(TimeSpan timeout, CancellationToken cancellation)
    {
        if (Interlocked.Increment(ref semaphoreCount) > 1)
        {
            SpinWait spinner = default;
            while (Interlocked.CompareExchange(ref semaphoreCount, 0, 0) > 1)
            {
                spinner.SpinOnce();

                if (spinner.NextSpinWillYield)
                {
                    SpinWait.SpinUntil(() => Interlocked.CompareExchange(ref semaphoreState, 1, 0) == 1);
                    semaphore ??= new SemaphoreSlim(1, 1);
                    Interlocked.Exchange(ref semaphoreState, 0);
                    if (!await semaphore.WaitAsync(timeout, cancellation))
                    {
                        FinalizeValidation(false);
                        cancellation.ThrowIfCancellationRequested();
                        throw new TimeoutException();
                    }
                    return true;
                }
            }
        }
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void FinalizeValidation(bool needToRelease)
    {
        if (needToRelease) 
            semaphore!.Release();

        if (Interlocked.Decrement(ref semaphoreCount) == 0)
        {
            SpinWait.SpinUntil(() => Interlocked.CompareExchange(ref semaphoreState, 1, 0) == 1);
            semaphore?.Dispose();
            semaphore = null;
            Interlocked.Exchange(ref semaphoreState, 0);
        }
    }
    #endregion // Validation processing
}

public static class FlatValidator
{
    #region Static inline validation methods
    public static FlatValidationResult Validate<TModel>(in TModel model, Action<FlatValidator<TModel>> action)
    {
        var validator = new FlatValidator<TModel>();
        validator.Grouped((_) => true, m => action(validator));
        return validator.Validate(model);
    }
    public static ValueTask<FlatValidationResult> ValidateAsync<TModel>(in TModel model, Action<FlatValidator<TModel>> action, CancellationToken cancellationToken = default)
    {
        var validator = new FlatValidator<TModel>();
        validator.Grouped((_) => true, m => action(validator));
        return validator.ValidateAsync(model, cancellationToken);
    }
    #endregion // Static inline validation methods
}
