using System;

namespace Operations.Utils.AverageCounting {

    public class AverageKeeper : IAverageKeeper
    {
        public double? CurrentAverage {get; private set;}

        public void Add(double val)
        {
            if (!CurrentAverage.HasValue){
                CurrentAverage = 0;
            }
            CurrentAverage += (val - CurrentAverage) / (_Count + 1);
            _Count++;
        }

        private int _Count = 0;
    }

}