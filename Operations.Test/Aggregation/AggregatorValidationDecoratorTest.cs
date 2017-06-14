using System;
using System.Collections.Generic;
using System.Linq;
using CommonEntities;
using Operations.Aggregation;
using Xunit;

namespace Operations.Test.Aggregation{
    public class AggregatorValidationDecoratorTest{
        private class AggregatorMock : IAggregator
        {
            public IEnumerable<GroupingResult> Get<T, TA>(IEnumerable<T> source, IEnumerable<string> horisontalGroupping, IEnumerable<string> verticalGroupping, Func<IEnumerable<T>, TA> aggregation)
            {
                CallCount ++;
                var result = new List<GroupingResult>(){
                    new GroupingResult(1, new List<GroupingResult>()),
                    new GroupingResult(2, new List<GroupingResult>()),
                    new GroupingResult(3, new List<GroupingResult>()),
                    new GroupingResult(4, new List<GroupingResult>())
                };
                LastReturnValue = result;
                return result;
            }

            public object LastReturnValue {get; private set;} = default(object);
            public int CallCount {get; private set;} = 0;
        }
        [Fact]
        public void CommonCase(){
            var aggregator = new AggregatorMock();
            var validator = new AggregatorValidationDecorator(aggregator);

            var result = validator.Get(new List<Person>(), new[]{"Age"}, new[]{"Country"}, Aggregation);

            Assert.Equal(result, aggregator.LastReturnValue);
            Assert.Equal(1, aggregator.CallCount);
        }

        [Fact]
        public void InvalidConstructionCase(){
            Assert.Throws(typeof(ArgumentNullException), ()=>new AggregatorValidationDecorator(null));
            Assert.Throws(typeof(ArgumentException), ()=>new AggregatorValidationDecorator(new AggregatorValidationDecorator(new AggregatorMock())));
        }
        [Fact]
        public void InvalidArguments(){
            var aggregator = new AggregatorMock();
            var validator = new AggregatorValidationDecorator(aggregator);

            Assert.Throws(typeof(AggregationException), () => validator.Get(new List<Person>(), new[]{"Ag"}, new[]{"Country"}, Aggregation));
            Assert.Throws(typeof(AggregationException), () => validator.Get(new List<Person>(), new string[]{}, new[]{"Country"}, Aggregation));
            Assert.Throws(typeof(AggregationException), () => validator.Get(new List<Person>(), null, new[]{"Country"}, Aggregation));
            Assert.Throws(typeof(AggregationException), () => validator.Get(new List<Person>(), new[]{"Age"}, new string[]{}, Aggregation));
            Assert.Throws(typeof(AggregationException), () => validator.Get(new List<Person>(), new[]{"Ag"}, null, Aggregation));
            Assert.Throws(typeof(AggregationException), () => validator.Get(new List<Person>(), new string[]{}, new string[]{}, Aggregation));
            Assert.Throws(typeof(AggregationException), () => validator.Get(new List<Person>(), null, null, Aggregation));
            Assert.Throws(typeof(AggregationException), () => validator.Get(new List<Person>(), new[]{"Age", "Age"}, new[]{"Country"}, Aggregation));
            Assert.Throws(typeof(AggregationException), () => validator.Get(new List<Person>(), new[]{"Age", "Country"}, new[]{"Country"}, Aggregation));
        }
        private double Aggregation(IEnumerable<Person> people){
            return people.Sum(e => e.GrossSalary);
        }
    }
}