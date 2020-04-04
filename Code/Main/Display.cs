using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using ClassLibrary1;
using Microsoft.VisualBasic;

namespace Main
{
    public partial class Display : Form
    {
        private List<Portfolio> portfolios;
        private List<Strategy> strategies;
        private List<Asset> assets;

        private double total_invest;

        public List<Portfolio> Portfolios { get => portfolios; }
        public List<Strategy> Strategies { get => strategies; }
        public List<Asset> Assets { get => assets; }

        public Display()
        {
            portfolios = new List<Portfolio>();
            assets = new List<Asset>();
            strategies = new List<Strategy>();
            total_invest = 0;
            InitializeComponent();
            statusUpdate();
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        #region Custom Handlers
        private void portfolio_Custom(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            Portfolio selected = portfolios.Find(delegate (Portfolio p) { return p.Ticker == item.Text; });

            Helpers.stats_Change(selected, statsContainer, stats);
            bench_box.Visible = selected.Bench == null;
        }

        private void asset_Custom(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            Asset selected = assets.Find(delegate (Asset a) { return a.Ticker == item.Text; });

            Helpers.stats_Change(selected, statsContainer, stats);
            bench_box.Visible = false;
        }

        private void strategy_Custom(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            Strategy selected = strategies.Find(delegate (Strategy s) { return s.Ticker == item.Text; });

            Helpers.stats_Change(selected, statsContainer, stats);
            bench_box.Visible = false;
        }
        #endregion

        #region FileOpen
        private void choosePortfolio_Click(object sender, EventArgs e)
        {
            choosePortfolio.ShowDialog();
        }

        private void addPortfolio_Click(object sender, EventArgs e)
        {
            choosePortfolio.ShowDialog();
        }

        private void addAsset_Click(object sender, EventArgs e)
        {
            chooseAsset.ShowDialog();
        }

        private void add_bench_Click(object sender, EventArgs e)
        {
            chooseBenchmark.ShowDialog();
        }

        private void choosePortfolio_FileOK(object sender, CancelEventArgs e)
        {
            string ticker = Path.GetFileNameWithoutExtension(choosePortfolio.FileName);

            if (listPortfolio.DropDownItems.Find(ticker, false).Length == 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                Portfolio p = new Portfolio(ticker, "fillprev", choosePortfolio.FileName);
                statusUpdate(p);

                if (!portfolios.Contains(p))
                    portfolios.Add(p);

                ToolStripMenuItem item = (ToolStripMenuItem)listPortfolio.DropDownItems.Add(p.Ticker, null, portfolio_Custom);
                item.Name = p.Ticker;
                Helpers.stats_Change(p, statsContainer, stats);
                bench_box.Visible = p.Bench == null;
                Cursor.Current = Cursors.Default;
            }
            else
            {
                ErrorDialog err = new ErrorDialog("Cet objet est déjà dans la solution.", "Ticker : " + ticker);
                err.ShowDialog();
            }
        }

        private void statusUpdate(Instrument i = null)
        {
            if (i != null)
            {
                if (i is Portfolio)
                    total_invest += ((Portfolio)i).Initial_cash;
                else if (i is Strategy)
                    total_invest += ((Strategy)i).Initial_invest;
            }
            status2.Text = String.Format("{0:0.00}", total_invest);
        }

        private void chooseAsset_FileOk(object sender, CancelEventArgs e)
        {
            string ticker = Path.GetFileNameWithoutExtension(chooseAsset.FileName);

            if (listAsset.DropDownItems.Find(ticker, false).Length == 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                Asset a = new Asset(ticker, "fillprev", chooseAsset.FileName);

                if (!assets.Contains(a))
                    assets.Add(a);

                ToolStripMenuItem item = (ToolStripMenuItem)listAsset.DropDownItems.Add(a.Ticker, null, asset_Custom);
                item.Name = a.Ticker;
                Helpers.stats_Change(a, statsContainer, stats);
                bench_box.Visible = false;
                Cursor.Current = Cursors.Default;
            }
            else
            {
                ErrorDialog err = new ErrorDialog("Cet objet est déjà dans la solution.", "Ticker : " + ticker);
                err.ShowDialog();
            }
        }

        private void chooseBenchmark_FileOk(object sender, CancelEventArgs e)
        {
            string ticker = Path.GetFileNameWithoutExtension(chooseBenchmark.FileName);

            Cursor.Current = Cursors.WaitCursor;
            Benchmark b = new Benchmark(ticker, "fillprev", chooseBenchmark.FileName);

            Portfolio current = portfolios.Find(delegate (Portfolio p) { return p.Ticker == statsContainer.Text; });

            if (!current.AddBench(b))
            {
                ErrorDialog err = new ErrorDialog("Ce benchmark ne possède pas les memes dates que le portefeuille.", "Nombre : " + b.NbDates);
                err.ShowDialog();
            }

            Helpers.stats_Change(current, statsContainer, stats);
            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region Custom Forms
        private void buildPortfolio_Click(object sender, EventArgs e)
        {
            BuildPortfolio build = new BuildPortfolio(portfolios, assets);

            DialogResult result = build.ShowDialog();

            if (result == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                Portfolio p = new Portfolio(build.Returns);

                if (listPortfolio.DropDownItems.Find(p.Ticker, false).Length == 0)
                {
                    statusUpdate(p);

                    if (!portfolios.Contains(p))
                        portfolios.Add(p);

                    ToolStripMenuItem item = (ToolStripMenuItem)listPortfolio.DropDownItems.Add(p.Ticker, null, portfolio_Custom);
                    item.Name = p.Ticker;
                    Helpers.stats_Change(p, statsContainer, stats);
                    bench_box.Visible = true;
                }
                else
                {
                    ErrorDialog err = new ErrorDialog("Cet objet est déjà dans la solution.", "Ticker : " + p.Ticker);
                    err.ShowDialog();
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void createStrategy_Click(object sender, EventArgs e)
        {
            CreateStrategy strat = new CreateStrategy(portfolios);

            DialogResult result = strat.ShowDialog();

            if (result == DialogResult.OK)
            {
                Strategy s = strat.Return;

                if (listStrategy.DropDownItems.Find(s.Ticker, false).Length == 0)
                {
                    statusUpdate(s);

                    if (!strategies.Contains(s))
                        strategies.Add(s);

                    ToolStripMenuItem item = (ToolStripMenuItem)listStrategy.DropDownItems.Add(s.Ticker, null, strategy_Custom);
                    item.Name = s.Ticker;
                    Helpers.stats_Change(s, statsContainer, stats);
                    bench_box.Visible = false;
                }
                else
                {
                    ErrorDialog err = new ErrorDialog("Cet objet est déjà dans la solution.", "Ticker : " + s.Ticker);
                    err.ShowDialog();
                }
            }
        }
        #endregion
        private Instrument FindInstrument(string ticker)
        {
            Instrument current;
            try
            {
                current = portfolios.Find(delegate (Portfolio p) { return p.Ticker == statsContainer.Text; });
            }
            catch
            {
                try
                {
                    current = assets.Find(delegate (Asset a) { return a.Ticker == statsContainer.Text; });
                }
                catch
                {
                    current = strategies.Find(delegate (Strategy s) { return s.Ticker == statsContainer.Text; });
                }
            }
            return current;
        }

        private void backtest_export_Click(object sender, EventArgs e)
        {
            Backtest.DrawPortfolioValue(FindInstrument(statsContainer.Text));
        }

        private void compo_export_Click(object sender, EventArgs e)
        {
            Backtest.DrawPortfolioCompo(FindInstrument(statsContainer.Text));
        }

        private void quit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void optimize_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            
            Portfolio p = (Portfolio)FindInstrument(statsContainer.Text);
            Column optims = p.GetOptimizedWeights();

            string ticker = p.Ticker + "_optimized";
            Portfolio res = new Portfolio(ticker, p.Instruments.ToArray(), optims.ArrayFrom1D(), p.Bench);

            if (listPortfolio.DropDownItems.Find(ticker, false).Length == 0)
            {
                if (!portfolios.Contains(res))
                    portfolios.Add(res);

                ToolStripMenuItem item = (ToolStripMenuItem)listPortfolio.DropDownItems.Add(ticker, null, portfolio_Custom);
                item.Name = res.Ticker;
                Helpers.stats_Change(res, statsContainer, stats);
                bench_box.Visible = res.Bench == null;
            }
            else
            {
                ErrorDialog err = new ErrorDialog("Cet objet est déjà dans la solution.", "Ticker : " + res.Ticker);
                err.ShowDialog();
            }
            Cursor.Current = Cursors.Default;
        }

        private void create_strat_menu_Click(object sender, EventArgs e)
        {
            createStrategy_Click(null, e);
        }
    }

    public static class Helpers
    {
        public static void stats_Change(Instrument selected, GroupBox statsContainer, TableLayoutPanel stats)
        {
            Control[] exports = statsContainer.Parent.Controls.Find("export_box", false);
            if (exports.Length >= 1)
                exports[0].Visible = true;
            statsContainer.Text = selected.Ticker;
            statsContainer.Visible = true;

            IEnumerable<TextBox> values = stats.Controls.OfType<TextBox>();
            foreach (TextBox tb in values)
            {
                switch (tb.Name)
                {
                    case "volat_value":
                        tb.Text = selected.Volatility.ToString("P");
                        break;
                    case "volat_annu_value":
                        tb.Text = selected.Volatility_annualised.ToString("P");
                        break;
                    case "var_param_value":
                        try
                        {
                            tb.Text = selected.Var_series["VaR Paramétrique"].Daily.ToString("P");
                        }
                        catch (Exception e)
                        {
                            if (selected.Var_series.Count == 1)
                                tb.Text = selected.Var_series.First().Value.Daily.ToString("P");
                            else
                                throw e;
                        }
                        break;
                    case "total_perf_value":
                        tb.Text = selected.Total_perf.ToString("P");
                        break;
                    case "last_price_value":
                        tb.Text = String.Format("{0:0.00}", selected.Prices[selected.Prices.Length - 1]);
                        break;
                    case "sharpe_value":
                        tb.Text = String.Format("{0:0.00}", selected.SharpeRatio);
                        break;
                    case "bench_value":
                        if (selected is Asset)
                        {
                            stats.Controls.Find("bench", false)[0].Visible = false;
                            tb.Visible = false;
                        }
                        else
                        {
                            stats.Controls.Find("bench", false)[0].Visible = true;
                            tb.Visible = true;
                            if (selected is Portfolio)
                            {
                                if (((Portfolio)selected).Bench == null)
                                    tb.Text = "Pas de benchmark";
                                else
                                    tb.Text = ((Portfolio)selected).Bench.Ticker;
                            }
                            else
                            {
                                if (((Strategy)selected).Assets.Bench == null)
                                    tb.Text = "Pas de benchmark";
                                else
                                    tb.Text = ((Strategy)selected).Assets.Bench.Ticker;
                            }
                        }
                        break;
                    case "track_value":
                        if (selected is Asset)
                        {
                            stats.Controls.Find("track", false)[0].Visible = false;
                            tb.Visible = false;
                        }
                        else
                        {
                            stats.Controls.Find("track", false)[0].Visible = true;
                            tb.Visible = true;

                            if (selected is Portfolio)
                            {
                                if (((Portfolio)selected).Bench == null)
                                    tb.Text = "Pas de benchmark";
                                else
                                    tb.Text = String.Format("{0:0.00}", ((Portfolio)selected).Tracking_error);
                            }
                            else
                            {
                                if (((Strategy)selected).Assets.Bench == null)
                                    tb.Text = "Pas de benchmark";
                                else
                                    tb.Text = String.Format("{0:0.00}", ((Strategy)selected).Assets.Tracking_error);
                            }
                        }
                        break;
                    case "pct_breach_value":
                        tb.Text = selected.Var_series["VaR Paramétrique"].PctBreach.ToString("P");
                        break;
                    case "beta_value":
                        if (selected is Asset)
                        {
                            stats.Controls.Find("beta", false)[0].Visible = false;
                            tb.Visible = false;
                        }
                        else
                        {
                            stats.Controls.Find("beta", false)[0].Visible = true;
                            tb.Visible = true;
                            if (selected is Portfolio)
                            {
                                if (((Portfolio)selected).Bench == null)
                                    tb.Text = "Pas de benchmark";
                                else
                                    tb.Text = String.Format("{0:0.00}", ((Portfolio)selected).Beta);
                            }
                            else
                            {
                                if (((Strategy)selected).Assets.Bench == null)
                                    tb.Text = "Pas de benchmark";
                                else
                                    tb.Text = String.Format("{0:0.00}", ((Strategy)selected).Assets.Beta);
                            }
                        }
                        break;
                    case "period_value":
                        tb.Text = selected.Dates[0].ToString("dd/MM/yyyy") + " - " + selected.Dates[selected.Length - 1].ToString("dd/MM/yyyy");
                        break;
                    case "initial_cash_value":
                        if (selected is Asset)
                        {
                            stats.Controls.Find("initial_cash", false)[0].Visible = false;
                            tb.Visible = false;
                        }
                        else
                        {
                            stats.Controls.Find("initial_cash", false)[0].Visible = true;
                            tb.Visible = true;
                            if (selected is Strategy)
                            {
                                tb.Text = String.Format("{0:0.00}", ((Strategy)selected).Initial_invest);
                            }
                            else
                            {
                                tb.Text = String.Format("{0:0.00}", ((Portfolio)selected).Initial_cash);
                            }
                        }
                        break;
                    default:
                        throw new NotImplementedException("Something went wrong");
                }
            }
        }
    }
}
