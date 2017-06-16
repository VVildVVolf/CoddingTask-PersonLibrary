using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Operations.Aggregation.Printing{
    //Just do not have a time to clean the code, so it is as is.
    public class PrinterToConsole : IPrinter
    {
        private const int CellLength = 15;
        private string EmptyCell = string.Join("", Enumerable.Repeat(' ', CellLength));
        private string CellSeparator = string.Join("", Enumerable.Repeat('-', CellLength));
        public void Print(IEnumerable<GroupingResult> source, IEnumerable<string> horisontal, IEnumerable<string> vertical)
        {
            var unprinted = PrintHeader(vertical.Count(), source, horisontal);
            PrintBody(unprinted, source, vertical);
            var lastLine = "-" + string.Join("-", Enumerable.Repeat(CellSeparator, vertical.Count() + CalculateWidth(source))) + "-";
            Console.WriteLine(lastLine);
        }

        //returns the next not printed GroupResults 
        private IEnumerable<GroupingResult> PrintHeader(int prefixLen, IEnumerable<GroupingResult> source, IEnumerable<string> horisontalHeaders){
            var widths = new Dictionary<string, int>();
            var len = BindWidthsOfHeaders(source, widths);
            var headerRow = "-" + string.Join("-", Enumerable.Repeat( CellSeparator, len + prefixLen)) + "-";
            Console.WriteLine(headerRow);
            var prefixCell = "|" + string.Join(" ", Enumerable.Repeat( EmptyCell, prefixLen)) + "|";

            var waitToPrint = source.ToList();
            var level = 0;
            var result = new List<GroupingResult>();
            while(waitToPrint.Any()){
                Console.Write(prefixCell);
                var header = horisontalHeaders.ToList()[level];
                var active = waitToPrint;
                waitToPrint = new List<GroupingResult>();
                foreach(var item in active){
                    //Just because it cannot find by the direct link to the object.
                    var cellLen = widths[JsonConvert.SerializeObject( item)];
                    var valueAsString = header + ":" + item.Key.ToString();
                    var cellLenInSymbols = cellLen * CellLength + (cellLen - 1) - valueAsString.Length - 1;

                    Console.Write(" " + valueAsString + string.Join("", Enumerable.Repeat(" ", cellLenInSymbols)) + "|");
                    if (item.GroupValues.First().IsHorisontal){
                        waitToPrint.AddRange(item.GroupValues);
                    } else {
                        result.AddRange(item.GroupValues);
                    }
                }
                Console.WriteLine();
                Console.WriteLine(headerRow);
                level++;
            }
            return result;
        }
        private int BindWidthsOfHeaders(IEnumerable<GroupingResult> source, Dictionary<string, int> storage){
            if (!source.First().IsHorisontal) return 1;

            var sum = 0;
            foreach(var iGroup in source){
                var len = BindWidthsOfHeaders( iGroup.GroupValues, storage);
                storage[ JsonConvert.SerializeObject( iGroup)] = len;
                sum += len;
            }
            return sum;
        }

        private void PrintBody(IEnumerable<GroupingResult> notPrinted, IEnumerable<GroupingResult> root, IEnumerable<string> verticalHeaders){
            var table = GetTableValues(root, notPrinted);
            var rowIndex = 0;
            var prefix = "|";
            Console.Write(prefix);
            PrintRows(notPrinted, 0, prefix, verticalHeaders, table, ref rowIndex);
        }
        private void PrintRows(IEnumerable<GroupingResult> source, int wasColumnsBefore, string prefix, IEnumerable<string> verticalHeaders, IList<IEnumerable<object>> table, ref int nextRow){
            if (source.First().GroupValues == null){
                PrintValueRow(table[nextRow]);
                nextRow ++;
                Console.WriteLine();
            } else {
                var isFirst = true;
                foreach(var item in source.GroupBy(e => e.Key)){
                    if(isFirst){
                        isFirst = false;
                    } else {
                        Console.Write(prefix);
                    }
                    var fieldName = verticalHeaders.ToList()[wasColumnsBefore];
                    var header = $"{fieldName}:{item.Key}";
                    //TODO: optimize this place.
                    if (header.Length > CellLength){
                        var value = ":"+item.Key;
                        header = fieldName[0] + "~" + value;
                    }
                    var headerCell = header + string.Join("", Enumerable.Repeat(" ", CellLength - header.Length)) + "|";
                    Console.Write(headerCell);

                    var many = item.SelectMany(e => e.GroupValues).GroupBy(e => e.Key);
                    PrintRows(item.SelectMany(e => e.GroupValues), wasColumnsBefore + 1, prefix + string.Join("", Enumerable.Repeat(" ", CellLength)) + "|", verticalHeaders, table, ref nextRow);
                }
            }
        }
        private void PrintValueRow(IEnumerable<object> source){
            foreach (var item in source){
                var valueAsString = item.ToString();
                var postfixLen = CellLength - valueAsString.Length - 1;
                Console.Write( " " + valueAsString + string.Join("", Enumerable.Repeat(" ", postfixLen)) + "|");
            }
        }

        private IList<IEnumerable<object>> GetTableValues(IEnumerable<GroupingResult> source, IEnumerable<GroupingResult> notPrinted){
            var width = CalculateWidth(source);
            var height = CalculateHeight(notPrinted);
            var headerIndexes = GetHeaderIndexes(source);
            var rowIndexes = GetOtherIndexes(notPrinted);

            var result = new List<IEnumerable<object>>();

            foreach(var iRowIndex in rowIndexes){
                var row = new List<object>();
                foreach(var index in headerIndexes){
                    var val = FindValue(source, index.Union(iRowIndex).ToList(), 0);
                    row.Add(val);
                }
                result.Add(row);
            }

            return result;
        }
        private int CalculateWidth(IEnumerable<GroupingResult> source){
            var result = default(int);
            if (!source.First().GroupValues.First().IsHorisontal){
                result = source.Count();
            }
            else {
                result = source.Select(e => CalculateWidth(e.GroupValues)).Sum(); 
            }
            return result;
        }
        private int CalculateHeight(IEnumerable<GroupingResult> values){

            var result = default(int);
            if (values.First().GroupValues.First().GroupValues == null){
                result = values.GroupBy(e => e.Key).Count();
            }else {

                var grouped = values.GroupBy(e => e.Key);

                result = grouped.Select(e => CalculateHeight(e.SelectMany(ee => ee.GroupValues))).Sum();
            }
            return result;
        }

        private IEnumerable<List<object>> GetHeaderIndexes(IEnumerable<GroupingResult> values){
            if (!values.First().GroupValues.First().IsHorisontal){
                return values.Select(e => new List<object>(){e.Key});
            }
            var result = new List<List<object>>();
            foreach(var item in values){
                var keys = GetHeaderIndexes(item.GroupValues).ToList();
                foreach(var iKey in keys){
                    iKey.Insert(0, item.Key); // keys.Contains(iKey) == false !!!
                }
                result.AddRange(keys);
            }
            return result;
        }
        private IEnumerable<List<object>> GetOtherIndexes(IEnumerable<GroupingResult> unprinted){
            if (unprinted.First().GroupValues.First().GroupValues == null){
                return unprinted.GroupBy(e => e.Key).Select(e => new List<object>(){e.Key});;
            }
            var result = new List<List<object>>();
            var grouped = unprinted.GroupBy(e => e.Key);
            foreach(var item in grouped){
                var keys = GetOtherIndexes(item.SelectMany(e => e.GroupValues)).ToList();
                foreach(var iKey in keys){
                    iKey.Insert(0, item.Key);
                }
                result.AddRange(keys);
            }
            return result;
        }

        private object FindValue(IEnumerable<GroupingResult> source, IList<object> keys, int deepIndx){
            if (deepIndx == keys.Count()){
                return source.Single().Key;
            }
            var found = source.SingleOrDefault(e => e.Key.Equals(keys[deepIndx]));
            if (found == null){
                return string.Empty;
            }
            return FindValue(found.GroupValues, keys, deepIndx + 1);
        }
    }
}