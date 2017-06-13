using Operations.Utils.AverageCounting;
using Xunit;

namespace Operations.Test.Utils.AverageCounting{
    public class AverageKeeperFactoryTest{
        [Fact]
        public void CommonCase(){
            var factory = new AverageKeeperFactory();
            Assert.True(factory.Create != null);
        }
    }
}