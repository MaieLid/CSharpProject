using System;
using NUnit.Framework;
using ClassLibrary1;
using Main;
using System.IO;

namespace UnitTestProject1
{
    [TestFixture]
    public class PortfolioTest
    {
        Asset asset;
        int NbAsset_T;
        double[,] weight_test;
        double[,] average_test;
        double Avgs_returns_all_assets_t;
        double Volat_ptf_t;
        double sum_T;
        Double[,] assets_varcov_mat_T;
        double[,] result_test;
        int test_simul;
        double min_test;
        double max_test;
        Portfolio ptf = new Portfolio("output_INDICES"); // not nice but simple fix to multiple pop-ups

        [SetUp]
        public void SetUp()
        {
            asset = new Asset("SX5E Index");
            NbAsset_T = 7;
            test_simul = 3;
            min_test = 0.05;
            max_test = 0.08;
            weight_test = new double[7, 1]


            {
                { 0.4 },
                { 0.1 },
                { 0.8 },
                { 0.3 },
                { 0.4 },
                { 0.1 },
                { 0.8 },
            };
            sum_T = 2.9;


            average_test = new double[7, 1]
            {
                { 0.000967},
                { 0.001082 },
                {  0.000221},
                { 0.000350},
                { 0.000144},
                { 0.000240 },
                {  0.000199},
            };

            Avgs_returns_all_assets_t = 0.00035;

            Volat_ptf_t = 0.001569117;

            assets_varcov_mat_T = new Double[7, 7]
           {
                {0.0000561, 0.0000561, -0.0000048, 0.0000148, 0.0000047, 0.0000171, -0.000006 },
                {0.0000561, 0.0000562, -0.0000048, 0.0000148, 0.0000047, 0.0000172, -0.0000069},
                { -0.0000048,    -0.0000048 ,  0.0000015,  -0.0000015,  -0.0000005 , -0.0000019,   0.0000025},
                { 0.0000148,   0.0000148,  -0.0000015,   0.0000112,   0.0000014 ,  0.0000050,  -0.0000016 },
                { 0.0000047,   0.0000047,  -0.0000005,   0.0000014,   0.0000007,   0.0000025,  -0.0000008 },
                { 0.0000171 ,  0.0000172,  -0.0000019 ,  0.0000050 ,  0.0000025,   0.0000096,  -0.0000029 },
                {-0.0000069 , -0.0000069,   0.0000025,  -0.0000016,  -0.0000008,  -0.0000029,   0.0000075 },
           };

            result_test = new Double[3, 10]
            {
                {0.1, 0.2, 0.1, 0.2, 0.1, 0.1, 0.2 , 0.06, 0.05, 0.04 },
                {0.2, 0.1, 0.2, 0.1, 0.3, 0.1, 0 , 0.05, 0.07, 0.04 },
                {0.1, 0.2, 0.1, 0.2, 0.1, 0.1, 0.2 , 0.08, 0.06, 0.08 },

           };


        }

        [Test]
        public void BasicCreation()
        {
            CollectionAssert.AreEqual(ptf.GetInstrument("SX5E Index").Prices, asset.Prices);
        }

        [Test]
        public void DateOrder()
        {
            DateTime[] sorted = new DateTime[ptf.Dates.Length];
            Array.Copy(ptf.Dates, sorted, ptf.Dates.Length);
            Array.Sort(sorted);
            CollectionAssert.AreEqual(sorted, ptf.Dates);
        }

        [Test]
        public void MultipleAssets()
        {
            Assert.IsNotNull(ptf.Prices);
            Assert.IsNotNull(ptf.Returns);
        }

        [Test]
        public void NormalizeWeT()
        {
            Column weight = new Column(weight_test);
            Column result = ptf.NormalizeWeight(weight, sum_T, NbAsset_T);
            Assert.AreEqual(result.Sum(), 1);
        }

        [Test]
        public void SumAssetT()
        {
            Column weight = new Column(weight_test);
            Double result = ptf.SumAsset(NbAsset_T, weight);
            Assert.AreEqual(result, sum_T, 0.00001);
        }

        [Test]
        public void Average_assets_returnsT()
        {
            Column average_assets_returns = new Column(NbAsset_T);
            Column result = ptf.Average_assets_returns();
            CollectionAssert.AreEqual(result, average_test, new DoubleComparer(0.0001));
        }

        [Test]
        public void Avgs_returns_annualised_from_assetsT()
        {
            Column weight = new Column(weight_test);
            Column average_assets_returns = new Column(NbAsset_T);
            double result = ptf.Avgs_returns_annualised_from_assets(average_assets_returns);
            Assert.AreEqual(result, Avgs_returns_all_assets_t, 0.00001);
        }

        [Test]
        public void Volatility_annualised_from_assets()
        {
            Matrix assets_varcov_mat_m = new Matrix(assets_varcov_mat_T);
            double result = ptf.Volatility_annualised_from_assets(assets_varcov_mat_m);
            Assert.AreEqual(result, Volat_ptf_t, 0.00001);
        }


        [Test]
        public void weight_opti_simulT1()
        {
            double[] result = ptf.weight_opti_simul_min(NbAsset_T, min_test, result_test, test_simul);
            Column result_c = new Column(result);
            Assert.AreEqual(result_c.Sum(), 1, 0.00001);
        }

        [Test]
        public void weight_opti_simulT2()
        {
            double[] result = ptf.weight_opti_simul_max(NbAsset_T, max_test, result_test, test_simul);
            Column result_c = new Column(result);
            Assert.AreEqual(result_c.Sum(), 1, 0.00001);
        }


        [Test]
        public void Get_Random_WeightT()
        {
            Column result = ptf.Get_random_weights();
            Assert.AreEqual(result.Sum(), 1, 0.00001);

        }


        [Test]
        public void OptionSimul_Test1()
        {
            double result = ptf.Optimisation_Simul(result_test, 1, NbAsset_T, test_simul);
            Assert.Greater(result, 0);

        }

        [Test]
        public void OptionSimul_Test2()
        {
            double result = ptf.Optimisation_Simul(result_test, 2, NbAsset_T, test_simul);
            Assert.Less(result, 4);

        /*
        }
        [Test] Celui ci ne tourne dans le vide il ne me met ni rouge ni vert
        public void GetOptimizedWeightsT()
        {
            Column result = ptf.GetOptimizedWeights();
            Assert.AreEqual(result.Sum(), 1, 0.00001);
        */

        }
        [Test]
        public void Run_simulationT1()
        {
            double[] result = ptf.Run_simulation(test_simul, 1);
            Column result_c = new Column(result);
            Assert.AreEqual(result_c.Sum(), 1, 0.00001);
        }

        [Test]
        public void Run_simulationT2()
        {
            double[] result = ptf.Run_simulation(test_simul, 2);
            Column result_c = new Column(result);
            Assert.AreEqual(result_c.Sum(), 1, 0.00001);
        }

    }
}
