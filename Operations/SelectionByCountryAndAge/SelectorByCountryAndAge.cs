using System.Collections.Generic;
using CommonEntities;

namespace Operations.SelectionByCountryAndAge {
    public class SelectorByCountryAndAge: ISelectorByCountryAndAge {
        public IEnumerable<Person> Get(IEnumerable<Person> source, string country, int age){
            foreach (var iPerson in source){
                if (country == iPerson.Country && age == iPerson.Age){
                    yield return iPerson;
                }
            }
        }
    }
}