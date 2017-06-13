using System.Collections.Generic;
using CommonEntities;
using Operations.SelectionByCountryAndAge;
using Xunit;
using System.Linq;

namespace Operations.Test.SelectionByCountryAndAge{
    public class SelectionByCountryAndAgeTest {
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
            var selector = new SelectorByCountryAndAge();
            var source = GenerateStorage;
            var result = selector.Get(source, "USA", 20);

            Assert.True( result.SequenceEqual(source.Where(e => e.Country == "USA" && e.Age == 20)));
        }

        [Fact]
        public void EmptyCase()
        {
            var selector = new SelectorByCountryAndAge();
            var source = GenerateStorage;
            var result = selector.Get(source, "JPN", 20);

            Assert.Empty(result);
        }
    }
}