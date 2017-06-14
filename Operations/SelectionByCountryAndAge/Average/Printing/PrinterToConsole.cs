using System;
using System.Collections.Generic;
using Pair = System.Tuple<string, int>;
using AggregatedResults = System.Collections.Generic.IDictionary<System.Tuple<string, int>, double>;

namespace Operations.SelectionByCountryAndAge.Average.Printing {
    public class PrinterToConsole : IPrinter
    {
        public void Print(AggregatedResults aggregatedResults)
        {
            var pairs = aggregatedResults.Keys;
            var countries = GetValues(pairs, p => p.Item1);
            var ages = GetValues(pairs, p => p.Item2);

            PrintLine();
            string[] strings = new string[ages.Count + 1];
            strings[0] = "Average";
            var i = 1;
            foreach(var iAge in ages){
                strings[i] = iAge.ToString();
                i++;
            }

            PrintRow(strings);
            PrintLine();

            foreach (var iCountry in countries){
                strings[0] = iCountry;
                i = 1;
                foreach(var iAge in ages){
                    var key = new Tuple<string, int>(iCountry, iAge);

                    strings[i] = aggregatedResults.ContainsKey(key) ? aggregatedResults[key].ToString() : "x";
                    i++;
                }
                PrintRow(strings);
                PrintLine();
            }

        }

        private ISet<T> GetValues<T>(IEnumerable<Pair> pairs, Func<Pair, T> predicate){
            var values = new HashSet<T>();

            foreach(var iPair in pairs){
                var value = predicate(iPair);
                values.Add(value);
            }

            return values;
        }

        //from https://stackoverflow.com/questions/856845/how-to-best-way-to-draw-table-in-console-app-c
        static int _tableWidth = 77;

        static void PrintLine()
        {
            Console.WriteLine(new string('-', _tableWidth));
        }

        static void PrintRow(params string[] columns)
        {
            int width = (_tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}