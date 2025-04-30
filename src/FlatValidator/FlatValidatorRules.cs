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

[StructLayout(LayoutKind.Auto)]
internal struct Rule
{
    internal RuleType RuleType;
    internal Delegate Conditions;
    internal Delegate WhenThen;
    internal Delegate WhenElse;
    internal Delegate FuncMessage;
    internal string ConstMessage;
    internal Expression MemberSelector1;
    internal Expression MemberSelector2;
    internal Expression MemberSelector3;
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

    private const int DefaultGrowth = 8;
    private Rule[] rules = new Rule[CapacityCache.Get<TModel>()];
    private int count = 0;

    internal int Count => count;

    #endregion // Members

    #region Constructors
    public FlatValidatorRules()
    { }
    #endregion // Constructors

    #region Methods

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ref Rule GetRule(int index) => ref rules[index];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Add(
        RuleType ruleType,
        Delegate conditions,
        Delegate whenThen,
        Delegate whenElse,
        Delegate error,
        string errorMessage,
        Expression memberSelector1,
        Expression memberSelector2,
        Expression memberSelector3)
    {
        if (count == rules.Length)
            Array.Resize(ref rules, rules.Length + DefaultGrowth);

        ref var rule = ref rules[count++];

        rule.RuleType = ruleType;
        rule.Conditions = conditions;
        rule.WhenThen = whenThen;
        rule.WhenElse = whenElse;
        rule.FuncMessage = error;
        rule.ConstMessage = errorMessage;
        rule.MemberSelector1 = memberSelector1;
        rule.MemberSelector2 = memberSelector2;
        rule.MemberSelector3 = memberSelector3;
    }

    internal Snapshot MakeSnapshot() => new Snapshot(count);
    internal void RestoreSnapshot(in Snapshot snapshot)
    {
        CapacityCache.Set<TModel>(count);

        count = snapshot.Count;
    }

    #endregion // Methods
}
