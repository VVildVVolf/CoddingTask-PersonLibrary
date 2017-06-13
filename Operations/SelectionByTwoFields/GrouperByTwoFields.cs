using System;
using System.Collections.Generic;
using CommonEntities;

namespace Operations.SelectionByTwoFields{
    public class GrouperByTwoFields : IGrouperByTwoFields
    {
        public IDictionary<Tuple<object, object>, T> GroupByTwoFields<T>(List<Person> source, string fieldName1, string fieldName2, Func<IEnumerable<Person>, T> aggregation)
        {
            var selector1 = PersonSelectors.Selectors[fieldName1];
            var selector2 = PersonSelectors.Selectors[fieldName2];

            var hashes = new Dictionary<object, Dictionary<object, IList<Person>>>();

            foreach(var iPerson in source){
                var firstAggregationValue = selector1(iPerson);
                if (!hashes.ContainsKey(firstAggregationValue)){
                    hashes[firstAggregationValue] = new Dictionary<object, IList<Person>>();
                }
                var selection = hashes[firstAggregationValue];
                var secondAggregationValue = selector2(iPerson);
                if(!selection.ContainsKey(secondAggregationValue)){
                    selection[secondAggregationValue] = new List<Person>();
                }
                selection[secondAggregationValue].Add(iPerson);
            }

            var result = new Dictionary<Tuple<object, object>, T>();

            foreach(var iSetKey in hashes.Keys){
                foreach(var iGroupKey in hashes[iSetKey].Keys){
                    var key = new Tuple<object, object> (iSetKey, iGroupKey);
                    var value = aggregation(hashes[iSetKey][iGroupKey]);
                    result[key] = value;
                }
            }

            return result;
        }
    }
}