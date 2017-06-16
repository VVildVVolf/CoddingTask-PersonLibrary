using System.Collections.Generic;

namespace Operations.Aggregation.Printing{
    public interface IPrinter{
        void Print(IEnumerable<GroupingResult> source, IEnumerable<string> horisontal, IEnumerable<string> vertical);
    }
}