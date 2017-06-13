using System.Collections.Generic;
using CommonEntities;
using Operations.SelectionByCountryAndAge;

namespace Operations.Utils.AverageSetting{
    public class GrossSalaryAverageSetter {
        public IEnumerable<Person> SetAverage(List<Person> source, int age, string country, int value){
            var peopleToUpdate = _selectorByCountryAndAge.Get(source, country, age);
            foreach (var iPerson in peopleToUpdate){
                iPerson.GrossSalary = value;
            }
            return peopleToUpdate;
        }

        private readonly ISelectorByCountryAndAge _selectorByCountryAndAge;
        public GrossSalaryAverageSetter(ISelectorByCountryAndAge selectorByCountryAndAge){
            _selectorByCountryAndAge = selectorByCountryAndAge;
        }
    }
}