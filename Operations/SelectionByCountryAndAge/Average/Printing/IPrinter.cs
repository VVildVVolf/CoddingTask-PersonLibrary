using System;
using System.Collections.Generic;

namespace Operations.SelectionByCountryAndAge.Average.Printing {

    public interface IPrinter{
        void Print(IDictionary<Tuple<string, int>, double> aggregatedResult);
    }

}