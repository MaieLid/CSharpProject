namespace Main
{
    partial class BuildPortfolio
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listInstruments = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.validate = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.statsContainer = new System.Windows.Forms.GroupBox();
            this.stats = new System.Windows.Forms.TableLayoutPanel();
            this.period = new System.Windows.Forms.Label();
            this.total_perf = new System.Windows.Forms.Label();
            this.total_perf_value = new System.Windows.Forms.TextBox();
            this.last_price_value = new System.Windows.Forms.TextBox();
            this.last_price = new System.Windows.Forms.Label();
            this.volat = new System.Windows.Forms.Label();
            this.volat_value = new System.Windows.Forms.TextBox();
            this.volat_annu = new System.Windows.Forms.Label();
            this.volat_annu_value = new System.Windows.Forms.TextBox();
            this.sharpe = new System.Windows.Forms.Label();
            this.var_param = new System.Windows.Forms.Label();
            this.sharpe_value = new System.Windows.Forms.TextBox();
            this.var_param_value = new System.Windows.Forms.TextBox();
            this.pct_breach = new System.Windows.Forms.Label();
            this.pct_breach_value = new System.Windows.Forms.TextBox();
            this.bench_value = new System.Windows.Forms.TextBox();
            this.bench = new System.Windows.Forms.Label();
            this.beta_value = new System.Windows.Forms.TextBox();
            this.beta = new System.Windows.Forms.Label();
            this.track_value = new System.Windows.Forms.TextBox();
            this.track = new System.Windows.Forms.Label();
            this.period_value = new System.Windows.Forms.TextBox();
            this.statsContainer.SuspendLayout();
            this.stats.SuspendLayout();
            this.SuspendLayout();
            // 
            // listInstruments
            // 
            this.listInstruments.CheckOnClick = true;
            this.listInstruments.FormattingEnabled = true;
            this.listInstruments.Location = new System.Drawing.Point(15, 40);
            this.listInstruments.Name = "listInstruments";
            this.listInstruments.Size = new System.Drawing.Size(308, 169);
            this.listInstruments.TabIndex = 0;
            this.listInstruments.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listInstruments_ItemCheck);
            this.listInstruments.SelectedIndexChanged += new System.EventHandler(this.listInstruments_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(313, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Choisir les instruments qui vont composer le nouveau portefeuille.";
            // 
            // validate
            // 
            this.validate.Enabled = false;
            this.validate.Location = new System.Drawing.Point(250, 226);
            this.validate.Name = "validate";
            this.validate.Size = new System.Drawing.Size(75, 23);
            this.validate.TabIndex = 2;
            this.validate.Text = "Valider";
            this.validate.UseVisualStyleBackColor = true;
            this.validate.Click += new System.EventHandler(this.validate_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(169, 226);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 3;
            this.cancel.Text = "Annuler";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // statsContainer
            // 
            this.statsContainer.Controls.Add(this.stats);
            this.statsContainer.Location = new System.Drawing.Point(331, 9);
            this.statsContainer.Name = "statsContainer";
            this.statsContainer.Size = new System.Drawing.Size(312, 240);
            this.statsContainer.TabIndex = 4;
            this.statsContainer.TabStop = false;
            this.statsContainer.Text = "Default";
            this.statsContainer.Visible = false;
            // 
            // stats
            // 
            this.stats.ColumnCount = 2;
            this.stats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.stats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.stats.Controls.Add(this.period, 0, 10);
            this.stats.Controls.Add(this.total_perf, 0, 0);
            this.stats.Controls.Add(this.total_perf_value, 1, 0);
            this.stats.Controls.Add(this.last_price_value, 1, 4);
            this.stats.Controls.Add(this.last_price, 0, 4);
            this.stats.Controls.Add(this.volat, 0, 2);
            this.stats.Controls.Add(this.volat_value, 1, 2);
            this.stats.Controls.Add(this.volat_annu, 0, 3);
            this.stats.Controls.Add(this.volat_annu_value, 1, 3);
            this.stats.Controls.Add(this.sharpe, 0, 1);
            this.stats.Controls.Add(this.var_param, 0, 5);
            this.stats.Controls.Add(this.sharpe_value, 1, 1);
            this.stats.Controls.Add(this.var_param_value, 1, 5);
            this.stats.Controls.Add(this.pct_breach, 0, 6);
            this.stats.Controls.Add(this.pct_breach_value, 1, 6);
            this.stats.Controls.Add(this.bench_value, 1, 7);
            this.stats.Controls.Add(this.bench, 0, 7);
            this.stats.Controls.Add(this.beta_value, 1, 8);
            this.stats.Controls.Add(this.beta, 0, 8);
            this.stats.Controls.Add(this.track_value, 1, 9);
            this.stats.Controls.Add(this.track, 0, 9);
            this.stats.Controls.Add(this.period_value, 1, 10);
            this.stats.Location = new System.Drawing.Point(6, 19);
            this.stats.Name = "stats";
            this.stats.RowCount = 12;
            this.stats.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stats.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stats.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stats.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stats.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stats.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stats.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stats.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stats.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stats.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stats.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.stats.Size = new System.Drawing.Size(283, 280);
            this.stats.TabIndex = 14;
            // 
            // period
            // 
            this.period.AutoSize = true;
            this.period.Location = new System.Drawing.Point(3, 190);
            this.period.Name = "period";
            this.period.Size = new System.Drawing.Size(43, 13);
            this.period.TabIndex = 21;
            this.period.Text = "Période";
            // 
            // total_perf
            // 
            this.total_perf.AutoSize = true;
            this.total_perf.Location = new System.Drawing.Point(3, 0);
            this.total_perf.Name = "total_perf";
            this.total_perf.Size = new System.Drawing.Size(96, 13);
            this.total_perf.TabIndex = 4;
            this.total_perf.Text = "Performance totale";
            // 
            // total_perf_value
            // 
            this.total_perf_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.total_perf_value.Location = new System.Drawing.Point(119, 3);
            this.total_perf_value.Name = "total_perf_value";
            this.total_perf_value.ReadOnly = true;
            this.total_perf_value.Size = new System.Drawing.Size(146, 13);
            this.total_perf_value.TabIndex = 5;
            // 
            // last_price_value
            // 
            this.last_price_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.last_price_value.Location = new System.Drawing.Point(119, 79);
            this.last_price_value.Name = "last_price_value";
            this.last_price_value.ReadOnly = true;
            this.last_price_value.Size = new System.Drawing.Size(146, 13);
            this.last_price_value.TabIndex = 11;
            // 
            // last_price
            // 
            this.last_price.AutoSize = true;
            this.last_price.Location = new System.Drawing.Point(3, 76);
            this.last_price.Name = "last_price";
            this.last_price.Size = new System.Drawing.Size(60, 13);
            this.last_price.TabIndex = 10;
            this.last_price.Text = "Dernier prix";
            // 
            // volat
            // 
            this.volat.AutoSize = true;
            this.volat.Location = new System.Drawing.Point(3, 38);
            this.volat.Name = "volat";
            this.volat.Size = new System.Drawing.Size(46, 13);
            this.volat.TabIndex = 0;
            this.volat.Text = "Volatilité";
            // 
            // volat_value
            // 
            this.volat_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.volat_value.Location = new System.Drawing.Point(119, 41);
            this.volat_value.Name = "volat_value";
            this.volat_value.ReadOnly = true;
            this.volat_value.Size = new System.Drawing.Size(146, 13);
            this.volat_value.TabIndex = 1;
            // 
            // volat_annu
            // 
            this.volat_annu.AutoSize = true;
            this.volat_annu.Location = new System.Drawing.Point(3, 57);
            this.volat_annu.Name = "volat_annu";
            this.volat_annu.Size = new System.Drawing.Size(100, 13);
            this.volat_annu.TabIndex = 2;
            this.volat_annu.Text = "Volatilité annualisée";
            // 
            // volat_annu_value
            // 
            this.volat_annu_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.volat_annu_value.Location = new System.Drawing.Point(119, 60);
            this.volat_annu_value.Name = "volat_annu_value";
            this.volat_annu_value.ReadOnly = true;
            this.volat_annu_value.Size = new System.Drawing.Size(146, 13);
            this.volat_annu_value.TabIndex = 3;
            // 
            // sharpe
            // 
            this.sharpe.AutoSize = true;
            this.sharpe.Location = new System.Drawing.Point(3, 19);
            this.sharpe.Name = "sharpe";
            this.sharpe.Size = new System.Drawing.Size(84, 13);
            this.sharpe.TabIndex = 8;
            this.sharpe.Text = "Ratio de Sharpe";
            // 
            // var_param
            // 
            this.var_param.AutoSize = true;
            this.var_param.Location = new System.Drawing.Point(3, 95);
            this.var_param.Name = "var_param";
            this.var_param.Size = new System.Drawing.Size(110, 13);
            this.var_param.TabIndex = 6;
            this.var_param.Text = "VaR Paramétrique 5%";
            // 
            // sharpe_value
            // 
            this.sharpe_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sharpe_value.Location = new System.Drawing.Point(119, 22);
            this.sharpe_value.Name = "sharpe_value";
            this.sharpe_value.ReadOnly = true;
            this.sharpe_value.Size = new System.Drawing.Size(146, 13);
            this.sharpe_value.TabIndex = 9;
            // 
            // var_param_value
            // 
            this.var_param_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.var_param_value.Location = new System.Drawing.Point(119, 98);
            this.var_param_value.Name = "var_param_value";
            this.var_param_value.ReadOnly = true;
            this.var_param_value.Size = new System.Drawing.Size(146, 13);
            this.var_param_value.TabIndex = 7;
            // 
            // pct_breach
            // 
            this.pct_breach.AutoSize = true;
            this.pct_breach.Location = new System.Drawing.Point(3, 114);
            this.pct_breach.Name = "pct_breach";
            this.pct_breach.Size = new System.Drawing.Size(104, 13);
            this.pct_breach.TabIndex = 17;
            this.pct_breach.Text = "Pourcentage breach";
            // 
            // pct_breach_value
            // 
            this.pct_breach_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pct_breach_value.Location = new System.Drawing.Point(119, 117);
            this.pct_breach_value.Name = "pct_breach_value";
            this.pct_breach_value.ReadOnly = true;
            this.pct_breach_value.Size = new System.Drawing.Size(146, 13);
            this.pct_breach_value.TabIndex = 20;
            // 
            // bench_value
            // 
            this.bench_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.bench_value.Location = new System.Drawing.Point(119, 136);
            this.bench_value.Name = "bench_value";
            this.bench_value.ReadOnly = true;
            this.bench_value.Size = new System.Drawing.Size(146, 13);
            this.bench_value.TabIndex = 13;
            // 
            // bench
            // 
            this.bench.AutoSize = true;
            this.bench.Location = new System.Drawing.Point(3, 133);
            this.bench.Name = "bench";
            this.bench.Size = new System.Drawing.Size(61, 13);
            this.bench.TabIndex = 12;
            this.bench.Text = "Benchmark";
            // 
            // beta_value
            // 
            this.beta_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.beta_value.Location = new System.Drawing.Point(119, 155);
            this.beta_value.Name = "beta_value";
            this.beta_value.ReadOnly = true;
            this.beta_value.Size = new System.Drawing.Size(146, 13);
            this.beta_value.TabIndex = 19;
            // 
            // beta
            // 
            this.beta.AutoSize = true;
            this.beta.Location = new System.Drawing.Point(3, 152);
            this.beta.Name = "beta";
            this.beta.Size = new System.Drawing.Size(29, 13);
            this.beta.TabIndex = 14;
            this.beta.Text = "Beta";
            // 
            // track_value
            // 
            this.track_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.track_value.Location = new System.Drawing.Point(119, 174);
            this.track_value.Name = "track_value";
            this.track_value.ReadOnly = true;
            this.track_value.Size = new System.Drawing.Size(146, 13);
            this.track_value.TabIndex = 18;
            // 
            // track
            // 
            this.track.AutoSize = true;
            this.track.Location = new System.Drawing.Point(3, 171);
            this.track.Name = "track";
            this.track.Size = new System.Drawing.Size(73, 13);
            this.track.TabIndex = 16;
            this.track.Text = "Tracking error";
            // 
            // period_value
            // 
            this.period_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.period_value.Location = new System.Drawing.Point(119, 193);
            this.period_value.Name = "period_value";
            this.period_value.ReadOnly = true;
            this.period_value.Size = new System.Drawing.Size(146, 13);
            this.period_value.TabIndex = 22;
            // 
            // BuildPortfolio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 258);
            this.Controls.Add(this.statsContainer);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.validate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listInstruments);
            this.Name = "BuildPortfolio";
            this.Text = "Construire un portefeuille";
            this.statsContainer.ResumeLayout(false);
            this.stats.ResumeLayout(false);
            this.stats.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox listInstruments;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button validate;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.GroupBox statsContainer;
        private System.Windows.Forms.TableLayoutPanel stats;
        private System.Windows.Forms.Label period;
        private System.Windows.Forms.Label total_perf;
        private System.Windows.Forms.TextBox total_perf_value;
        private System.Windows.Forms.TextBox last_price_value;
        private System.Windows.Forms.Label last_price;
        private System.Windows.Forms.Label volat;
        private System.Windows.Forms.TextBox volat_value;
        private System.Windows.Forms.Label volat_annu;
        private System.Windows.Forms.TextBox volat_annu_value;
        private System.Windows.Forms.Label sharpe;
        private System.Windows.Forms.Label var_param;
        private System.Windows.Forms.TextBox sharpe_value;
        private System.Windows.Forms.TextBox var_param_value;
        private System.Windows.Forms.Label pct_breach;
        private System.Windows.Forms.TextBox pct_breach_value;
        private System.Windows.Forms.TextBox bench_value;
        private System.Windows.Forms.Label bench;
        private System.Windows.Forms.TextBox beta_value;
        private System.Windows.Forms.Label beta;
        private System.Windows.Forms.TextBox track_value;
        private System.Windows.Forms.Label track;
        private System.Windows.Forms.TextBox period_value;
    }
}