using System;
using System.Reflection;
using System.Collections.Generic;

namespace Operations.SelectionByTwoFields.Generic{
    public class GrouperByTwoFields : IGrouperByTwoFields
    {
        public IDictionary<Tuple<object, object>, TA> GroupByTwoFields<TA, TE>(IEnumerable<TE> source, string fieldName1, string fieldName2, Func<IEnumerable<TE>, TA> aggregation)
        {
            var selector1 = CreateGetter<TE>(fieldName1);
            var selector2 = CreateGetter<TE>(fieldName2);
            var hashes = new Dictionary<object, Dictionary<object, IList<TE>>>();
            foreach(var iElement in source){
                var firstAggregationValue = selector1(iElement);
                if (!hashes.ContainsKey(firstAggregationValue)){
                    hashes[firstAggregationValue] = new Dictionary<object, IList<TE>>();
                }
                var selection = hashes[firstAggregationValue];
                var secondAggregationValue = selector2(iElement);
                if(!selection.ContainsKey(secondAggregationValue)){
                    selection[secondAggregationValue] = new List<TE>();
                }
                selection[secondAggregationValue].Add(iElement);
            }

            var result = new Dictionary<Tuple<object, object>, TA>();

            foreach(var iSetKey in hashes.Keys){
                foreach(var iGroupKey in hashes[iSetKey].Keys){
                    var key = new Tuple<object, object> (iSetKey, iGroupKey);
                    var value = aggregation(hashes[iSetKey][iGroupKey]);
                    result[key] = value;
                }
            }
            return result;
        }

        private Func<T, object> CreateGetter<T>(string fieldName){
            var property = typeof(T).GetRuntimeProperty(fieldName);
            Func<T, object> result = arg =>{
                return property.GetValue(arg);
            };
            return result;
        }
    }
}