namespace System.Validation;

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
