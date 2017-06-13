using Xunit;
using System;
using System.Linq;
using System.Collections.Generic;
using Operations.Utils.AverageCounting;
using Operations.SelectionByCountryAndAge.Average;
using CommonEntities;
using Operations.SelectionByCountryAndAge;

namespace Operations.Test.SelectionByCountryAndAge.Average{

    class AverageKeeperMock : IAverageKeeper {
        public IReadOnlyCollection<double> IncomingValues => _incomingValues;
        public int RequestsCount {get; private set;} = default(int);
        public double? LastRequestedAnswer { get; private set; } = default(double?);
        public double? CurrentAverage {
            get {
                RequestsCount++;
                LastRequestedAnswer = _incomingValues.Sum()/_incomingValues.Count;
                return LastRequestedAnswer.Value;
            }
        }

        public void Add(double val)
        {
            _incomingValues.Add(val);
        }

        private readonly List<double> _incomingValues = new List<double>();
    }

    class AverageKeeperFactoryMock : IAverageKeeperFactory {
        public IAverageKeeper Create => Instance;

        public readonly AverageKeeperMock Instance = new AverageKeeperMock();
    }

    class SelectorByCountryAndAgeMock : ISelectorByCountryAndAge {
        public IEnumerable<Person> Get(IEnumerable<Person> source, string country, int age)
        {
            LastRequestedSource = source.ToList();
            LastRequestedCountry = country;
            LastRequestedAge = age;
            RequestsCount++;
            return source.Where(e => e.Age == age && country == e.Country);
        }
        public IEnumerable<Person> LastRequestedSource { get; private set; } = default(IEnumerable<Person>);
        public string LastRequestedCountry { get; private set; } = default(string);
        public int? LastRequestedAge {get; private set; } = default(int?);
        public int RequestsCount { get; private set; } = 0;
    }

    public class SingleAverageGetterTest {
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

        [Fact]
        public void TestCommonCase(){
            var averageKeeperFactory = new AverageKeeperFactoryMock();
            var keeper = averageKeeperFactory.Instance;
            var selectorByCountryAndAge = new SelectorByCountryAndAgeMock();
            var singleAverageGetter = new SingleAverageGetter(averageKeeperFactory, selectorByCountryAndAge);

            var source = GenerateStorage;
            var age = 20;
            var country = "USA";

            var result = singleAverageGetter.Calculate(source, age, country);

            Assert.Equal(1, selectorByCountryAndAge.RequestsCount);
            Assert.True(selectorByCountryAndAge.LastRequestedSource.SequenceEqual(source));
            Assert.Equal(age, selectorByCountryAndAge.LastRequestedAge);
            Assert.Equal(country, selectorByCountryAndAge.LastRequestedCountry);
            Assert.True(keeper.LastRequestedAnswer.HasValue);
            Assert.Equal(result, keeper.LastRequestedAnswer.Value);
            Assert.Equal(1, keeper.RequestsCount);
            Assert.True(keeper.IncomingValues.SequenceEqual(new List<double>(){2000, 2500, 1500}));
        }

        [Fact]
        public void TestEmptyCase(){
            var averageKeeperFactory = new AverageKeeperFactoryMock();
            var keeper = averageKeeperFactory.Instance;
            var selectorByCountryAndAge = new SelectorByCountryAndAgeMock();
            var singleAverageGetter = new SingleAverageGetter(averageKeeperFactory, selectorByCountryAndAge);
            
            var source = GenerateStorage;
            var age = 100;
            var country = "USA";

            var result = singleAverageGetter.Calculate(source, age, country);

            Assert.Equal(1, selectorByCountryAndAge.RequestsCount);
            Assert.True(selectorByCountryAndAge.LastRequestedSource.SequenceEqual(source));
            Assert.Equal(age, selectorByCountryAndAge.LastRequestedAge);
            Assert.Equal(country, selectorByCountryAndAge.LastRequestedCountry);
            Assert.True(keeper.LastRequestedAnswer.HasValue);
            Assert.Equal(result, keeper.LastRequestedAnswer.Value);
            Assert.Equal(1, keeper.RequestsCount);
            Assert.False(keeper.IncomingValues.Any());
        }

        [Fact]
        public void TestNotRequestedResult() {
            var averageKeeperFactory = new AverageKeeperFactoryMock();
            var keeper = averageKeeperFactory.Instance;
            var selectorByCountryAndAge = new SelectorByCountryAndAgeMock();
            var singleAverageGetter = new SingleAverageGetter(averageKeeperFactory, selectorByCountryAndAge);


            Assert.Equal(0, selectorByCountryAndAge.RequestsCount);
            Assert.True(selectorByCountryAndAge.LastRequestedSource == default(IEnumerable<Person>));
            Assert.False(selectorByCountryAndAge.LastRequestedAge.HasValue);
            Assert.True(string.IsNullOrEmpty(selectorByCountryAndAge.LastRequestedCountry));
            Assert.False(keeper.LastRequestedAnswer.HasValue);
            Assert.Equal(0, keeper.RequestsCount);
            Assert.False(keeper.IncomingValues.Any());
        }
    }
}