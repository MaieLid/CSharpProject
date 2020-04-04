using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    public class Asset : Instrument
    {
        public Asset(string ticker, string method = "fillprev", string path = null)
            : base(ticker)
        {
            LoadPrices(method, path);
            SetReturns();
            ComputeVaRs();
        }

        public Asset(string ticker, DateTime[] dates, double[] prices)
            : base(ticker, dates, prices)
        {
            SetReturns();
            ComputeVaRs();
        }

        public Asset(Asset assetCopy)
            : base(assetCopy)
        { }

        private void LoadPrices(string method, string path)
        {
            ExcelApplication app = new ExcelApplication();

            base.LoadPrices(app, path);
            Tuple<DateTime[], double[]> tmp = app.ParseAsset(method);

            dates = new DateTime[tmp.Item1.Length];
            Array.Copy(tmp.Item1, dates, tmp.Item1.Length);
            prices = new Column(tmp.Item2);
        }
    }
}