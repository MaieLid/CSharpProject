using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    public abstract class Instrument
    {
        protected string ticker;
        protected DateTime[] dates;
        protected Column prices;
        protected Column returns;
        protected Dictionary<string, VaR> var_series;

        #region Constructeurs
        public Instrument(string ticker)
        {
            this.ticker = ticker;
            this.dates = null;
            this.prices = null;
            this.returns = null;
            this.var_series = new Dictionary<string, VaR>();
        }

        public Instrument(string ticker, DateTime[] dates, double[] prices)
        {
            this.ticker = ticker;
            this.dates = dates;
            this.prices = new Column(prices);
            this.returns = null;
            this.var_series = new Dictionary<string, VaR>();
        }

        public Instrument(Instrument copy)
        {
            this.ticker = copy.ticker;
            this.dates = new DateTime[copy.dates.Length];
            Array.Copy(copy.dates, this.dates, copy.dates.Length);
            this.prices = new Column(copy.prices);
            this.returns = new Column(copy.returns);
            this.var_series = copy.Var_series;
        }
        #endregion

        #region Accesseurs
        public string Ticker => ticker;
        public DateTime[] Dates => dates;
        public int NbDates => this.dates.Length;
        public Column Returns => returns;
        public Column Prices => prices;
        public int Length => dates.Length;
        public double Total_perf => prices[prices.Length - 1] / prices[0] - 1;
        public double Volatility => returns.StdDev();
        public double Volatility_annualised => returns.StdDev() * Math.Sqrt(252);
        public double Average_returns => returns.Average();
        public Dictionary<string, VaR> Var_series => var_series;
        public double SharpeRatio => (this.Average_returns - Market.risk_free_rate) / this.Volatility;
        #endregion

        protected void LoadPrices(ExcelApplication app, string path)
        {
            app.OpenWorkbook(this.ticker + ".xlsx", path);
        }

        protected void SetReturns(string method = "variation")
        {
            returns = this.prices.CalculateReturns(method);
        }

        protected void ComputeVaRs()
        {
            this.AddVaR("VaR Paramétrique");
            this.AddVaR("VaR Historique");
            this.AddVaR("VaR MonteCarlo");
        }

        public Column Get_Prices_Interval(DateTime start, DateTime end)
        {
            return Get_Interval(start, end, true);
        }

        public Column Get_Prices_Interval(int start, int end)
        {
            return Get_Interval(start, end, true);
        }

        public Column Get_Returns_Interval(DateTime start, DateTime end)
        {
            return Get_Interval(start, end, false);
        }

        public Column Get_Returns_Interval(int start, int end)
        {
            return Get_Interval(start, end, false);
        }

        private Column Get_Interval(DateTime start, DateTime end, bool prices)
        {
            if (start > end)
                throw new ArgumentException("l'intervalle doit etre positif");

            int i = 0;
            while (i < dates.Length && dates[i] < start)
            {
                ++i;
            }

            int j = i;
            while (j < dates.Length && dates[i] <= end)
            {
                ++j;
            }

            return Get_Interval(i, j, prices);
        }
            
        private Column Get_Interval(int idx_start, int idx_end, bool prices)
        {
            if (idx_start >= dates.Length || idx_end < 0) { throw new ArgumentOutOfRangeException(); }
            if (idx_start > idx_end) { throw new ArgumentException("l'intervalle doit etre positif"); }

            double[] res = new double[idx_end - idx_start + 1];
            Array.Copy(prices ? this.prices.ArrayFrom1D() : this.returns.ArrayFrom1D(), idx_start, res, 0, idx_end - idx_start + 1);
            return new Column(res);
        }

        private void AddVaR(string name)
        {
            switch (name)
            {
                case "VaR Paramétrique":
                    this.var_series.Add(name, new VaR(Backtest.VaR_Parametrique, this.Returns));
                    break;
                case "VaR Historique":
                    this.var_series.Add(name, new VaR(Backtest.VaR_Historique, this.Returns));
                    break;
                case "VaR MonteCarlo":
                    this.var_series.Add(name, new VaR(Backtest.VaR_MonteCarlo, this.Returns));
                    break;
                default:
                    throw new NotImplementedException("VaR non existante");
            }
        }

        // Retourne vrai ou faux si la date est présente dans l'instrument
        public bool Date_Exists(DateTime dt)
        {
            return dates.Contains(dt);
        }

        // Récuperer l'indice d'une date
        public int Get_Index_Date(DateTime dt)
        {
            for (int i = 0; i < dates.Length; ++i)
            {
                if (dates[i].Equals(dt))
                    return i;
            }

            throw new Exception("La date n'existe pas dans les dates des prix du portefeuille");
        }


        // Récuperer la date d'un indice
        public DateTime Get_Date_index(int idx)
        {
            return dates.ElementAt(idx);
        }
    }

    public class InstrumentComparer : IEqualityComparer<Instrument>
    {
        public bool Equals(Instrument x, Instrument y)
        {
            return x.Ticker == y.Ticker;
        }

        public int GetHashCode(Instrument obj)
        {
            throw new NotImplementedException();
        }
    }
}
