namespace Main
{
    partial class Display
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Display));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buildPortfolio = new System.Windows.Forms.Button();
            this.clickChoosePortfolio = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.status1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.choosePortfolio = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.file = new System.Windows.Forms.ToolStripDropDownButton();
            this.listPortfolio = new System.Windows.Forms.ToolStripMenuItem();
            this.addPortfolio = new System.Windows.Forms.ToolStripMenuItem();
            this.listAsset = new System.Windows.Forms.ToolStripMenuItem();
            this.addAsset = new System.Windows.Forms.ToolStripMenuItem();
            this.listStrategy = new System.Windows.Forms.ToolStripMenuItem();
            this.quit = new System.Windows.Forms.ToolStripMenuItem();
            this.statsContainer = new System.Windows.Forms.GroupBox();
            this.stats = new System.Windows.Forms.TableLayoutPanel();
            this.total_perf = new System.Windows.Forms.Label();
            this.total_perf_value = new System.Windows.Forms.TextBox();
            this.volat = new System.Windows.Forms.Label();
            this.volat_value = new System.Windows.Forms.TextBox();
            this.volat_annu = new System.Windows.Forms.Label();
            this.volat_annu_value = new System.Windows.Forms.TextBox();
            this.sharpe = new System.Windows.Forms.Label();
            this.sharpe_value = new System.Windows.Forms.TextBox();
            this.initial_cash = new System.Windows.Forms.Label();
            this.initial_cash_value = new System.Windows.Forms.TextBox();
            this.period = new System.Windows.Forms.Label();
            this.period_value = new System.Windows.Forms.TextBox();
            this.track = new System.Windows.Forms.Label();
            this.track_value = new System.Windows.Forms.TextBox();
            this.beta = new System.Windows.Forms.Label();
            this.beta_value = new System.Windows.Forms.TextBox();
            this.bench = new System.Windows.Forms.Label();
            this.bench_value = new System.Windows.Forms.TextBox();
            this.pct_breach = new System.Windows.Forms.Label();
            this.pct_breach_value = new System.Windows.Forms.TextBox();
            this.var_param = new System.Windows.Forms.Label();
            this.var_param_value = new System.Windows.Forms.TextBox();
            this.last_price_value = new System.Windows.Forms.TextBox();
            this.last_price = new System.Windows.Forms.Label();
            this.chooseAsset = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.createStrategy = new System.Windows.Forms.Button();
            this.bench_box = new System.Windows.Forms.GroupBox();
            this.optimize = new System.Windows.Forms.Button();
            this.add_bench = new System.Windows.Forms.Button();
            this.chooseBenchmark = new System.Windows.Forms.OpenFileDialog();
            this.export_box = new System.Windows.Forms.GroupBox();
            this.compo_export = new System.Windows.Forms.Button();
            this.backtest_export = new System.Windows.Forms.Button();
            this.status2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.create_strat_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statsContainer.SuspendLayout();
            this.stats.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.bench_box.SuspendLayout();
            this.export_box.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buildPortfolio);
            this.groupBox1.Controls.Add(this.clickChoosePortfolio);
            this.groupBox1.Location = new System.Drawing.Point(12, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(303, 59);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Créer un portefeuille.";
            // 
            // buildPortfolio
            // 
            this.buildPortfolio.Location = new System.Drawing.Point(155, 19);
            this.buildPortfolio.Name = "buildPortfolio";
            this.buildPortfolio.Size = new System.Drawing.Size(125, 23);
            this.buildPortfolio.TabIndex = 24;
            this.buildPortfolio.Text = "Construire";
            this.buildPortfolio.UseVisualStyleBackColor = true;
            this.buildPortfolio.Click += new System.EventHandler(this.buildPortfolio_Click);
            // 
            // clickChoosePortfolio
            // 
            this.clickChoosePortfolio.Location = new System.Drawing.Point(6, 19);
            this.clickChoosePortfolio.Name = "clickChoosePortfolio";
            this.clickChoosePortfolio.Size = new System.Drawing.Size(125, 23);
            this.clickChoosePortfolio.TabIndex = 0;
            this.clickChoosePortfolio.Text = "Choisir un fichier";
            this.clickChoosePortfolio.UseVisualStyleBackColor = true;
            this.clickChoosePortfolio.Click += new System.EventHandler(this.choosePortfolio_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status1,
            this.status2,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 340);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(629, 26);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // status1
            // 
            this.status1.Name = "status1";
            this.status1.Size = new System.Drawing.Size(145, 21);
            this.status1.Text = "Investissement (cumulé) : ";
            // 
            // choosePortfolio
            // 
            this.choosePortfolio.FileName = "choosePortfolio";
            this.choosePortfolio.FileOk += new System.ComponentModel.CancelEventHandler(this.choosePortfolio_FileOK);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.file});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(629, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // file
            // 
            this.file.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.listPortfolio,
            this.listAsset,
            this.listStrategy,
            this.quit});
            this.file.Image = ((System.Drawing.Image)(resources.GetObject("file.Image")));
            this.file.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.file.Name = "file";
            this.file.Size = new System.Drawing.Size(55, 22);
            this.file.Text = "Fichier";
            // 
            // listPortfolio
            // 
            this.listPortfolio.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPortfolio});
            this.listPortfolio.Name = "listPortfolio";
            this.listPortfolio.Size = new System.Drawing.Size(152, 22);
            this.listPortfolio.Text = "Portefeuilles";
            // 
            // addPortfolio
            // 
            this.addPortfolio.Name = "addPortfolio";
            this.addPortfolio.Size = new System.Drawing.Size(116, 22);
            this.addPortfolio.Text = "Ouvrir...";
            this.addPortfolio.Click += new System.EventHandler(this.addPortfolio_Click);
            // 
            // listAsset
            // 
            this.listAsset.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAsset});
            this.listAsset.Name = "listAsset";
            this.listAsset.Size = new System.Drawing.Size(152, 22);
            this.listAsset.Text = "Actifs";
            // 
            // addAsset
            // 
            this.addAsset.Name = "addAsset";
            this.addAsset.Size = new System.Drawing.Size(116, 22);
            this.addAsset.Text = "Ouvrir...";
            this.addAsset.Click += new System.EventHandler(this.addAsset_Click);
            // 
            // listStrategy
            // 
            this.listStrategy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.create_strat_menu});
            this.listStrategy.Name = "listStrategy";
            this.listStrategy.Size = new System.Drawing.Size(152, 22);
            this.listStrategy.Text = "Stratégies";
            // 
            // quit
            // 
            this.quit.Name = "quit";
            this.quit.Size = new System.Drawing.Size(152, 22);
            this.quit.Text = "Quitter";
            this.quit.Click += new System.EventHandler(this.quit_Click);
            // 
            // statsContainer
            // 
            this.statsContainer.Controls.Add(this.stats);
            this.statsContainer.Location = new System.Drawing.Point(321, 37);
            this.statsContainer.Name = "statsContainer";
            this.statsContainer.Size = new System.Drawing.Size(295, 295);
            this.statsContainer.TabIndex = 3;
            this.statsContainer.TabStop = false;
            this.statsContainer.Text = "Default";
            this.statsContainer.Visible = false;
            // 
            // stats
            // 
            this.stats.ColumnCount = 2;
            this.stats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.stats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.stats.Controls.Add(this.total_perf, 0, 0);
            this.stats.Controls.Add(this.total_perf_value, 1, 0);
            this.stats.Controls.Add(this.volat, 0, 2);
            this.stats.Controls.Add(this.volat_value, 1, 2);
            this.stats.Controls.Add(this.volat_annu, 0, 3);
            this.stats.Controls.Add(this.volat_annu_value, 1, 3);
            this.stats.Controls.Add(this.sharpe, 0, 1);
            this.stats.Controls.Add(this.sharpe_value, 1, 1);
            this.stats.Controls.Add(this.initial_cash, 0, 4);
            this.stats.Controls.Add(this.initial_cash_value, 1, 4);
            this.stats.Controls.Add(this.period, 0, 11);
            this.stats.Controls.Add(this.period_value, 1, 11);
            this.stats.Controls.Add(this.track, 0, 10);
            this.stats.Controls.Add(this.track_value, 1, 10);
            this.stats.Controls.Add(this.beta, 0, 9);
            this.stats.Controls.Add(this.beta_value, 1, 9);
            this.stats.Controls.Add(this.bench, 0, 8);
            this.stats.Controls.Add(this.bench_value, 1, 8);
            this.stats.Controls.Add(this.pct_breach, 0, 7);
            this.stats.Controls.Add(this.pct_breach_value, 1, 7);
            this.stats.Controls.Add(this.var_param, 0, 6);
            this.stats.Controls.Add(this.var_param_value, 1, 6);
            this.stats.Controls.Add(this.last_price_value, 1, 5);
            this.stats.Controls.Add(this.last_price, 0, 5);
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
            this.stats.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.stats.Size = new System.Drawing.Size(280, 270);
            this.stats.TabIndex = 14;
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
            // sharpe_value
            // 
            this.sharpe_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sharpe_value.Location = new System.Drawing.Point(119, 22);
            this.sharpe_value.Name = "sharpe_value";
            this.sharpe_value.ReadOnly = true;
            this.sharpe_value.Size = new System.Drawing.Size(146, 13);
            this.sharpe_value.TabIndex = 9;
            // 
            // initial_cash
            // 
            this.initial_cash.AutoSize = true;
            this.initial_cash.Location = new System.Drawing.Point(3, 76);
            this.initial_cash.Name = "initial_cash";
            this.initial_cash.Size = new System.Drawing.Size(103, 13);
            this.initial_cash.TabIndex = 23;
            this.initial_cash.Text = "Investissement initial";
            // 
            // initial_cash_value
            // 
            this.initial_cash_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.initial_cash_value.Location = new System.Drawing.Point(119, 79);
            this.initial_cash_value.Name = "initial_cash_value";
            this.initial_cash_value.ReadOnly = true;
            this.initial_cash_value.Size = new System.Drawing.Size(146, 13);
            this.initial_cash_value.TabIndex = 24;
            // 
            // period
            // 
            this.period.AutoSize = true;
            this.period.Location = new System.Drawing.Point(3, 209);
            this.period.Name = "period";
            this.period.Size = new System.Drawing.Size(43, 13);
            this.period.TabIndex = 21;
            this.period.Text = "Période";
            // 
            // period_value
            // 
            this.period_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.period_value.Location = new System.Drawing.Point(119, 212);
            this.period_value.Name = "period_value";
            this.period_value.ReadOnly = true;
            this.period_value.Size = new System.Drawing.Size(146, 13);
            this.period_value.TabIndex = 22;
            // 
            // track
            // 
            this.track.AutoSize = true;
            this.track.Location = new System.Drawing.Point(3, 190);
            this.track.Name = "track";
            this.track.Size = new System.Drawing.Size(73, 13);
            this.track.TabIndex = 16;
            this.track.Text = "Tracking error";
            // 
            // track_value
            // 
            this.track_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.track_value.Location = new System.Drawing.Point(119, 193);
            this.track_value.Name = "track_value";
            this.track_value.ReadOnly = true;
            this.track_value.Size = new System.Drawing.Size(146, 13);
            this.track_value.TabIndex = 18;
            // 
            // beta
            // 
            this.beta.AutoSize = true;
            this.beta.Location = new System.Drawing.Point(3, 171);
            this.beta.Name = "beta";
            this.beta.Size = new System.Drawing.Size(29, 13);
            this.beta.TabIndex = 14;
            this.beta.Text = "Beta";
            // 
            // beta_value
            // 
            this.beta_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.beta_value.Location = new System.Drawing.Point(119, 174);
            this.beta_value.Name = "beta_value";
            this.beta_value.ReadOnly = true;
            this.beta_value.Size = new System.Drawing.Size(146, 13);
            this.beta_value.TabIndex = 19;
            // 
            // bench
            // 
            this.bench.AutoSize = true;
            this.bench.Location = new System.Drawing.Point(3, 152);
            this.bench.Name = "bench";
            this.bench.Size = new System.Drawing.Size(61, 13);
            this.bench.TabIndex = 12;
            this.bench.Text = "Benchmark";
            // 
            // bench_value
            // 
            this.bench_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.bench_value.Location = new System.Drawing.Point(119, 155);
            this.bench_value.Name = "bench_value";
            this.bench_value.ReadOnly = true;
            this.bench_value.Size = new System.Drawing.Size(146, 13);
            this.bench_value.TabIndex = 13;
            // 
            // pct_breach
            // 
            this.pct_breach.AutoSize = true;
            this.pct_breach.Location = new System.Drawing.Point(3, 133);
            this.pct_breach.Name = "pct_breach";
            this.pct_breach.Size = new System.Drawing.Size(104, 13);
            this.pct_breach.TabIndex = 17;
            this.pct_breach.Text = "Pourcentage breach";
            // 
            // pct_breach_value
            // 
            this.pct_breach_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pct_breach_value.Location = new System.Drawing.Point(119, 136);
            this.pct_breach_value.Name = "pct_breach_value";
            this.pct_breach_value.ReadOnly = true;
            this.pct_breach_value.Size = new System.Drawing.Size(146, 13);
            this.pct_breach_value.TabIndex = 20;
            // 
            // var_param
            // 
            this.var_param.AutoSize = true;
            this.var_param.Location = new System.Drawing.Point(3, 114);
            this.var_param.Name = "var_param";
            this.var_param.Size = new System.Drawing.Size(110, 13);
            this.var_param.TabIndex = 6;
            this.var_param.Text = "VaR Paramétrique 5%";
            // 
            // var_param_value
            // 
            this.var_param_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.var_param_value.Location = new System.Drawing.Point(119, 117);
            this.var_param_value.Name = "var_param_value";
            this.var_param_value.ReadOnly = true;
            this.var_param_value.Size = new System.Drawing.Size(146, 13);
            this.var_param_value.TabIndex = 7;
            // 
            // last_price_value
            // 
            this.last_price_value.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.last_price_value.Location = new System.Drawing.Point(119, 98);
            this.last_price_value.Name = "last_price_value";
            this.last_price_value.ReadOnly = true;
            this.last_price_value.Size = new System.Drawing.Size(146, 13);
            this.last_price_value.TabIndex = 11;
            // 
            // last_price
            // 
            this.last_price.AutoSize = true;
            this.last_price.Location = new System.Drawing.Point(3, 95);
            this.last_price.Name = "last_price";
            this.last_price.Size = new System.Drawing.Size(60, 13);
            this.last_price.TabIndex = 10;
            this.last_price.Text = "Dernier prix";
            // 
            // chooseAsset
            // 
            this.chooseAsset.FileName = "chooseAsset";
            this.chooseAsset.FileOk += new System.ComponentModel.CancelEventHandler(this.chooseAsset_FileOk);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.createStrategy);
            this.groupBox2.Location = new System.Drawing.Point(12, 102);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(303, 59);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Créer une stratégie.";
            // 
            // createStrategy
            // 
            this.createStrategy.Location = new System.Drawing.Point(6, 19);
            this.createStrategy.Name = "createStrategy";
            this.createStrategy.Size = new System.Drawing.Size(125, 23);
            this.createStrategy.TabIndex = 0;
            this.createStrategy.Text = "Créer";
            this.createStrategy.UseVisualStyleBackColor = true;
            this.createStrategy.Click += new System.EventHandler(this.createStrategy_Click);
            // 
            // bench_box
            // 
            this.bench_box.Controls.Add(this.optimize);
            this.bench_box.Controls.Add(this.add_bench);
            this.bench_box.Location = new System.Drawing.Point(12, 232);
            this.bench_box.Name = "bench_box";
            this.bench_box.Size = new System.Drawing.Size(303, 59);
            this.bench_box.TabIndex = 26;
            this.bench_box.TabStop = false;
            this.bench_box.Text = "Actions sur le portefeuille courant.";
            this.bench_box.Visible = false;
            // 
            // optimize
            // 
            this.optimize.Location = new System.Drawing.Point(155, 19);
            this.optimize.Name = "optimize";
            this.optimize.Size = new System.Drawing.Size(125, 23);
            this.optimize.TabIndex = 1;
            this.optimize.Text = "Optimiser";
            this.optimize.UseVisualStyleBackColor = true;
            this.optimize.Click += new System.EventHandler(this.optimize_Click);
            // 
            // add_bench
            // 
            this.add_bench.Location = new System.Drawing.Point(6, 19);
            this.add_bench.Name = "add_bench";
            this.add_bench.Size = new System.Drawing.Size(125, 23);
            this.add_bench.TabIndex = 0;
            this.add_bench.Text = "Ajouter un benchmark";
            this.add_bench.UseVisualStyleBackColor = true;
            this.add_bench.Click += new System.EventHandler(this.add_bench_Click);
            // 
            // chooseBenchmark
            // 
            this.chooseBenchmark.FileName = "chooseBenchmark";
            this.chooseBenchmark.FileOk += new System.ComponentModel.CancelEventHandler(this.chooseBenchmark_FileOk);
            // 
            // export_box
            // 
            this.export_box.Controls.Add(this.compo_export);
            this.export_box.Controls.Add(this.backtest_export);
            this.export_box.Location = new System.Drawing.Point(12, 167);
            this.export_box.Name = "export_box";
            this.export_box.Size = new System.Drawing.Size(303, 59);
            this.export_box.TabIndex = 27;
            this.export_box.TabStop = false;
            this.export_box.Text = "Exporter l\'objet courant sous Excel.";
            this.export_box.Visible = false;
            // 
            // compo_export
            // 
            this.compo_export.Location = new System.Drawing.Point(155, 19);
            this.compo_export.Name = "compo_export";
            this.compo_export.Size = new System.Drawing.Size(125, 23);
            this.compo_export.TabIndex = 1;
            this.compo_export.Text = "Composition";
            this.compo_export.UseVisualStyleBackColor = true;
            this.compo_export.Click += new System.EventHandler(this.compo_export_Click);
            // 
            // backtest_export
            // 
            this.backtest_export.Location = new System.Drawing.Point(6, 19);
            this.backtest_export.Name = "backtest_export";
            this.backtest_export.Size = new System.Drawing.Size(125, 23);
            this.backtest_export.TabIndex = 0;
            this.backtest_export.Text = "Backtest";
            this.backtest_export.UseVisualStyleBackColor = true;
            this.backtest_export.Click += new System.EventHandler(this.backtest_export_Click);
            // 
            // status2
            // 
            this.status2.Name = "status2";
            this.status2.Size = new System.Drawing.Size(0, 21);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe Print", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(14, 21);
            this.toolStripStatusLabel1.Text = "|";
            this.toolStripStatusLabel1.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // create_strat_menu
            // 
            this.create_strat_menu.Name = "create_strat_menu";
            this.create_strat_menu.Size = new System.Drawing.Size(152, 22);
            this.create_strat_menu.Text = "Créer...";
            this.create_strat_menu.Click += new System.EventHandler(this.create_strat_menu_Click);
            // 
            // Display
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 366);
            this.Controls.Add(this.export_box);
            this.Controls.Add(this.bench_box);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.statsContainer);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Display";
            this.Text = "Outil de gestion de portefeuille";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statsContainer.ResumeLayout(false);
            this.stats.ResumeLayout(false);
            this.stats.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.bench_box.ResumeLayout(false);
            this.export_box.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel status1;
        private System.Windows.Forms.Button clickChoosePortfolio;
        private System.Windows.Forms.OpenFileDialog choosePortfolio;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton file;
        private System.Windows.Forms.ToolStripMenuItem listPortfolio;
        private System.Windows.Forms.ToolStripMenuItem addPortfolio;
        private System.Windows.Forms.GroupBox statsContainer;
        private System.Windows.Forms.TextBox last_price_value;
        private System.Windows.Forms.Label last_price;
        private System.Windows.Forms.TextBox sharpe_value;
        private System.Windows.Forms.Label sharpe;
        private System.Windows.Forms.TextBox var_param_value;
        private System.Windows.Forms.Label var_param;
        private System.Windows.Forms.TextBox total_perf_value;
        private System.Windows.Forms.Label total_perf;
        private System.Windows.Forms.TextBox volat_annu_value;
        private System.Windows.Forms.Label volat_annu;
        private System.Windows.Forms.TextBox volat_value;
        private System.Windows.Forms.Label volat;
        private System.Windows.Forms.TableLayoutPanel stats;
        private System.Windows.Forms.Label pct_breach;
        private System.Windows.Forms.TextBox pct_breach_value;
        private System.Windows.Forms.TextBox bench_value;
        private System.Windows.Forms.Label bench;
        private System.Windows.Forms.TextBox beta_value;
        private System.Windows.Forms.Label beta;
        private System.Windows.Forms.TextBox track_value;
        private System.Windows.Forms.Label track;
        private System.Windows.Forms.Label period;
        private System.Windows.Forms.TextBox period_value;
        private System.Windows.Forms.ToolStripMenuItem listAsset;
        private System.Windows.Forms.ToolStripMenuItem addAsset;
        private System.Windows.Forms.OpenFileDialog chooseAsset;
        private System.Windows.Forms.Button buildPortfolio;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button createStrategy;
        private System.Windows.Forms.ToolStripMenuItem listStrategy;
        private System.Windows.Forms.GroupBox bench_box;
        private System.Windows.Forms.Button add_bench;
        private System.Windows.Forms.OpenFileDialog chooseBenchmark;
        private System.Windows.Forms.GroupBox export_box;
        private System.Windows.Forms.Button backtest_export;
        private System.Windows.Forms.Label initial_cash;
        private System.Windows.Forms.TextBox initial_cash_value;
        private System.Windows.Forms.Button compo_export;
        private System.Windows.Forms.Button optimize;
        private System.Windows.Forms.ToolStripMenuItem quit;
        private System.Windows.Forms.ToolStripStatusLabel status2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem create_strat_menu;
    }
}

