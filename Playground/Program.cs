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


            var horisontal = new []{"Gender", "Age"};
            var vertical = new []{"Country", "GrossSalary"};
            var aggregator = new Aggregator();

            var result = aggregator.Get(source, horisontal, vertical, AggFunc);
            var groupPrinter = new Operations.Aggregation.Printing.PrinterToConsole();
            groupPrinter.Print(result, horisontal, vertical);
        }
        static List<Person> GenerateStorage {
            get {
                var list = new List<Person>();
                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 2000, Gender = "Female"});
                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 2000, Gender = "Female"});
                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 5000, Gender = "Female"});
                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 5000, Gender = "Female"});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 2000, Gender = "Female"});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 2000, Gender = "Female"});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 5000, Gender = "Female"});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 5000, Gender = "Female"});

                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 2000, Gender = "Female"});
                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 2000, Gender = "Female"});
                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 5000, Gender = "Female"});
                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 5000, Gender = "Female"});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 2000, Gender = "Female"});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 2000, Gender = "Female"});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 5000, Gender = "Female"});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 5000, Gender = "Female"});

                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 2000, Gender = "Male"});
                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 2000, Gender = "Male"});
                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 5000, Gender = "Male"});
                list.Add(new Person(){Age = 20, Country = "USA", GrossSalary = 5000, Gender = "Male"});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 2000, Gender = "Male"});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 2000, Gender = "Male"});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 5000, Gender = "Male"});
                list.Add(new Person(){Age = 20, Country = "UK", GrossSalary = 5000, Gender = "Male"});

                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 2000, Gender = "Male"});
                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 2000, Gender = "Male"});
                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 5000, Gender = "Male"});
                list.Add(new Person(){Age = 30, Country = "USA", GrossSalary = 5000, Gender = "Male"});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 2000, Gender = "Male"});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 2000, Gender = "Male"});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 5000, Gender = "Male"});
                list.Add(new Person(){Age = 30, Country = "UK", GrossSalary = 5000, Gender = "Male"});
                return list;
            }
        }
        static double AggFunc(IEnumerable<Person> people){
            return people.Sum(e => e.GrossSalary);
        }
    }
}
