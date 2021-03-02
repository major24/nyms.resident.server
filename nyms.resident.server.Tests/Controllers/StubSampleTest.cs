using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace nyms.resident.server.Tests.Controllers
{
    public interface IStockFeed
    {
        double GetStockPrice(string company);
    }

    public class StockData
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }

    public class StockAnalyzer
    {
        private IStockFeed _stockFeed;
        public StockAnalyzer(IStockFeed stockFeed)
        {
            _stockFeed = stockFeed;
        }


        public StockData GetData(string name)
        {
            var p = _stockFeed.GetStockPrice(name);

            return new StockData() { Name = name, Price = p };
        }
    }



    [TestClass]
    public class StubSampleTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            // arrange
            Mock<IStockFeed> mock = new Mock<IStockFeed>();
            mock.Setup(m => m.GetStockPrice("msoft")).Returns(120.00);

            // act
            var actual = new StockAnalyzer(mock.Object).GetData("msoft");

            // assert
            Assert.AreEqual(120.00, actual.Price);
            Assert.AreEqual("msoft", actual.Name);

        }
    }
}
