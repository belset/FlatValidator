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

    IfSynch,
    IfAsync,
    IfCancelledAsync,
}

[StructLayout(LayoutKind.Auto)]
public struct Rule
{
    internal RuleType RuleType;
    internal Delegate Conditions;
    internal Delegate IfThen;
    internal Delegate IfElse;
    internal Delegate FuncMessage;
    internal string ConstMessage;
    internal Expression MemberSelector1;
    internal Expression MemberSelector2;
    internal Expression MemberSelector3;
}

internal class CapacityCache
{
    internal const int DefaultCapacity = 8;
    static int[] capacities = new int[256];

    static CapacityCache()
    {
        Array.Fill(capacities, DefaultCapacity);
    }

    internal static int Get<TModel>()
    {
        return Volatile.Read(ref capacities[typeof(TModel).GetHashCode() & 0xFF]);
    }
    internal static void Set<TModel>(int value)
    {
        var index = typeof(TModel).GetHashCode() & 0xFF;
        var capacity = Volatile.Read(ref capacities[index]);
        capacity = ((capacity + capacity + capacity + value) >> 2) + 1;
        Volatile.Write(ref capacities[index], capacity);
    }
}

internal struct RuleList<TModel>
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
    public RuleList()
    { }
    #endregion // Constructors

    #region Methods

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ref Rule GetRule(int index) => ref rules[index];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ref Rule Add(
        RuleType ruleType,
        Delegate conditions,
        Delegate groupThen,
        Delegate groupElse,
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
        rule.IfThen = groupThen;
        rule.IfElse = groupElse;
        rule.FuncMessage = error;
        rule.ConstMessage = errorMessage;
        rule.MemberSelector1 = memberSelector1;
        rule.MemberSelector2 = memberSelector2;
        rule.MemberSelector3 = memberSelector3;

        return ref rule;
    }

    internal Snapshot MakeSnapshot() => new Snapshot(count);
    internal void RestoreSnapshot(in Snapshot snapshot)
    {
        CapacityCache.Set<TModel>(count);

        count = snapshot.Count;
    }

    #endregion // Methods
}
