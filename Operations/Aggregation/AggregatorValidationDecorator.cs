using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Operations.Aggregation{
    public class AggregatorValidationDecorator : IAggregator
    {
        public IEnumerable<GroupingResult> Get<T, TA>(IEnumerable<T> source, IEnumerable<string> horisontalGroupping, IEnumerable<string> verticalGroupping, Func<IEnumerable<T>, TA> aggregation)
        {
            Validate(horisontalGroupping, verticalGroupping, typeof(T));
            return _aggregator.Get(source, horisontalGroupping, verticalGroupping, aggregation);
        }

        private void Validate(IEnumerable<string> horisontalGroupping, IEnumerable<string> verticalGroupping, Type type){
            if (verticalGroupping == null || !verticalGroupping.Any()){
                throw AggregationException.EmptyFieldSet(true);
            }
            if (horisontalGroupping == null || !horisontalGroupping.Any()){
                throw AggregationException.EmptyFieldSet(false);
            }
            var all = horisontalGroupping.Union(verticalGroupping);
            if (all.Distinct().Count() != (verticalGroupping.Count() + horisontalGroupping.Count())){
                throw AggregationException.DuplicateField();
            }

            var fields = type.GetRuntimeProperties().Where(e => e.CanRead).Select(e => e.Name);
            foreach (var iField in all){
                if (!fields.Contains(iField)){
                    throw AggregationException.UnknownFieldName(iField, type);
                }
            }
        }

        private readonly IAggregator _aggregator;
        public AggregatorValidationDecorator(IAggregator aggregator){
            if (aggregator == null){
                throw new ArgumentNullException("The aggregator cannot be null.");
            }
            if (aggregator is AggregatorValidationDecorator){
                throw new ArgumentException("The recursive decoration is detected.");
            }
            _aggregator = aggregator;
        }
    }
}