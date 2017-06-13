using Operations.Utils.AverageCounting;
using Xunit;

namespace Operations.Test.Utils.AverageCounting{
    public class AverageKeeperTest {
        [Fact]
        public void CommonCounting(){
            var keeper = new AverageKeeper();
            keeper.Add(1);
            keeper.Add(2);
            keeper.Add(3);

            Assert.Equal(2, keeper.CurrentAverage);
        }

        [Fact]
        public void MultitimeCounting(){
            var keeper = new AverageKeeper();
            keeper.Add(1);
            keeper.Add(2);
            keeper.Add(3);
            Assert.Equal(2, keeper.CurrentAverage);
            keeper.Add(4);

            Assert.Equal(2.5, keeper.CurrentAverage);
        }

        [Fact]
        public void SingleValue(){
            var keeper = new AverageKeeper();
            keeper.Add(2);
            Assert.Equal(2, keeper.CurrentAverage);
        }
        
        [Fact]
        public void EmptyCase(){
            var keeper = new AverageKeeper();
            Assert.False(keeper.CurrentAverage.HasValue);
        }
    }
}