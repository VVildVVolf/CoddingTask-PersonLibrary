using System;
using System.Collections.Generic;
using CommonEntities;

namespace Operations.SelectionByCountryAndAge.Average {
    public interface ISingleAverageGetter {
        // I would prefer use the IEnumerable, or at least IList, but there is the List in the task. 
        // And sorry for writing it through the comment.
        double Calculate(List<Person> persons, int age, string country);
    }
}