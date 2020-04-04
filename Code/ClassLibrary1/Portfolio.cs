using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;

namespace ClassLibrary1
{
    public class Portfolio : Instrument
    {
        private List<Instrument> instruments;
        private Column weights;
        private double initial_cash;
        private Benchmark bench;

        // attributs pour l'optimisation de portefeuille
        private double[] weight_min_vol;
        private double[] weight_max_sharpe_ratio;
        private double min_volat;
        private double max_sharpe_ratio;

        #region Constructors
        public Portfolio(string ticker, string fill_method = "fillprev", string path = null)
            : base(ticker)
        {
            LoadPrices(fill_method, path);
            ComputeVaRs();
        }


        public Portfolio(Instrument instrument)
            : base(AskForTicker())
        {
            this.instruments = new List<Instrument>();
            instruments.Add(instrument);

            this.weights = new Column(1, 1);
            SetUp();

            ComputeVaRs();
        }

        public Portfolio(Instrument[] instruments, double[] weights = null)
            : base(AskForTicker())
        {
            this.instruments = new List<Instrument>(instruments);

            if (weights != null)
            {
                this.weights = new Column(weights);
            }
            else
            {
                List<double> new_weights = new List<double>();
                for (int i = 0; i < instruments.Length; ++i)
                    new_weights.Add(1.0 / instruments.Length);
                this.weights = new Column(new_weights.ToArray());
            }
            SetUp();
            ComputeVaRs();
        }

        public Portfolio(List<Instrument> instruments, List<double> weights = null)
            : base(AskForTicker())
        {
            this.instruments = instruments;
            if (weights != null)
            {
                this.weights = new Column(weights.ToArray());
            }
            else
            {
                List<double> new_weights = new List<double>();
                for (int i = 0; i < instruments.Count; ++i)
                    new_weights.Add(1.0 / instruments.Count);
                this.weights = new Column(new_weights.ToArray());
            }
            SetUp();
            ComputeVaRs();
        }

        // Create optimized portfolio
        public Portfolio(string ticker, Instrument[] instruments, double[] weights, Benchmark bench = null)
            : base(ticker)
        {
            this.instruments = new List<Instrument>(instruments);
            
            this.weights = new Column(weights);

            this.bench = bench;

            SetUp();
            ComputeVaRs();
        }
        #endregion

        #region Accesseur
        public int NbAsset => instruments.Count;
        public Benchmark Bench => bench;
        public double Alpha => Stats.SimpleLinearRegression(this.returns, this.Bench.returns).Item1;
        public double Beta => Stats.SimpleLinearRegression(this.returns, this.Bench.returns).Item2;
        public double Initial_cash => initial_cash;
        public List<Instrument> Instruments => instruments;
        public Column Weights => weights;

        // - Si la TE est comprise en 0 et 2 : Gestion passive
        // - Si la TE est comprise entre 2 et 10 : Gestion active
        // - Si la TE est supérieure à 10 : le benchmark ne convient pas
        public double Tracking_error => this.returns.DiffMat(this.Bench.returns).StdDev();
        #endregion

        #region Privates
        private void SetUp()
        {
            GetInitialCash();

            this.dates = new DateTime[instruments[0].Dates.Length];
            Array.Copy(instruments.First().Dates, this.dates, instruments[0].Dates.Length);

            SetPrices();
            SetReturns();
        }

        private void SetPrices()
        {
            this.prices = new Column(Length)
            {
                [0] = this.initial_cash
            };

            // Nombre de parts de chaque titre
            double[] shares = new double[NbAsset];
            for (int j = 0; j < NbAsset; ++j)
            {
                shares[j] = this.initial_cash * weights[j] / instruments[j].Prices[0];
            }

            for (int i = 1; i < Length; ++i)
            {
                for (int j = 0; j < NbAsset; ++j)
                {
                    this.prices[i] += shares[j] * instruments[j].Prices[i];
                }
            }
        }

        private void LoadPrices(string fill_method, string path)
        {
            ExcelApplication app = new ExcelApplication();

            base.LoadPrices(app, path);
            Asset[] list = app.ParseAssets(fill_method);
            instruments = new List<Instrument>(list);

            List<double> new_weights = new List<double>(instruments.Count);
            for (int i = 0; i < instruments.Count; ++i)
                new_weights.Add(1.0 / instruments.Count);
            this.weights = new Column(new_weights.ToArray());
            SetUp();
        }

        private static string AskForTicker()
        {
            return Interaction.InputBox("Entrer un nom pour le nouveau portefeuille", "Nouveau portefeuille", "Portefeuille", 10, 10);
        }

        public bool AddBench(Benchmark b)
        {
            if (b.Length != this.Length)
                return false;
            if (b.Dates.Except(this.Dates).ToArray().Length != 0 || this.Dates.Except(b.Dates).ToArray().Length != 0)
                return false;

            this.bench = b;
            return true;
        }

        private void GetInitialCash()
        {
            if (this is Benchmark)
            {
                this.initial_cash = 1000;
                return;
            }

            double cash = 0;
            while (!Double.TryParse(Interaction.InputBox("Entrer un montant d'investissement initial dans le portefeuille", "Nouveau portefeuille", "1000", 10, 10), out cash))
                continue;

            this.initial_cash = cash;
        }
        #endregion

        new public Matrix Get_Prices_Interval(int idx_start, int idx_end)
        {
            if (idx_start > idx_end)
            {
                throw new ArgumentException("l'intervalle doit etre positif");
            }

            if (idx_end > dates.Length || idx_start > dates.Length || idx_start < 0 || idx_end < 0)
                throw new ArgumentOutOfRangeException();

            List<Matrix1D> list = new List<Matrix1D>();
            foreach (Instrument instr in instruments)
                list.Add(instr.Get_Prices_Interval(idx_start, idx_end));

            return new Matrix(list.ToArray());
        }
    
        public new Matrix Get_Returns_Interval(int idx_start, int idx_end)
        {
            if (idx_start > idx_end)
            {
                throw new ArgumentException("l'intervalle doit etre positif");
            }

            if (idx_end > dates.Length || idx_start > dates.Length || idx_start < 0 || idx_end < 0)
                throw new ArgumentOutOfRangeException();

            List<Matrix1D> list = new List<Matrix1D>();
            foreach (Instrument instr in instruments)
                list.Add(instr.Get_Returns_Interval(idx_start, idx_end));

            return new Matrix(list.ToArray());
        }

        public Instrument GetInstrument(string ticker)
        {
            return instruments.Find((e) => e.Ticker == ticker);
        }

        #region Optimisation
        // Création de la matrice de rendements moyen pour chaque actif
        public Column Average_assets_returns()
        {
            Column average_assets_returns = new Column(NbAsset);

            for (int j = 0; j < NbAsset; j++)
            {
                average_assets_returns[j] = this.instruments[j].Returns.Average();
            }
            return average_assets_returns;
        }

        // Moyenne annualisée des rendements du portefeuille
        public double Avgs_returns_annualised_from_assets(Column average_assets_returns)
        {
            // On annualise juste si la période est supérieur à un an 
            if (NbDates - 1 > 252)
            {
                return this.weights.TransMat().MultMat(average_assets_returns) * 252;
            }
            else
            {
                return this.weights.TransMat().MultMat(average_assets_returns);

            }

        }

        // Volatilité du portefeuille calculé à partir de la matrice de variance covariance des actifs
        public double Volatility_annualised_from_assets(Matrix assets_varcov_mat)

        {
            if (NbDates - 1 > 252)
            {
                return Math.Sqrt(this.weights.TransMat().MultMat(assets_varcov_mat * 252).MultMat(this.weights));
            }

            else
            {
                return Math.Sqrt(this.weights.TransMat().MultMat(assets_varcov_mat).MultMat(this.weights));
            }
        }

        // Minimum de la volatilité portefeuille, on choisit 1 pour calculer le minimum de la volatilité et 2 pour le maximum du sharpe ratio
        public double Optimisation_Simul(double[,] a, int choixopti, int NbAsset, int n_simulation)
        {
            ;
            if (choixopti == 1)
            {
                min_volat = a[0, NbAsset + 1];
                for (int i_simul = 0; i_simul < n_simulation; i_simul++)
                {
                    if (a[i_simul, NbAsset + 1] < min_volat)
                    {
                        min_volat = a[i_simul, NbAsset + 1];
                    }
                }
                return min_volat;
            }
            else
            {
                max_sharpe_ratio = 0;
                for (int i_simul = 0; i_simul < n_simulation; i_simul++)
                {
                    if (a[i_simul, NbAsset + 2] > max_sharpe_ratio)
                    {
                        max_sharpe_ratio = a[i_simul, NbAsset + 2];
                    }
                }
                return max_sharpe_ratio;
            }
        }

        // Somme des poids des actifs 
        public double SumAsset(int NbAsset, Column WeightOpt)
        {
            double SumWeight = 0;
            for (int i = 0; i < NbAsset; i++)
            {
                SumWeight += WeightOpt[i];
            }

            return SumWeight;
        }
        // Normalisation des poids 
        public Column NormalizeWeight(Column WeightOpt, double SumWeight, int NbAsset)

        {
            for (int num_weight = 0; num_weight < NbAsset; num_weight++)
            {
                WeightOpt[num_weight] /= SumWeight;
            }

            return WeightOpt;
        }

        //Optimisation des poids avec la méthode de simulation
        public double[] weight_opti_simul_min(int NbAsset, double MethodeOpti, double[,] a, int n_simulation)
        {

            double[] weight_opti_min = new double[NbAsset];
            for (int i_sim = 0; i_sim < n_simulation; i_sim++)
            {


                if (a[i_sim, NbAsset + 1] == MethodeOpti)
                {
                    for (int i = 0; i < NbAsset; i++)
                    {
                        weight_opti_min[i] = a[i_sim, i];
                    }
                }
            }
            return weight_opti_min;
        }
        public double[] weight_opti_simul_max(int NbAsset, double MethodeOpti, double[,] a, int n_simulation)
        {

            double[] weight_opti_max = new double[NbAsset];
            for (int i_sim = 0; i_sim < n_simulation; i_sim++)
            {


                if (a[i_sim, NbAsset + 2] == MethodeOpti)
                {
                    for (int i = 0; i < NbAsset; i++)
                    {
                        weight_opti_max[i] = a[i_sim, i];
                    }
                }


            }

            return weight_opti_max;
        }

        // L'utilisateur choisit 1 pour avoir les poids lui retournant une volatilité minimum et 2 pour les poids lui retournant un ratio de sharpe minimum.
        public double[] Run_simulation(int n_simulation, int MethodeChoix)
        {

            int n_weights = NbAsset;
            double[,] results = new double[n_simulation, NbAsset + 3];

            Matrix assets_returns = this.Returns_To_matrix();
            Column average_assets_returns = Average_assets_returns();
            Matrix assets_varcov_mat = assets_returns.Varcov(); // Calcul de la matrice de varcov des actifs du portefeuille

            for (int i_sim = 0; i_sim < n_simulation; i_sim++)
            {
                this.weights = Get_random_weights(); // On modifie les poids du portefeuille

                // On calcule la moyenne annualisée des rendements du portefeuille, la volatilité et le ratio de sharpe
                // Une autre méthode serait de recalculer les prix du pf à chaque simulation et de réinitialiser ces attributs
                double ptf_avgs_ret_annual = this.Avgs_returns_annualised_from_assets(average_assets_returns);
                double ptf_vola = this.Volatility_annualised_from_assets(assets_varcov_mat);
                double ptf_sharpe_ratio = (ptf_avgs_ret_annual - Market.risk_free_rate) / ptf_vola;

                // poids pour chaque actif + rendement du portefeuille + volatilité du portefeuille  + ratio de sharpe ensemble sur la même ligne pour chaque simulation
                for (int i_asset = 0; i_asset < n_weights; i_asset++)
                {
                    results[i_sim, i_asset] = this.weights[i_asset];
                }
                results[i_sim, NbAsset] = ptf_avgs_ret_annual;
                results[i_sim, NbAsset + 1] = ptf_vola;
                results[i_sim, NbAsset + 2] = ptf_sharpe_ratio;
            }

            //Les calculs se font en fonction du choix de l'utilisateur 
            if (MethodeChoix == 1)
            {
                // On cherche le minimum de la volatilité dans toutes les simulations 
                Optimisation_Simul(results, 1, NbAsset, n_simulation);
                // On extrait les poids de la ligne où se trouve le minimum de la volatilité.
                weight_min_vol = weight_opti_simul_min(NbAsset, min_volat, results, n_simulation);

                return weight_min_vol;

            }
            else
            {
                // On cherche le maximum du sharpe dans toutes les simulations 
                Optimisation_Simul(results, 2, NbAsset, n_simulation);

                // On extrait les poids de la ligne où le maximum du ratio de sharpe
                weight_max_sharpe_ratio = weight_opti_simul_min(NbAsset, max_sharpe_ratio, results, n_simulation);

                return weight_max_sharpe_ratio;
            }


        }

        // Autre méthode pour optimiser les poids d'un portefeuille
        public Column GetOptimizedWeights()
        {
            // On calule la moyenne des rendements par actif du portefeuille et on les stocke

            Matrix assets_returns = this.Returns_To_matrix();
            Column average_assets_returns = Average_assets_returns();
            Matrix assets_varcov_mat = assets_returns.Varcov();
            Matrix Inverse_CoVar = assets_varcov_mat.Inverse(); // On inverse la matrice CoVar

            Column assets_returns_opti = average_assets_returns - Market.risk_free_rate;

            // On determine les poids optimaux 
            Column WeightOpt = new Column(NbAsset);

            // On multiplie la matrice inversée par les actifs 
            WeightOpt = Inverse_CoVar.MultMat(assets_returns_opti);

            // On calcule la somme des actifs 
            double sum_of_weights = SumAsset(NbAsset, WeightOpt);

            // On verifie si la somme est égal, si non on les normalise pour que la somme soit égale à 1. 
            if (sum_of_weights != 1)
            {
                NormalizeWeight(WeightOpt, sum_of_weights, NbAsset);
            }
            return WeightOpt;
        }

        #endregion

        public Column Get_random_weights()
        {
            Column weights = new Column(NbAsset);
            Random random = new Random();

            for (int num_weight = 0; num_weight < NbAsset; num_weight++)
                weights[num_weight] = random.NextDouble();

            double s = 0;
            foreach (double weight in weights)
                s += weight;

            // On normalise les poids pour que la somme soit égale à 1 

            NormalizeWeight(weights, s, NbAsset);
            return new Column(weights);
        }

        // Transforme un portefeuille en Matrice de prix
        public Matrix Prices_To_matrix()
        {
            double[,] Ar;

            Ar = new double[NbDates,NbAsset];

            Column[] Cls = new Column[NbAsset];

            for (int i=0; i< NbAsset;i++)

            {
                Cls[i]=instruments[i].Prices;
            }

            return new Matrix(Cls);
        }

        // Transforme un portefeuille en Matrice de rendements
        public Matrix Returns_To_matrix()
        {
        
            Column[] Cls = new Column[NbAsset];

            for (int i = 0; i < NbAsset; i++)

            {
                Cls[i] = instruments[i].Returns;
            }

            return new Matrix(Cls);
        }

        // Fusion de deux portefeuilles pour récupérer leurs prix dans une seule Matrix
        public static Matrix Merge_Portfolio_Options(Portfolio pf1, Portfolio pf2, string Option)
        {
            int id1 = 0;
            int id2= pf1.NbDates - 1;

            Matrix Ret1;
            double[,] Ret2;

            switch (Option)
            {
                case "Returns":
                    Ret1 = pf1.Get_Returns_Interval(id1, id2);
                    Ret2 = pf2.Returns_To_matrix().GetArray();
                    break;
                case "Prices":
                    Ret1 = pf1.Get_Prices_Interval(id1, id2);
                    Ret2 = pf2.Prices_To_matrix().GetArray();
                    break;
                default:
                    throw new ArgumentException("Option must be Returns or Prices");
            }

            double[,] Ret2_InterFilled =new double[id2+1,  pf2.NbAsset];

            int id3;
            for (int i = 0; i < pf1.NbDates; i++)
            {
                if (pf2.Date_Exists(pf1.Get_Date_index(i)))
                {
                    id3 = pf2.Get_Index_Date(pf1.Get_Date_index(i));
                    for (int j = 0; j < pf2.NbAsset; j++)
                    {
                        Ret2_InterFilled[i, j] = Ret2[id3, j];
                    }
                }
                else
                {
                    for (int j = 0; j < pf2.NbAsset; j++)
                    {
                        if (i == 0)
                        {
                            Ret2_InterFilled[i, j] = 0;
                        }
                        else
                        {
                            Ret2_InterFilled[i, j] = Ret2_InterFilled[i - 1, j];
                        }
                    }
                }
            }
            Ret1.Concat_Me(Ret2_InterFilled, 1);
            return Ret1 ;
        }

        public string[] GetInstrumentsTickers()
        {
            Instrument[] list = this.instruments.ToArray();
            string[] tickers = new string[list.Length];
            for (int i = 0; i < list.Length; ++i)
            {
                tickers[i] = list[i].Ticker;
            }

            return tickers;
        }
    }


    public class Benchmark : Portfolio
    {
        public Benchmark(string ticker, string fill_method = "fillprev", string path = null)
            : base(ticker, fill_method, path){}
    }
}
