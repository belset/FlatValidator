using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Validation;

internal enum FleeRuleType : int
{
    ErrorSynch,
    ValidSynch,

    ErrorAsync,
    ValidAsync,

    ErrorCancelledAsync,
    ValidCancelledAsync,

    ErrorConst,

    GroupSynch,
    GroupAsync,
    GroupCancelledAsync,
}

[StructLayout(LayoutKind.Auto)]
public class FleeRule
{
    internal FleeRuleType RuleType;
    internal Delegate Conditions;
    internal Delegate GroupThen;
    internal Delegate GroupElse;
    internal Delegate Error;
    internal string ErrorMessage;
    internal Expression MemberSelector1;
    internal Expression MemberSelector2;
    internal Expression MemberSelector3;
    internal string Tag;

    internal FleeRule(
        FleeRuleType ruleType,
        Delegate conditions,
        Delegate groupThen,
        Delegate groupElse,
        Delegate error,
        string errorMessage,
        Expression memberSelector1,
        Expression memberSelector2,
        Expression memberSelector3,
        string tag = null!
    )
    {
        RuleType = ruleType;
        Conditions = conditions;
        GroupThen = groupThen;
        GroupElse = groupElse;
        Error = error;
        ErrorMessage = errorMessage;
        MemberSelector1 = memberSelector1;
        MemberSelector2 = memberSelector2;
        MemberSelector3 = memberSelector3;
        Tag = tag ?? string.Empty;
    }
}

public static class FleeRuleExtensions
{
    public static FleeRule Tag(this FleeRule rule, string tag)
    {
        rule.Tag = tag;
        return rule;
    }
}

internal class FleeRuleList
{
    internal struct Stash
    {
        internal readonly int Count;
        internal readonly bool NeedToRelease;
        internal Stash(int count, bool needToRelease)
        {
            Count = count;
            NeedToRelease = needToRelease;
        }
    }

    private int lockState = 0; // 0 means unset, 1 means set.
    private int semaphoreState = 0; // 0 means unset, 1 means set.
    private int semaphoreUsage = 0;
    private SemaphoreSlim? semaphore = null;

    private static volatile int DefaultCapacity = 8;
    private const int DefaultGrowth = 8;

    private FleeRule[] rules = new FleeRule[DefaultCapacity];
    private int count = 0;

    internal int Count => count;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FleeRule GetFleeRule(int index) => rules[index];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FleeRule Add(
        FleeRuleType ruleType,
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

        //ref var rule = ref rules[count];
        //rule.RuleType = ruleType;
        //rule.Conditions = conditions;
        //rule.GroupThen = groupThen;
        //rule.GroupElse = groupElse;
        //rule.Error = error;
        //rule.ErrorMessage = errorMessage;
        //rule.MemberSelector1 = memberSelector1;
        //rule.MemberSelector2 = memberSelector2;
        //rule.MemberSelector3 = memberSelector3;
        //rule.Tag = string.Empty;

        //return ref rules[count++];

        rules[count] = new FleeRule(ruleType, conditions, groupThen, groupElse, error, errorMessage, memberSelector1, memberSelector2, memberSelector3);
        return rules[count++];
    }

    internal async ValueTask<Stash> MakeStash()
    {
        SpinWait spinner = default;
        Interlocked.Increment(ref lockState);
        while (Interlocked.CompareExchange(ref lockState, 0, 0) > 1)
        {
            spinner.SpinOnce();

            if (spinner.NextSpinWillYield)
            {
                while (Interlocked.CompareExchange(ref semaphoreState, 1, 0) == 1) spinner.SpinOnce();
                if (++semaphoreUsage == 1) Debug.Assert(semaphore is null);
                semaphore ??= new SemaphoreSlim(1, 1);
                Interlocked.Exchange(ref semaphoreState, 0);
                await semaphore!.WaitAsync();
                return new Stash(Count, needToRelease: true);
            }
        }
        return new Stash(Count, needToRelease: false);
    }

    internal void Restore(Stash stash)
    {
        var capacity = DefaultCapacity;
        DefaultCapacity = (((capacity + capacity + capacity + count) >> 3) + 1) << 1;

        count = stash.Count;
        if (stash.NeedToRelease)
        {
            SpinWait spinner = default;
            while (Interlocked.CompareExchange(ref semaphoreState, 1, 0) == 1) spinner.SpinOnce();
            Debug.Assert(semaphore is not null);
            semaphore!.Release();
            if (--semaphoreUsage == 0)
            {
                Debug.Assert(semaphore.CurrentCount == 1);
                semaphore!.Dispose();
                semaphore = null;
            }
            Interlocked.Exchange(ref semaphoreState, 0);
        }
        Interlocked.Decrement(ref lockState);
    }
}
