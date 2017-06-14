using System.Collections.Generic;
using System.Linq;
using CommonEntities;
using Operations.Aggregation;
using Xunit;

namespace Operations.Test.Aggregation{
    public class AggregatorTest {
        [Fact]
        public void CommonCase(){
            var source = GenerateStorage;
            var horisontal = new []{"Gender", "Age"};
            var vertical = new []{"Country"};
            var aggregator = new Aggregator();

            var result = aggregator.Get(source, horisontal, vertical, AggFunc);

            Assert.Equal(2, result.Count());
            var genders = result.ToList();
            Assert.Equal("Female", genders[0].Key);
            Assert.Equal("Male", genders[1].Key);

            var females = genders[0].GroupValues.ToList();
            Assert.True(genders[0].IsHorisontal);
            Assert.Equal(2, females.Count());
            Assert.Equal(20, females[0].Key);
            Assert.Equal(30, females[1].Key);

            var males = genders[1].GroupValues.ToList();
            Assert.True(genders[1].IsHorisontal);
            Assert.Equal(2, males.Count());
            Assert.Equal(20, males[0].Key);
            Assert.Equal(30, males[1].Key);

            var f20 = females[0].GroupValues.ToList();
            Assert.Equal(2, f20.Count());
            Assert.Equal("USA", f20[0].Key);
            Assert.False(f20[0].IsHorisontal);
            Assert.Equal(1, f20[0].GroupValues.Count() );
            Assert.Equal((double)3500, f20[0].GroupValues.First().Key);
            Assert.Equal("UK", f20[1].Key);
            Assert.False(f20[1].IsHorisontal);
            Assert.Equal(1, f20[1].GroupValues.Count() );
            Assert.Equal((double)4500, f20[1].GroupValues.First().Key);

            var f30 = females[1].GroupValues.ToList();
            Assert.Equal(2, f30.Count());
            Assert.Equal("USA", f30[0].Key);
            Assert.False(f30[0].IsHorisontal);
            Assert.Equal(1, f30[0].GroupValues.Count() );
            Assert.Equal((double)5500, f30[0].GroupValues.First().Key);
            Assert.Equal("UK", f30[1].Key);
            Assert.False(f30[1].IsHorisontal);
            Assert.Equal(1, f30[1].GroupValues.Count() );
            Assert.Equal((double)6500, f30[1].GroupValues.First().Key);


            var m20 = males[0].GroupValues.ToList();
            Assert.Equal(2, m20.Count());
            Assert.Equal("USA", m20[0].Key);
            Assert.False(m20[0].IsHorisontal);
            Assert.Equal(1, m20[0].GroupValues.Count() );
            Assert.Equal((double)2500, m20[0].GroupValues.First().Key);
            Assert.Equal("UK", m20[1].Key);
            Assert.False(m20[1].IsHorisontal);
            Assert.Equal(1, m20[1].GroupValues.Count() );
            Assert.Equal((double)3000, m20[1].GroupValues.First().Key);

            var m30 = males[1].GroupValues.ToList();
            Assert.Equal(2, m30.Count());
            Assert.Equal("USA", m30[0].Key);
            Assert.False(m30[0].IsHorisontal);
            Assert.Equal(1, m30[0].GroupValues.Count() );
            Assert.Equal((double)3500, m30[0].GroupValues.First().Key);
            Assert.Equal("UK", m30[1].Key);
            Assert.False(m30[1].IsHorisontal);
            Assert.Equal(1, m30[1].GroupValues.Count() );
            Assert.Equal((double)4000, m30[1].GroupValues.First().Key);
        }


        static List<Person> GenerateStorage {
            get {
                var list = new List<Person>();
                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 2000, Gender = "Female"});
                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 3000, Gender = "Female"});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 2500, Gender = "Female"});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 3500, Gender = "Female"});

                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 2500, Gender = "Male"});
                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 3500, Gender = "Male"});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 3000, Gender = "Male"});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 4000, Gender = "Male"});

                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 1500, Gender = "Female"});
                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 2500, Gender = "Female"});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 2000, Gender = "Female"});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 3000, Gender = "Female"});
                return list;
            }
        }
        static double AggFunc(IEnumerable<Person> people){
            return people.Sum(e => e.GrossSalary);
        }
    }
}