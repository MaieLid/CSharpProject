using System;
using ClassLibrary1;
using Main;
using System.IO;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class StatsTest
    {
        [TestMethod]
        public void Choose()
        {
            long n = 5;
            long k = 2;
            BigInteger c = Stats.Choose(n, k);
            Assert.AreEqual(c, 10);
        }

        [TestMethod]
        public void BinomialTest()
        {
            long n = 3;
            long k = 2;
            double c = Stats.BinomialTest(k, n, 0.5);
            Assert.AreEqual(c, 0.375);
        }

        [TestMethod]
        public void SimpleLinearRegression()
        {
            Column x = new Column(new double[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            Column Y = new Column(new double[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 });
            (double a, double b) = Stats.SimpleLinearRegression(Y, x);
            Assert.AreEqual(a,1,0.01);
            Assert.AreEqual(b,0,0.01);
        }

        [TestMethod]
        public void InverseNormalDistribution()
        {
            double c = Stats.InverseNormalDistribution(0.95);
            Assert.AreEqual(c,1.6448536,0.00001);
        }
    }
}
