using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace System.Validation;

internal enum RuleType : int
{
    ErrorSynch,
    ValidSynch,

    ErrorAsync,
    ValidAsync,

    ErrorCancelledAsync,
    ValidCancelledAsync,

    ErrorConst,

    WarningSynch,
    WarningAsync,

    WhenSynch,
    WhenAsync,
    WhenCancelledAsync,
}

[StructLayout(LayoutKind.Sequential)]
internal readonly struct Rule<TModel>(
        RuleType ruleType,
        Delegate conditions,
        Delegate whenThen,
        Delegate whenElse,
        Func<TModel, string> messageFunc,
        string message,
        Expression memberSelector1,
        Expression memberSelector2,
        Expression memberSelector3)
{
    internal readonly RuleType RuleType = ruleType;
    internal readonly Delegate Conditions = conditions;
    internal readonly Delegate WhenThen = whenThen;
    internal readonly Delegate WhenElse = whenElse;
    internal readonly Func<TModel, string> ErrorFunc = messageFunc;
    internal readonly string ErrorText = message;
    internal readonly Expression MemberSelector1 = memberSelector1;
    internal readonly Expression MemberSelector2 = memberSelector2;
    internal readonly Expression MemberSelector3 = memberSelector3;
}

internal struct FlatValidatorRules<TModel>
{
    #region Nested classes
    internal struct Snapshot(int count) 
    {
        internal readonly int Count = count;
    };
    #endregion // Nested classes

    #region Members

    private Rule<TModel>[] rules = new Rule<TModel>[16];
    private int count = 0;

    internal int Count => count;

    #endregion // Members

    #region Constructors
    public FlatValidatorRules()
    { }
    #endregion // Constructors

    #region Methods
    
    internal ref readonly Rule<TModel> this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref rules[index];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Add(in Rule<TModel> rule) // Передаем по ссылке для чтения
    {
        if (count == rules.Length)
            Array.Resize(ref rules, rules.Length + (rules.Length >> 2) + 4);

        rules[count++] = rule;
    }

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //internal void Add(
    //    RuleType ruleType,
    //    Delegate conditions,
    //    Delegate whenThen,
    //    Delegate whenElse,
    //    Func<TModel, string> errorFunc,
    //    string errorMessage,
    //    Expression memberSelector1,
    //    Expression memberSelector2,
    //    Expression memberSelector3)
    //{
    //    if (count == rules.Length)
    //        Array.Resize(ref rules, rules.Length + (rules.Length >> 2) + 4);

    //    rules[count++] = new Rule<TModel>(
    //        ruleType,
    //        conditions,
    //        whenThen,
    //        whenElse,
    //        errorFunc,
    //        errorMessage,
    //        memberSelector1,
    //        memberSelector2,
    //        memberSelector3);
    //}

    internal Snapshot MakeSnapshot() => new Snapshot(count);
    internal void RestoreSnapshot(in Snapshot snapshot)
    {
        count = snapshot.Count;
    }

    #endregion // Methods
}
