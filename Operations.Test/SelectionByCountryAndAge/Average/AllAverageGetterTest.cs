using System;
using System.Collections.Generic;
using System.Linq;
using CommonEntities;
using Operations.SelectionByCountryAndAge.Average;
using Operations.Utils.AverageCounting;
using Xunit;


class AverageKeeperMock1 : IAverageKeeper {
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


class AverageKeeperFactoryMock1 : IAverageKeeperFactory {
    public IAverageKeeper Create{
        get{
            var instance = Generate;
            _requestedKeepers.Add(instance);
            return instance;
        }
    }

    private AverageKeeperMock1 Generate => new AverageKeeperMock1();
    public IReadOnlyCollection<AverageKeeperMock1> RequestedKeepers => _requestedKeepers;
    private readonly List<AverageKeeperMock1> _requestedKeepers = new List<AverageKeeperMock1>();
}

namespace Operations.Test.SelectionByCountryAndAge.Average{

    public class AllAverageGetterTest {
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
        public void CommonCase() {
            var keeperFactory = new AverageKeeperFactoryMock1();
            var source = GenerateStorage;

            var averageCounter = new AllAverageGetter(keeperFactory);
            var result = averageCounter.Calculate(source);

            Assert.Equal(8, result.Keys.Count);
            var expectedTuples = source.GroupBy(e => e.Country).SelectMany(e => e.Select(ee => ee.Age).Distinct().Select(ee => new Tuple<string, int>(e.Key, ee)));
            Assert.True(expectedTuples.SequenceEqual(result.Keys));

            foreach(var iPair in expectedTuples){
                var selection = source.Where(e => e.Country == iPair.Item1 && e.Age == iPair.Item2);
                Assert.Equal(selection.Sum(e => e.GrossSalary)/selection.Count(), result[iPair]);
            }

            var keepers = keeperFactory.RequestedKeepers.Select(e => new KeeperMockDescription 
                {
                    IncomingValues = e.IncomingValues, 
                    LastRequestedAnswer = e.LastRequestedAnswer, 
                    RequestsCount = e.RequestsCount
                }
            );
            var expectedKeeperd = expectedTuples.Select(e => new KeeperMockDescription
                {
                    IncomingValues = source.Where(ee => ee.Country == e.Item1 && ee.Age == e.Item2).Select(ee => ee.GrossSalary),
                    LastRequestedAnswer = source.Where(ee => ee.Country == e.Item1 && ee.Age == e.Item2).Select(ee => ee.GrossSalary).Sum()/3,
                    RequestsCount = 1
                });
            Assert.True(keepers.SequenceEqual(expectedKeeperd));
        }
        
        private class KeeperMockDescription {
            public IEnumerable<double> IncomingValues {get;set;}
            public double? LastRequestedAnswer {get;set;}
            public int RequestsCount { get;set; }
            public override bool Equals(object obj){
                if (!(obj is KeeperMockDescription)) return false;
                var k = obj as KeeperMockDescription;
                return k.IncomingValues.SequenceEqual(IncomingValues) && k.LastRequestedAnswer == LastRequestedAnswer && k.RequestsCount == RequestsCount;
            }
        }

        [Fact]
        public void EmptySource(){
            var keeperFactory = new AverageKeeperFactoryMock1();
            var source = new List<Person>();

            var averageCounter = new AllAverageGetter(keeperFactory);
            var result = averageCounter.Calculate(source);

            Assert.Empty(result.Keys);
            Assert.Empty(keeperFactory.RequestedKeepers);
        }

        [Fact]
        public void NotCallCalculate(){
            var keeperFactory = new AverageKeeperFactoryMock1();
            var source = GenerateStorage;

            var averageCounter = new AllAverageGetter(keeperFactory);
            Assert.Empty(keeperFactory.RequestedKeepers);
        }
    }
}