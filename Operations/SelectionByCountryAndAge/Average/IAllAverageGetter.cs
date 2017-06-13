using System;
using System.Collections.Generic;
using CommonEntities;

namespace Operations.SelectionByCountryAndAge.Average {
    public interface IAllAverageGetter{
        // because there is the List<Persion> in the task
        IDictionary<Tuple<string, int>, double> Calculate(List<Person> persons);
    }
}