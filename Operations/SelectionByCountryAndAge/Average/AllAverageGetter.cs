using System;
using System.Collections.Generic;
using CommonEntities;
using Operations.Utils.AverageCounting;

namespace Operations.SelectionByCountryAndAge.Average {
    public class AllAverageGetter : IAllAverageGetter
    {
        public IDictionary<Tuple<string, int>, double> Calculate(List<Person> persons) => ConvertToExport(FindAllAverages(persons));

        private Dictionary<string, Dictionary<int, IAverageKeeper>> FindAllAverages(List<Person> persons){
            var hashes = new Dictionary<string, Dictionary<int, IAverageKeeper>>();

            foreach(var iPerson in persons){
                if (!hashes.ContainsKey(iPerson.Country)){
                    hashes[iPerson.Country] = new Dictionary<int, IAverageKeeper>();
                }
                var agesOfCurrentCountry = hashes[iPerson.Country];
                if(!agesOfCurrentCountry.ContainsKey(iPerson.Age)){
                    agesOfCurrentCountry[iPerson.Age] = _averageKeeperFactory.Create;
                }
                agesOfCurrentCountry[iPerson.Age].Add(iPerson.GrossSalary);
            }
            return hashes;
        }

        private IDictionary<Tuple<string, int>, double> ConvertToExport(Dictionary<string, Dictionary<int, IAverageKeeper>> averages){
            var result = new Dictionary<Tuple<string, int>, double>();
            foreach (var iCity in averages.Keys){
                foreach(var iAge in averages[iCity].Keys){
                    var cityAndAge = new Tuple<string, int>(iCity, iAge);
                    result[cityAndAge] = averages[iCity][iAge].CurrentAverage.Value;
                }
            }
            return result;
        }

        private readonly IAverageKeeperFactory _averageKeeperFactory;

        public AllAverageGetter(IAverageKeeperFactory averageKeeperFactory){
            _averageKeeperFactory = averageKeeperFactory;
        }
    }
}