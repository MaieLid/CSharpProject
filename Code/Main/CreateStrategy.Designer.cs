namespace Main
{
    partial class CreateStrategy
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ticker_value = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.assets_box = new System.Windows.Forms.ComboBox();
            this.futures_box = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.type_box = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.hedge_box = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.invest_value = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cancel_button = new System.Windows.Forms.Button();
            this.validate_button = new System.Windows.Forms.Button();
            this.roll_amount = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.roll_period = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.start_date = new System.Windows.Forms.ComboBox();
            this.end_date = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Choisir les paramètres, puis valider.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(62, 42);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Donner un nom à la stratégie";
            // 
            // ticker_value
            // 
            this.ticker_value.Location = new System.Drawing.Point(66, 66);
            this.ticker_value.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ticker_value.Name = "ticker_value";
            this.ticker_value.Size = new System.Drawing.Size(148, 26);
            this.ticker_value.TabIndex = 4;
            this.ticker_value.TextChanged += new System.EventHandler(this.ticker_value_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(61, 108);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Portefeuille d\'actifs";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(61, 177);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(164, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Portefeuille de futures";
            // 
            // assets_box
            // 
            this.assets_box.FormattingEnabled = true;
            this.assets_box.Location = new System.Drawing.Point(65, 133);
            this.assets_box.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.assets_box.Name = "assets_box";
            this.assets_box.Size = new System.Drawing.Size(180, 28);
            this.assets_box.TabIndex = 9;
            this.assets_box.SelectedIndexChanged += new System.EventHandler(this.assets_box_SelectedIndexChanged);
            // 
            // futures_box
            // 
            this.futures_box.FormattingEnabled = true;
            this.futures_box.Location = new System.Drawing.Point(65, 202);
            this.futures_box.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.futures_box.Name = "futures_box";
            this.futures_box.Size = new System.Drawing.Size(180, 28);
            this.futures_box.TabIndex = 10;
            this.futures_box.SelectedIndexChanged += new System.EventHandler(this.futures_box_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(345, 42);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Type";
            // 
            // type_box
            // 
            this.type_box.FormattingEnabled = true;
            this.type_box.Items.AddRange(new object[] {
            "Low Volatility",
            "High Volatility",
            "Low Beta",
            "High Beta",
            "Equal Weights"});
            this.type_box.Location = new System.Drawing.Point(350, 66);
            this.type_box.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.type_box.Name = "type_box";
            this.type_box.Size = new System.Drawing.Size(180, 28);
            this.type_box.TabIndex = 12;
            this.type_box.SelectedIndexChanged += new System.EventHandler(this.type_box_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(344, 108);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 20);
            this.label6.TabIndex = 13;
            this.label6.Text = "Hedge";
            // 
            // hedge_box
            // 
            this.hedge_box.FormattingEnabled = true;
            this.hedge_box.Items.AddRange(new object[] {
            "None",
            "Beta",
            "Amount"});
            this.hedge_box.Location = new System.Drawing.Point(349, 133);
            this.hedge_box.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.hedge_box.Name = "hedge_box";
            this.hedge_box.Size = new System.Drawing.Size(180, 28);
            this.hedge_box.TabIndex = 14;
            this.hedge_box.SelectedIndexChanged += new System.EventHandler(this.hedge_box_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(344, 177);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(155, 20);
            this.label7.TabIndex = 15;
            this.label7.Text = "Investissement initial";
            // 
            // invest_value
            // 
            this.invest_value.Location = new System.Drawing.Point(349, 202);
            this.invest_value.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.invest_value.Name = "invest_value";
            this.invest_value.Size = new System.Drawing.Size(148, 26);
            this.invest_value.TabIndex = 16;
            this.invest_value.TextChanged += new System.EventHandler(this.invest_value_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(62, 317);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 20);
            this.label8.TabIndex = 17;
            this.label8.Text = "Date de début";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(345, 317);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 20);
            this.label9.TabIndex = 19;
            this.label9.Text = "Date de fin";
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(366, 391);
            this.cancel_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(112, 35);
            this.cancel_button.TabIndex = 22;
            this.cancel_button.Text = "Annuler";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // validate_button
            // 
            this.validate_button.Enabled = false;
            this.validate_button.Location = new System.Drawing.Point(508, 391);
            this.validate_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.validate_button.Name = "validate_button";
            this.validate_button.Size = new System.Drawing.Size(112, 35);
            this.validate_button.TabIndex = 21;
            this.validate_button.Text = "Valider";
            this.validate_button.UseVisualStyleBackColor = true;
            this.validate_button.Click += new System.EventHandler(this.validate_button_Click);
            // 
            // roll_amount
            // 
            this.roll_amount.Location = new System.Drawing.Point(66, 272);
            this.roll_amount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.roll_amount.Name = "roll_amount";
            this.roll_amount.Size = new System.Drawing.Size(148, 26);
            this.roll_amount.TabIndex = 26;
            this.roll_amount.TextChanged += new System.EventHandler(this.roll_amount_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(344, 247);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(160, 20);
            this.label10.TabIndex = 25;
            this.label10.Text = "Période de roulement";
            // 
            // roll_period
            // 
            this.roll_period.FormattingEnabled = true;
            this.roll_period.Items.AddRange(new object[] {
            "Semaine",
            "Mois"});
            this.roll_period.Location = new System.Drawing.Point(350, 272);
            this.roll_period.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.roll_period.Name = "roll_period";
            this.roll_period.Size = new System.Drawing.Size(180, 28);
            this.roll_period.TabIndex = 24;
            this.roll_period.SelectedIndexChanged += new System.EventHandler(this.roll_period_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(61, 247);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(170, 20);
            this.label11.TabIndex = 23;
            this.label11.Text = "Nombre de roulements";
            // 
            // start_date
            // 
            this.start_date.FormattingEnabled = true;
            this.start_date.Location = new System.Drawing.Point(66, 342);
            this.start_date.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.start_date.Name = "start_date";
            this.start_date.Size = new System.Drawing.Size(180, 28);
            this.start_date.TabIndex = 27;
            // 
            // end_date
            // 
            this.end_date.FormattingEnabled = true;
            this.end_date.Location = new System.Drawing.Point(349, 342);
            this.end_date.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.end_date.Name = "end_date";
            this.end_date.Size = new System.Drawing.Size(180, 28);
            this.end_date.TabIndex = 28;
            // 
            // CreateStrategy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 438);
            this.Controls.Add(this.end_date);
            this.Controls.Add(this.start_date);
            this.Controls.Add(this.roll_amount);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.roll_period);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.validate_button);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.invest_value);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.hedge_box);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.type_box);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.futures_box);
            this.Controls.Add(this.assets_box);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ticker_value);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "CreateStrategy";
            this.Text = "Créer une stratégie";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ticker_value;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox assets_box;
        private System.Windows.Forms.ComboBox futures_box;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox type_box;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox hedge_box;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox invest_value;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Button validate_button;
        private System.Windows.Forms.TextBox roll_amount;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox roll_period;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox start_date;
        private System.Windows.Forms.ComboBox end_date;
    }
}
