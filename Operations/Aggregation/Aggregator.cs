using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommonEntities;

namespace Operations.Aggregation {
    public class Aggregator : IAggregator
    {
        public IEnumerable<GroupingResult> Get<T, TA>(IEnumerable<T> source, IEnumerable<string> horisontalGroupping, IEnumerable<string> verticalGroupping, Func<IEnumerable<T>, TA> aggregation)
        {
            return GetGrouped(source, horisontalGroupping, verticalGroupping, aggregation, 0, 0);
        }

        private IEnumerable<GroupingResult> GetGrouped<T, TA>(
            IEnumerable<T> source, 
            IEnumerable<string> horisontal,
            IEnumerable<string> vertical,  
            Func<IEnumerable<T>, TA> aggregation, int hBegin, int vBegin)
        {
            if (vBegin == vertical.Count()) { //the end of the recursion
                yield return new GroupingResult(aggregation(source), null, false);
                yield break;
            }

            var hashes = new Dictionary<object, List<T>>();


            var property = GetNextField(horisontal, vertical, hBegin, vBegin);
            //TODO: optimaze by "onplace" counting the next level aggregation
            foreach (var element in source){
                var key = GetValue(element, property);
                if (!hashes.ContainsKey(key)){
                    hashes[key] = new List<T>();
                }
                hashes[key].Add(element);
            }
            var h = hBegin;
            var v = vBegin;
            if (h < horisontal.Count()){
                h++;
            } else {
                h++;
                v++;
            }

            foreach(var iKey in hashes.Keys){
                yield return new GroupingResult(
                    iKey,
                    GetGrouped(hashes[iKey], horisontal, vertical, aggregation, h, v),
                    hBegin < horisontal.Count()
                );
            }
        }

        private object GetValue<T>(T obj, string fieldName){
            var property = typeof(T).GetRuntimeProperty(fieldName);
            return property.GetValue(obj);
        }
        private string GetNextField(
            IEnumerable<string> horisontal, 
            IEnumerable<string> vertical, 
            int hBegin, int vBegin){
                if (hBegin < horisontal.Count()){
                    //should be optimized, but I do not have enought time, so it is just ToList.
                    //TODO: change to GetNext of enumerator. 
                    return horisontal.ToList()[hBegin];
                } else {
                    return vertical.ToList()[vBegin];
                }
        }
    }
}