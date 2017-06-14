using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Operations.Aggregation;
using CommonEntities;
using Operations.SelectionByCountryAndAge.Average;
using Operations.SelectionByCountryAndAge.Average.Printing;
using Operations.Utils.AverageCounting;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = GenerateStorage;
            var allAverageGetter = new AllAverageGetter(new AverageKeeperFactory());
            var printer = new PrinterToConsole();
            printer.Print(allAverageGetter.Calculate(source));
        }
        static List<Person> GenerateStorage {
            get {
                var list = new List<Person>();
                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 2000, Gender = "Female"});
                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 3000, Gender = "Female"});
                list.Add(new Person(){Age = 40, Country = "USA", GrossSalary = 5000, Gender = "Female"});
                list.Add(new Person(){Age = 50, Country = "USA", GrossSalary = 10000, Gender = "Female"});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 2500, Gender = "Female"});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 3500, Gender = "Female"});
                list.Add(new Person(){Age = 40, Country = "UK", GrossSalary = 4000, Gender = "Female"});
                list.Add(new Person(){Age = 50, Country = "UK", GrossSalary = 7000, Gender = "Female"});

                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 2500, Gender = "Male"});
                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 3500, Gender = "Male"});
                list.Add(new Person(){Age = 40, Country = "USA", GrossSalary = 5500, Gender = "Male"});
                list.Add(new Person(){Age = 50, Country = "USA", GrossSalary = 15000, Gender = "Male"});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 3000, Gender = "Male"});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 4000, Gender = "Male"});
                list.Add(new Person(){Age = 40, Country = "UK", GrossSalary = 4500, Gender = "Male"});
                list.Add(new Person(){Age = 50, Country = "UK", GrossSalary = 7500, Gender = "Male"});

                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 1500, Gender = "Female"});
                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 2500, Gender = "Female"});
                list.Add(new Person(){Age = 40, Country = "USA", GrossSalary = 4500, Gender = "Female"});
                list.Add(new Person(){Age = 50, Country = "USA", GrossSalary = 9500, Gender = "Female"});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 2000, Gender = "Female"});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 3000, Gender = "Female"});
                list.Add(new Person(){Age = 40, Country = "UK", GrossSalary = 3500, Gender = "Female"});
                list.Add(new Person(){Age = 50, Country = "UK", GrossSalary = 6500, Gender = "Female"});
                return list;
            }
        }
        static double AggFunc(IEnumerable<Person> people){
            return people.Sum(e => e.GrossSalary);
        }
    }
}
