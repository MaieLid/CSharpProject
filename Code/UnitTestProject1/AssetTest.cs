using System;
using NUnit.Framework;
using ClassLibrary1;
using Main;
using System.IO;

namespace UnitTestProject1
{
    [TestFixture]
    public class AssetTest
    {
        double[] prices_basic;
        DateTime[] dates_basic;

        [SetUp]
        public void SetUp()
        {
            prices_basic = new double[]
            {
                1, 2, 3, 4, 5, 6, 7, 8
            };

            dates_basic = new DateTime[]
            {
                new DateTime(2008, 5, 1, 8, 30, 52), new DateTime(2008, 5, 2, 8, 30, 52),
                new DateTime(2008, 5, 3, 8, 30, 52), new DateTime(2008, 5, 4, 8, 30, 52),
                new DateTime(2008, 5, 5, 8, 30, 52), new DateTime(2008, 5, 6, 8, 30, 52),
                new DateTime(2008, 5, 7, 8, 30, 52), new DateTime(2008, 5, 8, 8, 30, 52),
            };
        }

        [Test]
        public void BasicCreation()
        {
            string name = "new_asset";
            Asset asset = new Asset(name, dates_basic, prices_basic);
            CollectionAssert.AreEqual(prices_basic, asset.Prices);
            CollectionAssert.AreEqual(dates_basic, asset.Dates);
            Assert.AreEqual(name, asset.Ticker);
        }

        [Test]
        public void BasicLoad()
        {
            string name = "SX5E Index";
            Asset asset = new Asset(name);
            Assert.AreEqual(asset.Prices.Length, asset.Dates.Length);
            Assert.AreEqual(227, asset.Prices.Length);
            Assert.AreNotEqual(asset.Dates[0], asset.Dates[1]);
            Assert.AreNotEqual(asset.Prices[0], asset.Prices[1]);
        }

        [Test]
        public void BrokenLoad()
        {
            Asset asset;
            string name = "this asset does not exist";
            Exception a = Assert.Throws<Exception>(() => asset = new Asset(name));
            Assert.That(a.Message.EndsWith("n'existe pas."));
        }
    }
}
