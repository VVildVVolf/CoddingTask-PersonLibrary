using System;
using System.Collections.Generic;
using CommonEntities;

namespace Operations.SelectionByTwoFields{
    static class PersonSelectors{
        public static IDictionary<string, Func<Person, object>> Selectors = new Dictionary<string, Func<Person, object>>(){
            {"Age", p => p.Age},
            {"Country", p => p.Country},
            {"Gender", p => p.Gender},
            {"GrossSalary", p => p.GrossSalary}
        };
    }
}