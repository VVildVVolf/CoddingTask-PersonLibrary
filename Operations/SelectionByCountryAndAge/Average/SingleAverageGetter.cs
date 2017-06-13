using System.Collections.Generic;
using CommonEntities;
using Operations.Utils.AverageCounting;

namespace Operations.SelectionByCountryAndAge.Average {
    public class SingleAverageGetter : ISingleAverageGetter
    {
        public double Calculate(List<Person> persons, int age, string country)
        {
            var keeper = _averageKeeperFactory.Create;

            foreach(var iPerson in _selectorByCountryAndAge.Get(persons, country, age)){
                keeper.Add(iPerson.GrossSalary);
            }

            return keeper.CurrentAverage.Value;
        }

        private readonly IAverageKeeperFactory _averageKeeperFactory;
        private readonly ISelectorByCountryAndAge _selectorByCountryAndAge;

        public SingleAverageGetter(IAverageKeeperFactory averageKeeperFactory, ISelectorByCountryAndAge selectionByCountryAndAge){
            _averageKeeperFactory = averageKeeperFactory;
            _selectorByCountryAndAge = selectionByCountryAndAge;
        }
    }

}