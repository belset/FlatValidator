using System.Linq.Expressions;
using System.Runtime.CompilerServices;


namespace System.Validation;

public class FleeValidator<TModel> : IFleeValidator<TModel>
{
    #region Members

    private FleeRuleList rules = new();

    #endregion // Members

    #region Grouped methods

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Grouped(Func<TModel, bool> conditions, Action<TModel> @then, Action<TModel> @else = null!)
        => rules.Add(FleeRuleType.GroupSynch, conditions, @then, @else, null!, null!, null!, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Grouped(Func<TModel, ValueTask<bool>> conditions, Action<TModel> @then, Action<TModel> @else = null!)
        => rules.Add(FleeRuleType.GroupAsync, conditions, @then, @else, null!, null!, null!, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Grouped(Func<TModel, CancellationToken, ValueTask<bool>> conditions, 
                            Action<TModel, CancellationToken> @then, Action<TModel, CancellationToken> @else = null!)
        => rules.Add(FleeRuleType.GroupCancelledAsync, conditions, @then, @else, null!, null!, null!, null!, null!);

    #endregion // Grouped methods

    #region Constant Error

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule Error<T1>(Func<TModel, string> error, Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(FleeRuleType.ErrorConst, null!, null!, null!, error, null!, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule Error<T1, T2>(Func<TModel, string> error,
                                Expression<Func<TModel, T1>> memberSelector1,
                                Expression<Func<TModel, T2>> memberSelector2)
    => rules.Add(FleeRuleType.ErrorConst, null!, null!, null!, error, null!, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule Error<T1, T2, T3>(Func<TModel, string> error,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(FleeRuleType.ErrorConst, null!, null!, null!, error, null!, memberSelector1, memberSelector2, memberSelector3);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule Error<T1>(string errorMessage, Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(FleeRuleType.ErrorConst, null!, null!, null!, null!, errorMessage, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule Error<T1, T2>(string errorMessage, Expression<Func<TModel, T1>> memberSelector1, Expression<Func<TModel, T2>> memberSelector2) 
        => rules.Add(FleeRuleType.ErrorConst, null!, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule Error<T1, T2, T3>(string errorMessage, Expression<Func<TModel, T1>> memberSelector1, Expression<Func<TModel, T2>> memberSelector2, Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(FleeRuleType.ErrorConst, null!, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, memberSelector3);

    #endregion // Constant Error

    #region Synchronous ErrorIf

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ErrorIf<T1>(Func<TModel, bool> conditions, Func<TModel, string> error, 
                                    Expression<Func<TModel, T1>> memberSelector) 
        => rules.Add(FleeRuleType.ErrorSynch, conditions, null!, null!, error, null!, memberSelector.Body, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ErrorIf<T1, T2>(Func<TModel, bool> conditions, Func<TModel, string> error,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2) 
        => rules.Add(FleeRuleType.ErrorSynch, conditions, null!, null!, error, null!, memberSelector1.Body, memberSelector2.Body, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ErrorIf<T1, T2, T3>(Func<TModel, bool> conditions, Func<TModel, string> error,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3) 
        => rules.Add(FleeRuleType.ErrorSynch, conditions, null!, null!, error, null!, memberSelector1.Body, memberSelector2.Body, memberSelector3.Body);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ErrorIf<T1>(Func<TModel, bool> conditions, string errorMessage, 
                                    Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(FleeRuleType.ErrorSynch, conditions, null!, null!, null!, errorMessage, memberSelector.Body, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ErrorIf<T1, T2>(Func<TModel, bool> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(FleeRuleType.ErrorSynch, conditions, null!, null!, null!, errorMessage, memberSelector1.Body, memberSelector2.Body, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ErrorIf<T1, T2, T3>(Func<TModel, bool> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(FleeRuleType.ErrorSynch, conditions, null!, null!, null!, errorMessage, memberSelector1.Body, memberSelector2.Body, memberSelector3.Body);

    #endregion // Synchronous ErrorIf

    #region Asynchronous ErrorIf

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public FleeRule ErrorIf<T1>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error, 
                                 Expression<Func<TModel, T1>> memberSelector)
    => rules.Add(FleeRuleType.ErrorAsync, conditions, null!, null!, error, null!, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public FleeRule ErrorIf<T1,T2>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error,
                               Expression<Func<TModel, T1>> memberSelector1,
                               Expression<Func<TModel, T2>> memberSelector2)
    => rules.Add(FleeRuleType.ErrorAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public FleeRule ErrorIf<T1,T2,T3>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error,
                                  Expression<Func<TModel, T1>> memberSelector1,
                                  Expression<Func<TModel, T2>> memberSelector2,
                                  Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(FleeRuleType.ErrorAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, memberSelector3);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ErrorIf<T1>(Func<TModel, ValueTask<bool>> conditions, string errorMessage, 
                            Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(FleeRuleType.ErrorAsync, conditions, null!, null!, null!, errorMessage, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ErrorIf<T1, T2>(Func<TModel, ValueTask<bool>> conditions, string errorMessage,
                                Expression<Func<TModel, T1>> memberSelector1,
                                Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(FleeRuleType.ErrorAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ErrorIf<T1, T2, T3>(Func<TModel, ValueTask<bool>> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(FleeRuleType.ErrorAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, memberSelector3);

    #endregion // Asynchronous ErrorIf

    #region Asynchronous ErrorIf with Cancellation
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async with CancellationToken
    public FleeRule ErrorIf<T1>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, Func<TModel, string> error,
                               Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(FleeRuleType.ErrorCancelledAsync, conditions, null!, null!, error, null!, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async with CancellationToken
    public FleeRule ErrorIf<T1,T2>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, Func<TModel, string> error, 
                               Expression<Func<TModel, T1>> memberSelector1,
                               Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(FleeRuleType.ErrorCancelledAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async with CancellationToken
    public FleeRule ErrorIf<T1,T2,T3>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, Func<TModel, string> error,
                                  Expression<Func<TModel, T1>> memberSelector1,
                                  Expression<Func<TModel, T2>> memberSelector2,
                                  Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(FleeRuleType.ErrorCancelledAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, memberSelector3);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ErrorIf<T1>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, string errorMessage,
                            Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(FleeRuleType.ErrorCancelledAsync, conditions, null!, null!, null!, errorMessage, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ErrorIf<T1, T2>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, string errorMessage,
                                Expression<Func<TModel, T1>> memberSelector1,
                                Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(FleeRuleType.ErrorCancelledAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ErrorIf<T1, T2, T3>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(FleeRuleType.ErrorCancelledAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, memberSelector3);

    #endregion // Asynchronous ErrorIf with Cancellation

    #region Synchronous ValidIf

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ValidIf<T1>(Func<TModel, bool> conditions, Func<TModel, string> error, 
                                    Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(FleeRuleType.ValidSynch, conditions, null!, null!, error, null!, memberSelector.Body, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ValidIf<T1,T2>(Func<TModel, bool> conditions, Func<TModel, string> error,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(FleeRuleType.ValidSynch, conditions, null!, null!, error, null!, memberSelector1.Body, memberSelector2.Body, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ValidIf<T1,T2,T3>(Func<TModel, bool> conditions, Func<TModel, string> error,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(FleeRuleType.ValidSynch, conditions, null!, null!, error, null!, memberSelector1.Body, memberSelector2.Body, memberSelector3.Body);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ValidIf<T1>(Func<TModel, bool> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(FleeRuleType.ValidSynch, conditions, null!, null!, null!, errorMessage, memberSelector.Body, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ValidIf<T1, T2>(Func<TModel, bool> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(FleeRuleType.ValidSynch, conditions, null!, null!, null!, errorMessage, memberSelector1.Body, memberSelector2.Body, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ValidIf<T1, T2, T3>(Func<TModel, bool> conditions, string errorMessage,
                                    Expression<Func<TModel, T1>> memberSelector1,
                                    Expression<Func<TModel, T2>> memberSelector2,
                                    Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(FleeRuleType.ValidSynch, conditions, null!, null!, null!, errorMessage, memberSelector1.Body, memberSelector2.Body, memberSelector3.Body);

    #endregion // Synchronous ValidIf

    #region Asynchronous ValidIf

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public FleeRule ValidIf<T1>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error,
                            Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(FleeRuleType.ValidAsync, conditions, null!, null!, error, null!, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public FleeRule ValidIf<T1,T2>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error,
                               Expression<Func<TModel, T1>> memberSelector1,
                               Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(FleeRuleType.ValidAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async
    public FleeRule ValidIf<T1,T2,T3>(Func<TModel, ValueTask<bool>> conditions, Func<TModel, string> error,
                               Expression<Func<TModel, T1>> memberSelector1,
                               Expression<Func<TModel, T2>> memberSelector2,
                               Expression<Func<TModel, T3>> memberSelector3)
       => rules.Add(FleeRuleType.ValidAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, memberSelector3);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ValidIf<T1>(Func<TModel, ValueTask<bool>> conditions, string errorMessage,
                            Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(FleeRuleType.ValidAsync, conditions, null!, null!, null!, errorMessage, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ValidIf<T1, T2>(Func<TModel, ValueTask<bool>> conditions, string errorMessage,
                            Expression<Func<TModel, T1>> memberSelector1,
                            Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(FleeRuleType.ValidAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ValidIf<T1, T2, T3>(Func<TModel, ValueTask<bool>> conditions, string errorMessage,
                                  Expression<Func<TModel, T1>> memberSelector1,
                                  Expression<Func<TModel, T2>> memberSelector2,
                                  Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(FleeRuleType.ValidAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, memberSelector3);

    #endregion // Asynchronous ValidIf

    #region Asynchronous ValidIf with Cancellation

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async with CancellationToken
    public FleeRule ValidIf<T1>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, Func<TModel, string> error,
                            Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(FleeRuleType.ValidCancelledAsync, conditions, null!, null!, error, null!, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async with CancellationToken
    public FleeRule ValidIf<T1,T2>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, Func<TModel, string> error,
                               Expression<Func<TModel, T1>> memberSelector1,
                               Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(FleeRuleType.ValidCancelledAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Async with CancellationToken
    public FleeRule ValidIf<T1,T2,T3>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, Func<TModel, string> error,
                                  Expression<Func<TModel, T1>> memberSelector1,
                                  Expression<Func<TModel, T2>> memberSelector2,
                                  Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(FleeRuleType.ValidCancelledAsync, conditions, null!, null!, error, null!, memberSelector1, memberSelector2, memberSelector3);

    //
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ValidIf<T1>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, string errorMessage,
                            Expression<Func<TModel, T1>> memberSelector)
        => rules.Add(FleeRuleType.ValidCancelledAsync, conditions, null!, null!, null!, errorMessage, memberSelector, null!, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ValidIf<T1, T2>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, string errorMessage,
                            Expression<Func<TModel, T1>> memberSelector1,
                            Expression<Func<TModel, T2>> memberSelector2)
        => rules.Add(FleeRuleType.ValidCancelledAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, null!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Synch
    public FleeRule ValidIf<T1, T2, T3>(Func<TModel, CancellationToken, ValueTask<bool>> conditions, string errorMessage,
                                  Expression<Func<TModel, T1>> memberSelector1,
                                  Expression<Func<TModel, T2>> memberSelector2,
                                  Expression<Func<TModel, T3>> memberSelector3)
        => rules.Add(FleeRuleType.ValidCancelledAsync, conditions, null!, null!, null!, errorMessage, memberSelector1, memberSelector2, memberSelector3);

    #endregion // Asynchronous ValidIf with Cancellation

    #region Validate methods
    /// <summary>
    /// Validates the specified instance.
    /// </summary>
    /// <param name="model">The instance of the model to validate</param>
    /// <returns>A ValidationResult instance may contain some validation failures.</returns>
    public FleeValidationResult Validate(in TModel model)
    {
        var valueTask = ValidateAsync(model);
        if (valueTask.IsCompleted)
        {
            return valueTask.Result;
        }
        //return Task.Run(async () => await valueTask).Result;
        return valueTask.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Validate the specified instance asynchronously
    /// </summary>
    /// <param name="model">The instance of the model to validate</param>
    /// <param name="cancellation"></param>
    /// <returns>A ValidationResult instance may contain some validation failures.</returns>
    public async ValueTask<FleeValidationResult> ValidateAsync(TModel model, CancellationToken cancellation = default)
    {
        var stash = await rules.MakeStash();
        try
        {
            var validationResult = new FleeValidationResult();
            await ProcessRules(0, stash.Count, validationResult);
            return validationResult;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return new FleeValidationResult(ex);
        }
        finally
        {
            rules.Restore(stash);
        }

        async ValueTask ProcessRules(int fromIndex, int toIndex, FleeValidationResult validationResult)
        {
            for (int i = fromIndex; i < toIndex; i++)
            {
                var rule = rules.GetFleeRule(i);
                switch (rule.RuleType)
                {
                    case FleeRuleType.ErrorSynch:
                        if (((Func<TModel, bool>)rule.Conditions)(model))
                        {
                            ProcessRuleErrors(rule, validationResult);
                        }
                        break;

                    case FleeRuleType.ValidSynch:
                        if (!((Func<TModel, bool>)rule.Conditions)(model))
                        {
                            ProcessRuleErrors(rule, validationResult);
                        }
                        break;

                    case FleeRuleType.ErrorConst:
                        ProcessRuleErrors(rule, validationResult);
                        break;

                    case FleeRuleType.ErrorAsync:
                        if (await ((Func<TModel, ValueTask<bool>>)rule.Conditions)(model))
                        {
                            ProcessRuleErrors(rule, validationResult);
                        }
                        break;

                    case FleeRuleType.ErrorCancelledAsync:
                        if (await ((Func<TModel, CancellationToken, ValueTask<bool>>)rule.Conditions)(model, cancellation))
                        {
                            ProcessRuleErrors(rule, validationResult);
                        }
                        break;

                    case FleeRuleType.ValidAsync:
                        if (!await ((Func<TModel, ValueTask<bool>>)rule.Conditions)(model))
                        {
                            ProcessRuleErrors(rule, validationResult);
                        }
                        break;

                    case FleeRuleType.ValidCancelledAsync:
                        if (!await ((Func<TModel, CancellationToken, ValueTask<bool>>)rule.Conditions)(model, cancellation))
                        {
                            ProcessRuleErrors(rule, validationResult);
                        }
                        break;

                    case FleeRuleType.GroupSynch:
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

                    case FleeRuleType.GroupAsync:
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

                    case FleeRuleType.GroupCancelledAsync:
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
        void ProcessRuleErrors(in FleeRule rule, FleeValidationResult validationResult)
        {
            var errorMessage = rule.Error is not null ? ((Func<TModel, string>)rule.Error)(model) : rule.ErrorMessage;
            if (errorMessage is not null)
            {
                ProcessError(errorMessage, rule.Tag, rule.MemberSelector1, validationResult);
                ProcessError(errorMessage, rule.Tag, rule.MemberSelector2, validationResult);
                ProcessError(errorMessage, rule.Tag, rule.MemberSelector3, validationResult);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ProcessError(string errorMessage, string tag, Expression memberSelector, FleeValidationResult validationResult)
        {
            if (memberSelector is not null)
            {
                var memberPath = memberSelector.GetMemberName();
                validationResult.AddError(new FleeValidationError(memberPath, errorMessage, tag));
            }
        }
    }
    #endregion // Validate methods
}

public static class FleeValidator
{
    #region Static inline validation methods
    public static FleeValidationResult Validate<TModel>(in TModel model, Action<FleeValidator<TModel>> action)
    {
        var validator = new FleeValidator<TModel>();
        validator.Grouped((_) => true, m => action(validator));
        return validator.Validate(model);
    }
    public static ValueTask<FleeValidationResult> ValidateAsync<TModel>(in TModel model, Action<FleeValidator<TModel>> action, CancellationToken cancellationToken = default)
    {
        var validator = new FleeValidator<TModel>();
        validator.Grouped((_) => true, m => action(validator));
        return validator.ValidateAsync(model, cancellationToken);
    }
    #endregion // Static inline validation methods
}
