namespace Operations.Utils.AverageCounting {

    public class AverageKeeperFactory: IAverageKeeperFactory {
        public IAverageKeeper Create => new AverageKeeper();
    }

}