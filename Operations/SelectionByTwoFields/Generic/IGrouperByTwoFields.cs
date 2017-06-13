using System;
using System.Collections.Generic;

namespace Operations.SelectionByTwoFields.Generic{
    public interface IGrouperByTwoFields{
        IDictionary<Tuple<object, object>, TA> GroupByTwoFields<TA, TE>(IEnumerable<TE> source, string fieldName1, string fieldName2, Func<IEnumerable<TE>, TA> aggregation);
    }
}