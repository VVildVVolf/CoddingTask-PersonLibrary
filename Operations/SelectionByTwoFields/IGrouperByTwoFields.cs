using System;
using System.Collections.Generic;
using CommonEntities;

namespace Operations.SelectionByTwoFields{
    public interface IGrouperByTwoFields{
        IDictionary<Tuple<object, object>, T> GroupByTwoFields<T>(List<Person> source, string fieldName1, string fieldName2, Func<IEnumerable<Person>, T> aggregation);
    }
}