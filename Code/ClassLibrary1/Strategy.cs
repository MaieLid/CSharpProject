using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


namespace ClassLibrary1
{

    public class Strategy : Instrument
    {
        private double initial_invest;
        private string strategy_Type;
        private string strategy_Hedge;
        private Portfolio futures;
        private Portfolio assets;

        private Matrix backTest_strat;

        private Matrix prices_Futassets;
        private Matrix prices_Benchassets;
        private Matrix returns_Futassets;
        private Matrix returns_Benchassets;
        private Matrix quantities;
        private Matrix dividends;
        private Matrix movements;


        private int roll_nb;
        private double roll_Mult;
        private int roll_Amount;
        private int roll_Period;

        private Matrix volatilities;
        private Matrix invVolatilities;
        private Matrix betas;
        private Matrix invBetas;
        private Matrix vEW;

        public Strategy(string ticker, Portfolio assets, Portfolio futures, DateTime dateSt, DateTime dateEnd, double invest, string type, string hedge, int roll_amount, String sroll_period)
            : base(string.Concat(ticker, " - ", type, " - ", hedge))
        {
            this.futures = futures;
            this.assets = assets;
            Date_Beg_i = assets.Get_Index_Date(dateSt);
            Date_End_i = assets.Get_Index_Date(dateEnd);
            this.dates = new DateTime[Date_End_i - Date_Beg_i + 1];
            Array.Copy(assets.Dates, Date_Beg_i, this.dates, 0, Date_End_i - Date_Beg_i + 1);

            initial_invest = invest;
            strategy_Type = type;
            strategy_Hedge = hedge;

            prices_Futassets = Portfolio.Merge_Portfolio_Options(assets, futures, "Prices");
            returns_Futassets = Portfolio.Merge_Portfolio_Options(assets, futures, "Returns");

            prices_Benchassets = Portfolio.Merge_Portfolio_Options(assets, assets.Bench, "Prices");
            returns_Benchassets = Portfolio.Merge_Portfolio_Options(assets, assets.Bench, "Returns");
            int jBench = assets.NbAsset;

            this.roll_Amount = roll_amount;

            if (sroll_period == "Semaine")
            {
                roll_Period = 5;

            }
            else if (sroll_period == "Mois")
            {
                roll_Period = 21;
            }
            else
            {
                throw new NotSupportedException("La période de roll doit être semaine ou mois");
            }
            roll_Mult = roll_Period * roll_Amount; // Multiplicateur

            roll_nb = (int)((Date_End_i - Date_Beg_i) / roll_Mult + 1); // Nombre de rolls

            volatilities = new Matrix(roll_nb, assets.NbAsset);
            invVolatilities = new Matrix(roll_nb, assets.NbAsset);
            betas = new Matrix(roll_nb, assets.NbAsset);
            invBetas = new Matrix(roll_nb, assets.NbAsset);
            vEW = new Matrix(roll_nb, assets.NbAsset);

            //  Compteurs des indices des lignes
            int iDateTempStart = Date_Beg_i - (int)roll_Mult;
            int iDateTempEnd = Date_Beg_i + roll_Amount;

            // Itération sur chaque roll on calcule la volatilité des actifs et leur beta avec le benchmark 
            for (int i = 0; i < roll_nb; i++)
            {
                for (int j = 0; j < assets.NbAsset; j++)
                {
                    volatilities[i, j] = returns_Benchassets.Row(iDateTempStart, iDateTempEnd - iDateTempStart + 1).Column(j).StdDev();
                    invVolatilities[i, j] = 1 / volatilities[i, j];
                    betas[i, j] = returns_Benchassets.Row(iDateTempStart, iDateTempEnd - iDateTempStart + 1).Column(j).Cov(returns_Benchassets.Row(iDateTempStart, iDateTempEnd - iDateTempStart + 1).Column(jBench));
                    invBetas[i, j] = 1 / betas[i, j];
                    vEW[i, j] = 1;
                }
                iDateTempStart = iDateTempEnd;
                iDateTempEnd = (int)Math.Min(iDateTempStart + roll_Mult, Date_End_i);
            }

            if (strategy_Hedge == "None")
            {
                Strategy_NoHedge();
            }
            else
            {
                Strategy_WithHedge();
            }
            this.Bench_prices = new Column(assets.Bench.Prices.Row(Date_Beg_i, Date_End_i - Date_Beg_i + 1));
            this.Bench_returns = this.Bench_prices.CalculateReturns("variation");
            NAV_compute();
            ComputeVaRs();
        }

        #region Accesseurs
        public int Date_End_i { get; }
        public int Date_Beg_i { get; }
        public double Initial_invest => initial_invest;
        public Portfolio Futures => futures;
        public Portfolio Assets => assets;
        public Column Bench_prices { get; }
        public Column Bench_returns { get; }
        public Benchmark Bench { get; }
        public double Alpha => Stats.SimpleLinearRegression(this.returns, this.Bench_returns).Item1;
        public double Beta => Stats.SimpleLinearRegression(this.returns, this.Bench_returns).Item2;
        public double Tracking_error => this.returns.DiffMat(this.Bench_returns).StdDev();

        #endregion

        private void Strategy_NoHedge()
        {

            int id1=0;
            int id3=0;
            Matrix VarStrat;
            Double VartStrat_RowSum;

            quantities = new Matrix(assets.NbDates, assets.NbAsset + futures.NbAsset); 

            switch (strategy_Type)
            {
                case "Low Volatility":
                    VarStrat = new Matrix(invVolatilities);
                    break;
                case "High Volatility":
                    VarStrat = new Matrix(volatilities);
                    break;
                case "Low Beta":
                    VarStrat = new Matrix (invBetas);
                    break;
                case "High Beta":
                    VarStrat =new Matrix(betas);
                    break;
                case "Equal Weights":
                    VarStrat = new Matrix(vEW);
                    break;
                default:
                    throw new ArgumentException("Type should be one of authorized String");
            }


            for (int i =0; i < roll_nb; i++)
            {

                VartStrat_RowSum = VarStrat.Row(i).Sum();

                for (int j=0; j < assets.NbAsset; j++)
                {
                
                    if (i == 0) { quantities[Date_Beg_i+id1, j] = VarStrat[i, j]/VartStrat_RowSum*initial_invest/prices_Futassets[Date_Beg_i+id1, j]; }

                    else
                    {
                        // On détermine la nouvelle valeur de l'investissement selon les quantités achetées au dernier roll et les prix au roll actuel
                        initial_invest = 0;

                        for(int k=0; k < assets.NbAsset; k++) { initial_invest += quantities[Date_Beg_i + id3, k] * prices_Futassets[Date_Beg_i + id1 - 1, k]; }

                        // On détermine la quantité à acheter au nouveau roll avec les prix actuels et la nouvelle valeur du portefeuille
                        quantities[Date_Beg_i + id1, j] = VarStrat[i, j] / VartStrat_RowSum * initial_invest / prices_Futassets[Date_Beg_i + id1, j];

                    }
                }

                id3 = id1;
                id1 = (int)(Math.Min(id1 + roll_Mult, Date_End_i - Date_Beg_i+1)); //TODO

                for (int j= Date_Beg_i+id3+1; j<Date_Beg_i+id1; j++)
                {
                    for (int k=0; k < assets.NbAsset; k++)
                    {
                        quantities[j, k] = quantities[j - 1, k];
                    }
                }
            }

        }

        private void Strategy_WithHedge()
        {

            int id1 = 0;
            int id3 = 0;
            int jFutPrev = 0;
            int jFut=-1;

            Double Beta_Pf;
            Matrix VarStrat;
            Double VartStrat_RowSum;
            quantities= new Matrix(assets.NbDates, assets.NbAsset + futures.NbAsset);

            switch (strategy_Type)
            {
                case "Low Volatility":
                    VarStrat = new Matrix (invVolatilities);
                    break;
                case "High Volatility":
                    VarStrat =new Matrix(volatilities);
                    break;
                case "Low Beta":
                    VarStrat = new Matrix(invBetas);
                    break;
                case "High Beta":
                    VarStrat = new Matrix(betas);
                    break;
                case "Equal Weights":
                    VarStrat = new Matrix(vEW);
                    break;
                default:
                    throw new ArgumentException("Type should be one of authorized String");
            }


            for (int i = 0; i < roll_nb; i++)
            {

                VartStrat_RowSum = VarStrat.Row(i).Sum();


                //Trouver le futur qui correspond à la période de roll
                for (int j = assets.NbAsset; j< assets.NbAsset + futures.NbAsset;j++)
                {
                    if (Date_Beg_i+id1+roll_Mult<= Date_End_i)
                    {

                        if (prices_Futassets[Date_Beg_i + id1, j] != 0 & prices_Futassets[Date_Beg_i + id1 + (int)roll_Mult, j] != 0) 

                        {
                            jFut = j;
                        }
                    }

                }

                if (jFut == -1) { throw new NotSupportedException("No future found"); }


                for (int j = 0; j < assets.NbAsset; j++)
                {

                    if (i == 0) { quantities[Date_Beg_i + id1, j] = VarStrat[i, j] / VartStrat_RowSum * initial_invest / prices_Futassets[Date_Beg_i + id1, j]; }
                    else
                    {
                        // On détermine la nouvelle valeur de l'investissement selon les quantités achetées au dernier roll et les prix au roll actuel
                        initial_invest = 0;
                        for (int k = 0; k < assets.NbAsset; k++) { initial_invest += quantities[Date_Beg_i + id3, k] * prices_Futassets[Date_Beg_i + id1 - 1, k]; }


                        //Ajout des appels de marge du future
                        if (jFutPrev != -1)
                        {
                            initial_invest += quantities[Date_Beg_i + id3, jFutPrev] * (prices_Futassets[Date_Beg_i + id3, jFutPrev] - prices_Futassets[Date_Beg_i + id1 - 1, jFutPrev]);
                        }


                        // On détermine la quantité à acheter au nouveau roll avec les prix actuels et la nouvelle valeur du portefeuille
                        quantities[Date_Beg_i + id1, j] = VarStrat[i, j] / VartStrat_RowSum * initial_invest / prices_Futassets[Date_Beg_i + id1, j];

                    }
                }




                Beta_Pf = 0;
                //Déterminer le Beta du portefeuille


                /// SUPPRIMER LA BOUCLE SI TEST VERIFIE 
                for(int j = 0; j <assets.NbAsset; j++)

                {
                    Beta_Pf += VarStrat[i, j] / VartStrat_RowSum * betas[i, j];
                }

                /// NEED TO CHECK THE VALUE IF IT S THE SAMMEEEEEEEE
                Beta_Pf = (VarStrat.Row(i)*betas.Row(i)*(1/VartStrat_RowSum)).Sum();



                if (jFutPrev != -1)
                {
                    quantities[Date_Beg_i + id1, jFutPrev] = 0;

                }
                if (strategy_Hedge == "Beta")
                {
                    if (prices_Futassets[Date_Beg_i + id1, jFut] > 0) { quantities[Date_Beg_i + id1, jFut] = initial_invest * Beta_Pf / prices_Futassets[Date_Beg_i + id1, jFut]; }
                    else { quantities[Date_Beg_i + id1, jFut] = 0; }
                }
                else if (strategy_Hedge == "Amount") {
                    if (prices_Futassets[Date_Beg_i + id1, jFut] > 0) { quantities[Date_Beg_i + id1, jFut] = initial_invest / prices_Futassets[Date_Beg_i + id1, jFut]; }

                    else { quantities[Date_Beg_i + id1, jFut] = 0; }
                }
                else
                { throw new ArgumentException("Hedge should be Amount, Beta or None"); }

                jFutPrev = jFut;


                ///

                id3 = id1;
                id1 = (int)(Math.Min(id1 + roll_Mult, Date_End_i - Date_Beg_i+1));// TODO

                for (int j = Date_Beg_i + id3 + 1; j < Date_Beg_i + id1; j++)
                {
                    for (int k = 0; k < assets.NbAsset + futures.NbAsset; k++)
                    {
                        quantities[j, k] = quantities[j - 1, k];
                    }
                }
            }

            quantities = new Matrix(quantities);
        }

        private void NAV_compute()
        {
            double div;
            double SousRac;
            double Val;

            int n;

            dividends = new Matrix(assets.NbDates,assets.NbAsset + futures.NbAsset);
            backTest_strat = new Matrix(Date_End_i - Date_Beg_i + 1, 2,0);
            movements = new Matrix(assets.NbDates,assets.NbAsset + futures.NbAsset, 0);
            
            //Remplissage des Mouvements
            for (int i = 1; i < quantities.Height; i++)
            {
                for(int j = 0; j < quantities.Width; j++)
                {
                    movements[i, j] = quantities[i,j]-quantities[i-1,j];
                }
            }
            //Remplissage des Dividendes
            for (int i=0; i< quantities.Height; i++)
            {
                for (int j= assets.NbAsset; j< quantities.Width;j++)
                {
                    if (i == 0) { dividends[i, j] = 0; }
                    else if (i < quantities.Height - 1)
                    {
                        if(quantities[i-1,j]==0 || quantities[i, j] == 0) { dividends[i, j] = 0; }
                        else
                        {
                            if(movements[i-1,j-1] != 0 || i== Date_Beg_i + 1)
                            {
                                dividends[i, j] = -(quantities[i, j] * prices_Futassets[i, j] - prices_Futassets[i - 1, j] * quantities[i - 1, j]);
                            }
                            else
                            {
                                dividends[i, j] = -(quantities[i, j] * prices_Futassets[i, j] - prices_Futassets[i - 1, j] * quantities[i - 1, j])+movements[i-1,j];
                            }
                        }
                    }
                    else
                    {
                        if (quantities[i - 1, j] == 0 || i==Date_End_i+1) { dividends[i, j] = 0; }

                        else
                        {
                            if (movements[i - 1, j - 1] != 0 || i == Date_Beg_i + 1)
                            {
                                dividends[i, j] = -(quantities[i, j] * prices_Futassets[i, j] - prices_Futassets[i - 1, j] * quantities[i - 1, j]);
                            }
                            else
                            {
                                dividends[i, j] = -(quantities[i, j] * prices_Futassets[i, j] - prices_Futassets[i - 1, j] * quantities[i - 1, j]) + movements[i - 1, j];
                            }
                        }
                        if(quantities[i,j-1]==0 && quantities[i-1,j]==0 && quantities[i,j] != 0 && i != Date_End_i && i!= Date_Beg_i)
                        {
                            dividends[i, j] = -(quantities[i, j] * prices_Futassets[i, j] - prices_Futassets[i - 1, j-1] * quantities[i - 1, j-1]);
                        }
                    }
                }
            }
            
            div = 0;
            n = 0;
            for (int i = Date_Beg_i; i < Date_End_i+1; i++)
            { 
                Val = (quantities.Row(i) * prices_Futassets.Row(i)).Column(0,assets.NbAsset).Sum();
                SousRac =(movements.Row(i) * prices_Futassets.Row(i)).Column(0,assets.NbAsset).Sum();

                div = -dividends.Row(i).Sum();
                Val += div;

                if (i == Date_Beg_i)
                {
                    backTest_strat[n, 0] = Val / 100;
                    backTest_strat[n, 1] = 100;
                }
                else
                {
                    backTest_strat[n, 0] = backTest_strat[n - 1, 0] + SousRac * backTest_strat[n - 1, 0] / Val;
                    backTest_strat[n, 1] = (Val + SousRac) / backTest_strat[n, 0];
                }
                Debug.WriteLine(backTest_strat[n, 1]);
                n += 1;
            }

            this.prices = backTest_strat.Column(1);
            this.SetReturns("variation");
        }
    }
}








