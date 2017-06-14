using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommonEntities;
using Operations.SelectionByTwoFields;
using Xunit;

namespace Operations.Test.SelectionByTwoFields{
    public class GrouperByTwoFieldsValidationDecoratorTest {
        static List<Person> GenerateStorage {
            get {
                var list = new List<Person>();
                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 2000});
                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 3000});
                list.Add(new Person(){Age = 40, Country = "USA", GrossSalary = 5000});
                list.Add(new Person(){Age = 50, Country = "USA", GrossSalary = 10000});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 2500});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 3500});
                list.Add(new Person(){Age = 40, Country = "UK", GrossSalary = 4000});
                list.Add(new Person(){Age = 50, Country = "UK", GrossSalary = 7000});

                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 2500});
                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 3500});
                list.Add(new Person(){Age = 40, Country = "USA", GrossSalary = 5500});
                list.Add(new Person(){Age = 50, Country = "USA", GrossSalary = 15000});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 3000});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 4000});
                list.Add(new Person(){Age = 40, Country = "UK", GrossSalary = 4500});
                list.Add(new Person(){Age = 50, Country = "UK", GrossSalary = 7500});

                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 1500});
                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 2500});
                list.Add(new Person(){Age = 40, Country = "USA", GrossSalary = 4500});
                list.Add(new Person(){Age = 50, Country = "USA", GrossSalary = 9500});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 2000});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 3000});
                list.Add(new Person(){Age = 40, Country = "UK", GrossSalary = 3500});
                list.Add(new Person(){Age = 50, Country = "UK", GrossSalary = 6500});
                return list;
            }
        }
        private class GrouperByTwoFieldsMock : IGrouperByTwoFields
        {
            public IDictionary<Tuple<object, object>, T> GroupByTwoFields<T>(List<Person> source, string fieldName1, string fieldName2, Func<IEnumerable<Person>, T> aggregation)
            {
                CountOfCalling++;
                var result = new Dictionary<Tuple<object, object>, T>();
                LastReturned = result;
                return result;
            }
            public int CountOfCalling {get; private set;} = 0;
            public object LastReturned {get; set; }
        }

        [Fact]
        public void CommonCase(){
            var fields = typeof(Person).GetRuntimeProperties().Where(e => e.CanRead).Select(e => e.Name);
            var pairs = from f1 in fields
                        from f2 in fields
                        where f2 != f1
                        select new {f1, f2};

            foreach (var iPair in pairs){
                var source = GenerateStorage;
                var decorated = new GrouperByTwoFieldsMock();

                var validator = new GrouperByTwoFieldsValidationDecorator(decorated);
                var result = validator.GroupByTwoFields(source, iPair.f1, iPair.f2, Aggregation);

                Assert.Equal(decorated.LastReturned, result);
                Assert.Equal(1, decorated.CountOfCalling);
            }
        }

        [Fact]
        public void IncorrectFirstField(){
            var source = GenerateStorage;
            var decorated = new GrouperByTwoFieldsMock();
            var f1 = "Age1";
            var f2 = "Country";

            var validator = new GrouperByTwoFieldsValidationDecorator(decorated);
            Assert.Throws(typeof(GrouperByTwoFieldsException), () => {validator.GroupByTwoFields(source, f1, f2, Aggregation);});

            Assert.Equal(0, decorated.CountOfCalling);
        }

        [Fact]
        public void IncorrectSecondField(){
            var source = GenerateStorage;
            var decorated = new GrouperByTwoFieldsMock();
            var f1 = "Age";
            var f2 = "Country1";

            var validator = new GrouperByTwoFieldsValidationDecorator(decorated);
            Assert.Throws(typeof(GrouperByTwoFieldsException), () => {validator.GroupByTwoFields(source, f1, f2, Aggregation);});

            Assert.Equal(0, decorated.CountOfCalling);
        }

        [Fact]
        public void SameField(){
            var source = GenerateStorage;
            var decorated = new GrouperByTwoFieldsMock();
            var f1 = "Age";
            var f2 = "Age";

            var validator = new GrouperByTwoFieldsValidationDecorator(decorated);
            Assert.Throws(typeof(GrouperByTwoFieldsException), () => {validator.GroupByTwoFields(source, f1, f2, Aggregation);});

            Assert.Equal(0, decorated.CountOfCalling);
        }
        [Fact]
        public void SameObject(){
            var decorated = new GrouperByTwoFieldsMock();
            var secondDecorated = new GrouperByTwoFieldsValidationDecorator(decorated);
            Assert.Throws(typeof(ArgumentException), () => {new GrouperByTwoFieldsValidationDecorator(secondDecorated);});
        }
        [Fact]
        public void EmptyDecorated(){
            Assert.Throws(typeof(ArgumentNullException), () => {new GrouperByTwoFieldsValidationDecorator(null);});
        }

        public int Aggregation(IEnumerable<Person> people){
            return people.Sum(e => e.Age);
        }
    }
}