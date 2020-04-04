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
    public partial class CreateStrategy : Form
    {
        private List<Portfolio> portfolios;
        private Strategy ret;

        public Strategy Return { get => ret; }

        public CreateStrategy(List<Portfolio> portfolios)
        {
            this.portfolios = portfolios;

            InitializeComponent();

            foreach (Portfolio p in portfolios)
            {
                if (p.Bench != null) // assets.Bench est utilisé dans le constructeur de Strategy
                    assets_box.Items.Add(p.Ticker);
                futures_box.Items.Add(p.Ticker);
            }
        }

        private void ticker_value_TextChanged(object sender, EventArgs e)
        {
            validate();
        }

        private void type_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            validate();
        }

        private void assets_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (futures_box.SelectedIndex > -1)
            {
                updateDatesProposed();
            }
            validate();
        }

        private void hedge_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            validate();
        }

        private void futures_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (assets_box.SelectedIndex > -1)
            {
                updateDatesProposed();
            }

            validate();
        }

        private void invest_value_TextChanged(object sender, EventArgs e)
        {
            validate();
        }

        private void start_date_ValueChanged(object sender, EventArgs e)
        {
            validate();
        }

        private void end_date_ValueChanged(object sender, EventArgs e)
        {
            validate();
        }

        private void roll_period_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (double.TryParse(roll_amount.Text, out double res))
            {
                double roll_Mult;
                if (roll_period.Text == "Semaine")
                {
                    roll_Mult = 5*res;

                }
                else if (roll_period.Text == "Mois")
                {
                    roll_Mult = 21 * res;
                }
                else
                {
                    throw new NotSupportedException("La période de roll doit être semaine ou mois");
                }
                restrainDatesProposed((int)roll_Mult);
            }

            validate();
        }

        private void roll_amount_TextChanged(object sender, EventArgs e)
        {
            if (roll_period.SelectedIndex > -1 && double.TryParse(roll_amount.Text, out double res))
            {
                double roll_Mult;
                if (roll_period.Text == "Semaine")
                {
                    roll_Mult = 5 * res;

                }
                else if (roll_period.Text == "Mois")
                {
                    roll_Mult = 21 * res;
                }
                else
                {
                    throw new NotSupportedException("La période de roll doit être semaine ou mois");
                }
                restrainDatesProposed((int)roll_Mult);
            }

            validate();
        }

        private void validate()
        {
            validate_button.Enabled = false;

            if (string.IsNullOrWhiteSpace(ticker_value.Text))
                return;

            if (type_box.SelectedIndex <= -1 || hedge_box.SelectedIndex <= -1)
                return;

            double res;
            if (!double.TryParse(invest_value.Text, out res))
                return;

            if (assets_box.SelectedIndex <= -1 || futures_box.SelectedIndex <= -1)
                return;

            if (roll_period.SelectedIndex <= -1)
                return;

            if (!double.TryParse(roll_amount.Text, out res))
                return;

            validate_button.Enabled = true;
        }

        private void updateDatesProposed()
        {
            Portfolio assets = portfolios.Find(delegate (Portfolio p) { return p.Ticker == assets_box.Text; });
            Portfolio futures = portfolios.Find(delegate (Portfolio p) { return p.Ticker == futures_box.Text; });
            start_date.Enabled = true;
            end_date.Enabled = true;
            start_date.Items.Clear();
            end_date.Items.Clear();
            foreach (DateTime date in assets.Dates)
            {
                start_date.Items.Add(date.ToString("dd/MM/yyyy"));
                end_date.Items.Add(date.ToString("dd/MM/yyyy"));
            }
            start_date.SelectedItem = start_date.Items[0];
            end_date.SelectedItem = end_date.Items[end_date.Items.Count-1];
        }
        private void restrainDatesProposed(int index_first_date)
        {
            Portfolio assets = portfolios.Find(delegate (Portfolio p) { return p.Ticker == assets_box.Text; });
            Portfolio futures = portfolios.Find(delegate (Portfolio p) { return p.Ticker == futures_box.Text; });
            start_date.Enabled = true;
            end_date.Enabled = true;
            start_date.Items.Clear();
            end_date.Items.Clear();
            for (int i = index_first_date; i < assets.NbDates; i++)
            {
                start_date.Items.Add(assets.Dates[i].ToString("dd/MM/yyyy"));
                end_date.Items.Add(assets.Dates[i].ToString("dd/MM/yyyy"));
            }
            start_date.SelectedItem = start_date.Items[0];
            end_date.SelectedItem = end_date.Items[end_date.Items.Count-1];
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void validate_button_Click(object sender, EventArgs e)
        {
            Portfolio assets = portfolios.Find(delegate (Portfolio p) { return p.Ticker == assets_box.Text; });
            Portfolio futures = portfolios.Find(delegate (Portfolio p) { return p.Ticker == futures_box.Text; });
            DateTime dateSt = DateTime.ParseExact(start_date.Text, "dd/MM/yyyy",null);
            DateTime dateEnd = DateTime.ParseExact(end_date.Text, "dd/MM/yyyy", null);
            Cursor.Current = Cursors.WaitCursor;
            ret = new Strategy(ticker_value.Text, assets, futures, dateSt, dateEnd, double.Parse(invest_value.Text), type_box.Text, hedge_box.Text,int.Parse(roll_amount.Text),roll_period.Text);
            Cursor.Current = Cursors.Default;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
