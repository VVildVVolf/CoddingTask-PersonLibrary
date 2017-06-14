using System;
using System.Collections.Generic;

namespace Operations.Aggregation {
    public interface IAggregator {
        IEnumerable<GroupingResult> Get<T, TA>(IEnumerable<T> source, IEnumerable<string> verticalGroupping, IEnumerable<string> horisontalGroupping, Func<IEnumerable<T>, TA> aggregation);
    }
}