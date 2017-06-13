using System.Collections.Generic;
using CommonEntities;

namespace Operations.SelectionByCountryAndAge {
    public interface ISelectorByCountryAndAge {
        IEnumerable<Person> Get(IEnumerable<Person> source, string country, int age);
    }
}