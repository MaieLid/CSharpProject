using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main
{
    public partial class BuildPortfolio : Form
    {
        private List<Portfolio> portfolios;
        private List<Asset> assets;
        private List<Instrument> returns;

        public List<Instrument> Returns { get => returns; }

        public BuildPortfolio(List<Portfolio> portfolios, List<Asset> assets)
        {
            this.assets = assets;
            this.portfolios = portfolios;
            this.returns = new List<Instrument>();

            InitializeComponent();
            foreach (Asset a in assets)
                listInstruments.Items.Add(a.Ticker + " (Actif)");

            foreach (Portfolio p in portfolios)
                listInstruments.Items.Add(p.Ticker + " (Portefeuille)");
        }

        private void listInstruments_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string ticker = ((CheckedListBox)sender).SelectedItem.ToString();
                if (ticker.EndsWith(" (Actif)"))
                    Helpers.stats_Change(assets.Find(delegate (Asset a) { return a.Ticker == ticker.Substring(0, ticker.Length - 8); }), statsContainer, stats);
                else
                    Helpers.stats_Change(portfolios.Find(delegate (Portfolio p) { return p.Ticker == ticker.Substring(0, ticker.Length - 15); }), statsContainer, stats);
            }
            catch
            {
                return; // not pretty but avoids crash when event is fired by a click out of any item
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void validate_Click(object sender, EventArgs e)
        {
            foreach(string item in listInstruments.CheckedItems)
            {
                if (item.EndsWith(" (Actif)"))
                    returns.Add(assets.Find(delegate (Asset a) { return a.Ticker == item.Substring(0, item.Length - 8); }));
                else
                    returns.Add(portfolios.Find(delegate (Portfolio a) { return a.Ticker == item.Substring(0, item.Length - 15); }));
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void listInstruments_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int checking = 1;
            if (listInstruments.GetItemChecked(e.Index))
                checking = -1;

            if (listInstruments.CheckedItems.Count + checking > 0)
                validate.Enabled = true;
            else
                validate.Enabled = false;
        }
    }
}
