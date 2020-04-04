using System;
using ClassLibrary1;
using Main;
using System.IO;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace UnitTestProject1
{
    [TestClass]
    public class BacktestTest
    {
        
        Column returns = new Column(new double[6] { 0.1, 0.2, 0.4, 0.6, 0.8, 0.9 });
        Column bench = new Column(new double[6] { 0.1, 0.1, 0.2, 0.2, 0.1, 0.2 });

        [TestMethod]
        public void VarH()
        {
            double c = Backtest.VaR_Historique(returns, 2);
            Assert.AreEqual(c, 0.17678,0.00001);
        }

        [TestMethod]
        public void VarP()
        {
            double c = Backtest.VaR_Parametrique(returns, 2);
            Assert.AreEqual(c, -0.68481, 0.00001);
        }

        [TestMethod]
        public void VarMC()
        {
            double c = Backtest.VaR_MonteCarlo(returns, 2); // VaR MC
            double d = Math.Abs(0.53 - c); //0.53 valeur arbitraire mais proche de celles données par les simulations
            bool e = false;
            if (d < 0.045)
            {
                e = true;
            }
            Assert.IsTrue(e);
        }

        [TestMethod]
        public void BacktestVaR()
        {
            (double a, double b) =  Backtest.BacktestVaR(Backtest.VaR_Parametrique, returns, 2);
            (double c, double d) = Backtest.BacktestVaR(Backtest.VaR_Historique, returns, 2);
            (double e, double f) = Backtest.BacktestVaR(Backtest.VaR_MonteCarlo, returns, 2);
            // NB breach
            Assert.AreEqual(a, 0);
            Assert.AreEqual(c, 0);
            Assert.AreEqual(e, 0);

            // Binom test
            Assert.AreEqual(b, 0.81451, 0.00001);
            Assert.AreEqual(d, 0.81451, 0.00001);
            Assert.AreEqual(f, 0.81451, 0.00001);
        }

        [TestMethod]
        public void JB()
        {
            bool e = Backtest.JBTest(returns);
            Assert.IsFalse(e);            
        }

        [TestMethod]
        public void Base100()
        {
            double[] res = new double[7] {100,110,132,184.8,295.68,532.2240,1011.2256 };
            Matrix M = Backtest.Base(returns);
            for (int i = 0; i < res.Length; i++)
            {
                Assert.AreEqual(res[i],M[i, 0], 0.0001);
            }
        }

        [TestMethod]
        public void ExcessPerf()
        {
            double[] res = new double[7] {100, 100, 108.70114, 125.45749, 161.54936, 241.10863, 351.90583 };
            Matrix M = Backtest.ExcessPerf(returns, bench);
            for (int i = 0; i<res.Length;i++)
            {
                Assert.AreEqual(res[i], M[i,0], 0.00001);
            }


        }

        [TestMethod]
        public void Beta()
        {
            Column x = new Column(new double[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            Column Y = new Column(new double[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 });
            double b = Backtest.Beta(Y, x);
            Assert.AreEqual(b, 0, 0.01);
        }

        [TestMethod]
        public void TrackinError()
        {
            double c = Backtest.TrackingError(returns, bench);
            Assert.AreEqual(c, 0.27538, 0.00001);
        }

        [TestMethod]
        public void Spe_Syst_Risk()
        {
            Column x = new Column(new double[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            Column Y = new Column(new double[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 });
            (double syst, double spe) = Backtest.Systematic_Specific_Risk(Y, x);
            Assert.AreEqual(Math.Round(syst), 0);
            Assert.AreEqual(40.98780, spe, 0.00001);

        }

        [TestMethod]
        public void TotalRisk()
        {
            double c = Backtest.TotalRisk(returns);
            Assert.AreEqual(c, 4.67333, 0.00001);
        }
    }
}
