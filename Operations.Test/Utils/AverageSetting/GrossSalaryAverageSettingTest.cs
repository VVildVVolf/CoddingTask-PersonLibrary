using System;
using System.Collections.Generic;
using System.Linq;
using CommonEntities;
using Operations.SelectionByCountryAndAge;
using Operations.SelectionByCountryAndAge.Average;
using Operations.Utils.AverageCounting;
using Operations.Utils.AverageSetting;
using Xunit;

namespace Operations.Test.Utils.AverageSetting{
    public class GrossSalaryAverageSetterTest{
        private class SelectorByCountryAndAgeMock : ISelectorByCountryAndAge
        {
            public IEnumerable<Person> Get(IEnumerable<Person> source, string country, int age)
            {
                var result = source.Where(e => e.Country == country && e.Age == age);
                _copyOfLastReturnedCollection = result.ToList();
                CountOfCalling++;
                return result;
            }
            public int CountOfCalling {get; private set;} = 0;
            public IReadOnlyCollection<Person> CopyOfLastReturnedCollection => _copyOfLastReturnedCollection;
            private List<Person> _copyOfLastReturnedCollection = default(List<Person>);
        }
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
        public void CommonCase(){
            var source = GenerateStorage;
            var singleAverageGetter = new SingleAverageGetter(new AverageKeeperFactory(), new SelectorByCountryAndAge());
            var age = 20;
            var country = "USA";
            var average = 4000;

            var selectorByCountryAndAgeMock = new SelectorByCountryAndAgeMock();
            var grossSalaryAverageSetter = new GrossSalaryAverageSetter(selectorByCountryAndAgeMock);


            var result = grossSalaryAverageSetter.SetAverage(source, age, country, average);

            Assert.Equal(average, singleAverageGetter.Calculate(result.ToList(), age, country));
            Assert.Equal(1, selectorByCountryAndAgeMock.CountOfCalling);
            Assert.True(result.SequenceEqual(selectorByCountryAndAgeMock.CopyOfLastReturnedCollection));
        }
        [Fact]
        public void EmptyCase(){
            var source = GenerateStorage;
            var age = 25;
            var country = "USA";
            var average = 4000;

            var selectorByCountryAndAgeMock = new SelectorByCountryAndAgeMock();
            var grossSalaryAverageSetter = new GrossSalaryAverageSetter(selectorByCountryAndAgeMock);

            var result = grossSalaryAverageSetter.SetAverage(source, age, country, average);
            var checkStorage = GenerateStorage;

            Assert.Equal(1, selectorByCountryAndAgeMock.CountOfCalling);
            Assert.False(result.Any());
            Func<Person, string> serializer =  p => $"{p.Age} {p.Country} {p.Gender} {p.GrossSalary}";
            Assert.True(checkStorage.Select(serializer).SequenceEqual(source.Select(serializer)));
        }
        [Fact]
        public void WithoutCall(){
            var source = GenerateStorage;
            var singleAverageGetter = new SingleAverageGetter(new AverageKeeperFactory(), new SelectorByCountryAndAge());

            var selectorByCountryAndAgeMock = new SelectorByCountryAndAgeMock();
            var grossSalaryAverageSetter = new GrossSalaryAverageSetter(selectorByCountryAndAgeMock);
            var checkStorage = GenerateStorage;

            Assert.Equal(0, selectorByCountryAndAgeMock.CountOfCalling);
            Func<Person, string> serializer =  p => $"{p.Age} {p.Country} {p.Gender} {p.GrossSalary}";
            Assert.True(checkStorage.Select(serializer).SequenceEqual(source.Select(serializer)));
        }
    }
}