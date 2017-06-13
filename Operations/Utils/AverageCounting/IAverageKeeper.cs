namespace Operations.Utils.AverageCounting{

    public interface IAverageKeeper {
        void Add(double val);
        double? CurrentAverage {get;}
    }

}