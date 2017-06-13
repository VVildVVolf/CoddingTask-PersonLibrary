using System;
using System.Collections.Generic;
using System.Linq;
using CommonEntities;
using Operations.SelectionByTwoFields.Generic;
using Xunit;

namespace Operations.Test.SelectionByTwoFields.Generic{
    public class GrouperByTwoFieldsTest{
        static List<Person> GenerateStorage {
            get {
                var list = new List<Person>();
                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 2000, Gender = "Male"});
                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 3000, Gender = "Male"});
                list.Add(new Person(){Age = 40, Country = "USA", GrossSalary = 5000, Gender = "Male"});
                list.Add(new Person(){Age = 50, Country = "USA", GrossSalary = 10000, Gender = "Male"});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 2500, Gender = "Male"});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 3500, Gender = "Male"});
                list.Add(new Person(){Age = 40, Country = "UK", GrossSalary = 4000, Gender = "Male"});
                list.Add(new Person(){Age = 50, Country = "UK", GrossSalary = 7000, Gender = "Male"});

                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 2500, Gender = "Female"});
                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 3500, Gender = "Female"});
                list.Add(new Person(){Age = 40, Country = "USA", GrossSalary = 5500, Gender = "Female"});
                list.Add(new Person(){Age = 50, Country = "USA", GrossSalary = 15000, Gender = "Female"});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 3000, Gender = "Female"});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 4000, Gender = "Female"});
                list.Add(new Person(){Age = 40, Country = "UK", GrossSalary = 4500, Gender = "Female"});
                list.Add(new Person(){Age = 50, Country = "UK", GrossSalary = 7500, Gender = "Female"});

                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 1500, Gender = "Male"});
                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 2500, Gender = "Male"});
                list.Add(new Person(){Age = 40, Country = "USA", GrossSalary = 4500, Gender = "Male"});
                list.Add(new Person(){Age = 50, Country = "USA", GrossSalary = 9500, Gender = "Male"});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 2000, Gender = "Male"});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 3000, Gender = "Male"});
                list.Add(new Person(){Age = 40, Country = "UK", GrossSalary = 3500, Gender = "Male"});
                list.Add(new Person(){Age = 50, Country = "UK", GrossSalary = 6500, Gender = "Male"});
                return list;
            }
        }
        public double Aggregation(IEnumerable<Person> people){
            return people.Sum(e => e.GrossSalary);
        }
        [Fact]
        public void CommonCase(){
            var source = GenerateStorage;
            var f1 = "Country";
            var f2 = "Gender";
            var expectation = new Dictionary<Tuple<object, object>, double>(){
                {new Tuple<object, object>("USA", "Male"), 38000},
                {new Tuple<object, object>("USA", "Female"), 26500},
                {new Tuple<object, object>("UK", "Male"), 32000},
                {new Tuple<object, object>("UK", "Female"), 19000},
            };

            var grouperByTwoFields = new GrouperByTwoFields();
            var result = grouperByTwoFields.GroupByTwoFields(source, f1, f2, Aggregation);

            Assert.True(expectation.SequenceEqual(result));
        }
    }
}