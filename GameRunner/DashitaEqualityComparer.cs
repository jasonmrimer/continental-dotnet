using System;
using System.Collections.Generic;
using System.Linq;

public class DashitaEqualityComparer : IEqualityComparer<Dashita>
{
    public bool Equals(Dashita x, Dashita y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Equals(y);
    }

    public int GetHashCode(Dashita obj)
    {
        //     IOrderedEnumerable<Run> orderedRuns = Runs.OrderBy(run => run[0].Suit);
        //     IOrderedEnumerable<Card> orderedAtama = Atama.OrderBy(card => card.Suit);
        //     int hashCode = HashCode.Combine(orderedAtama, orderedRuns);
        // List<Run> orderedRuns = new(obj.Runs.OrderBy(run => run[0].Suit));
        //
        // Atama orderedAtama = new(obj.Atama.OrderBy(card => card.Suit));
        //
        //
        // int hashCode = HashCode.Combine(orderedAtama, orderedRuns);
        //
        return 42;
    }
}