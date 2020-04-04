using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using Microsoft.Office.Interop.Excel;

namespace ClassLibrary1
{
    public static class Backtest
    {
        public static void GetVolat()
        {
            throw new System.NotImplementedException();
        }

        public static void SetRiskFree()
        {
            throw new System.NotImplementedException();
        }

        // Méthode retournant un quantile d'un vecteur
        // Inputs : 
        // - returnArray : vecteur des rendements (array) déjà triée
        // - niveau : niveau du quantile
        // Output : quantile
        private static double Quantile(double[] returnArray, double niveau) //Calcul du quantile
        {
            // Cas où on demande un quantile supérieur à 1
            if (niveau >= 1.0d) return returnArray[returnArray.Length - 1];

            double pos = (returnArray.Length + 1) * niveau; // Position dans returnsArray

            // variables dans lesquelles on va stocker les plus proches valeurs de celles du quantile cherché 
            double gauche = 0.0d, droite = 0.0d;

            double n = niveau * (returnArray.Length - 1) + 1.0d;

            // Trouver les quantiles
            // Floor : Retourne la valeur du plus grand entier inférieur ou égal au nombre à virgule flottante double précision spécifiée
            if (pos >= 1) // Si la pos de l'éélement cherché est supérieur à 1 (pour les grandes listes)
            {
                gauche = returnArray[(int)Math.Floor(n) - 1];
                droite = returnArray[(int)Math.Floor(n)];
            }
            else // Si la liste contient peu d'élements, la valeur cherchée sera forcément entre la 1ère et 2° valeur du vecteur
            {
                gauche = returnArray[0];
                droite = returnArray[1];
            }

            // Après avoir stocké les valeurs deux valeurs les plus proches (au dessus et en dessous) du quantile
            if (gauche == droite)
            {
                return gauche; // Les deux valeurs sont identiques dont retourne gauche comem résultat du quantile 
            }
            else
            {
                double part = n - Math.Floor(n);
                return gauche + part * (droite - gauche); // Calcul pour retourner le quantile 
            }

        }

        // Méthode effectuant une VaR Historique 
        // Input : 
        // - returns : vecteur des rendements historiques
        // - horizon : horizon de la VaR
        // - niveau_confiance : niveau de confiance voulu pour la VaR
        // Output :
        // - valeur de la VaR Historique
        public static double VaR_Historique(Column returns, int horizon = 252, double niveau_confiance = 0.95, int obsolete_param = -1)
        {
            // Check s'il y a assez de données compte tenu de l'horizon choisi
            if (returns.Length < horizon)
            {
                throw new ArgumentException(string.Format("Pas assez de data pour calculer la VaR Historique = {0}.", horizon));
            }

            // Conversion to array
            double[] returnArray = returns.ArrayFrom1D();

            // Tri des croissant des rendements
            Array.Sort(returnArray);

            // Utilisation de la méthode Quantile pour retourner le quantile voulu en fonction du niveau de confiance
            // Mutiplication par la racine de l'horizon (si horizon = 252, c'est annualisé)

            return Quantile(returnArray, 1 - niveau_confiance) * Math.Sqrt(horizon);

        }

        // Méthode effectuant une VaR Paramétrique 
        // Input : 
        // - returns : vecteur des rendements historiques
        // - horizon : horizon de la VaR
        // - niveau_confiance : niveau de confiance voulu pour la VaR
        // Output :
        // - valeur de la VaR Paramétrique
        public static double VaR_Parametrique(Column returns, int horizon = 252, double niveau_confiance = 0.95, int obsolete_param = -1)
        {
            double std = returns.StdDev(); // Ecart-type des rendements historiques

            // Objet pour appeler les méthodes de la classe Stats
            double inv = Stats.InverseNormalDistribution(niveau_confiance); // Inverse de la fonction de répartition de la loi normale 
            return -(std * inv * Math.Sqrt(horizon)); // Annualisation de la VaR si horizon = 252
        }

        // Méthode effectuant une VaR MonteCarlo 
        // Input : 
        // - returns : vecteur des rendements historiques
        // - horizon : horizon de la VaR
        // - niveau_confiance : niveau de confiance voulu pour la VaR
        // - nbSimuls : nombre de simulations
        // Output :
        // - valeur de la VaR MonteCarlo
        // Mixte entre la méthode historique et paramétrique 
        public static double VaR_MonteCarlo(Column returns, int horizon = 252, double niveau_confiance = 0.95, int nbSimuls = 1000)
        {
            double[] x = new double[nbSimuls]; // Vecteur contenant des nombres aléatoire entre 0 et 1. Utilisé pour avoir l'inverse de la fonction de répartition de la loi normale 

            Random random = new Random();
            for (int j = 0; j < nbSimuls; j++)
            {
                x[j] = random.NextDouble(); // Nombre aléatoire entre 0 et 1
            }

            double std = returns.StdDev(); // Ecart-type des rendements historiques 
            double[] simuls = new double[nbSimuls]; // stockage des différentes simulations 
            for (int i = 0; i < nbSimuls; i++)
            {
                simuls[i] = Math.Exp(std * Stats.InverseNormalDistribution(x[i])) - 1; // Inverse de la fonction de répartition de la loi normale
            }
            Array.Sort(simuls); // Tri croissant

            return -Quantile(simuls, 1 - niveau_confiance) * Math.Sqrt(horizon); //calcul de la VaR (annualisée si horizon = 252)

        }

        // Méthode effectuant le Backtest de la VaR
        // Inputs : 
        // - returns : vecteur des rendements historiques
        // - VaRFunction : méthode VaR à backtester
        // - horizon : horizon de la VaR
        // - niveau_confiance : niveau de confiance voulu
        // - nbSimuls : nombre de simulation (VaR MonteCarlo)
        // Output :
        // - booléen indiquant si la VaR reussi ou non le backtest
        // - double : pourcentage de breach
        public static (double, double) BacktestVaR(Func<Column, int, double, int, double> VaRFunction, Column returns, int horizon = 252, double niveau_confiance = 0.95, int nbSimuls = 1000)
        {
            if (returns.Length < horizon)
                throw new IndexOutOfRangeException("Impossible de backtester, l'horizon est supérieur au nombre de dates disponibles sur la période.");

            double[] vaR = new double[returns.Length]; // Vecteur contenant les VaR
            long n = returns.Length - horizon; // Nombre de périodes pour lesquelles les VaR sont calculées
            long nbBreach = 0; // Compteur de breach: si la VaR a été dépassée ou non
            double pctBreach = 0.0; // Pourcentage breach par rapport au total des VaR calculées

            // Initialisation valeurs de la variable qui stock les VaR glissantes
            // Il ne peut pas y avoir de valeurs pour des dates t, où la période qui précède est inférieur à l'hozrizon
            // Par exemple à t = 200, on ne peut pas avoir de VaR à 252 jours (1 an)
            // On met des NaN par défaut
            for (int j = 0; j < horizon; j++)
            {
                vaR[j] = double.NaN;
            }

            // Calcul des VaR pour des fenêtres roulantes
            // C'est-à-dire : pour chaque date t = horizon +1 jusqu'à la fin du vecteur, on calcule des VaR pour un horizon donné (par défaut 252) sur la période précédente
            // Donc, pour horizon = 252, on calcule la VaR à t = 253 sur les 252 derniers jours, puis pour t = 254, on calcule aussi sur les 252 jours précédents, et ainsi de suite
            // On obtient un vecteur vaR[] composé de NaN pour t < horizon puis de VaR à partir de t = horizon
            // Les résultats sont ensuite reporté à l'horizon (annualisés si horizon = 252)
            for (int i = horizon; i < returns.Length; i++)
            {
                vaR[i] = VaRFunction(returns.Row(i - horizon, horizon), horizon, niveau_confiance, nbSimuls) / Math.Sqrt(horizon);
            }

            // Calcul des fois où la VaR > rdt
            for (int i = 0; i < n; i++)
            {
                if (returns[i + horizon] < vaR[i + horizon])
                {
                    nbBreach += 1;
                }
            }
            pctBreach = (double)nbBreach / n;

            //Binomial test : pour savoir si oui ou non la VaR dont on fait le backtest passe ce test
            double test = Stats.BinomialTest(nbBreach, n, 1 - niveau_confiance);

            return (pctBreach, test);
        }

        // Méthode affichant sur Excel la composition d'un portefeuille
        // (liste d'actifs et de leurs poids)
        public static void DrawPortfolioCompo(Instrument instrument)
        {

            ExcelApplication app = new ExcelApplication(true); // Applications dans Excel

            // Report dans Excel des prix et des returns du portefeuille, tickers et dates
            if (instrument is Portfolio)
            {
                Portfolio p = (Portfolio)instrument;
                app.WriteCompo(p.Weights, p.GetInstrumentsTickers(), 1);
            }
            else if (instrument is Strategy)
            {
                Strategy s = (Strategy)instrument;

                app.WriteCompo(s.Assets.Weights, s.Assets.GetInstrumentsTickers(), 1);
                app.ChangeSheetName(1, "Portefeuille d'actifs");
                app.WriteCompo(s.Futures.Weights, s.Futures.GetInstrumentsTickers(), 2);
                app.ChangeSheetName(2, "Portefeuille de futures");
            }
            else if (instrument is Asset)
            {
                app.WriteCompo(new Column(1, 1), new string[] { instrument.Ticker }, 1);
            }

            app.Save(instrument.Ticker + "_composition");
        }

        // Méthode affichant sur Excel la valeur du portefeuille ainsi que celle du benchmark depuis le début des données disponibles
        // Affichage d'un graphique pour observer l'évolution entre les deux 
        // En base 100 pour les premières valeurs 
        public static void DrawPortfolioValue(Instrument myPortfolio)
        {
            ExcelApplication app = new ExcelApplication(true); // Applications dans Excel

            // Report dans Excel des prix et des returns du portefeuille, tickers et dates
            app.Write(myPortfolio.Prices, myPortfolio.Ticker, myPortfolio.Dates, 2);
            app.ChangeSheetName(2, "Prix");
            app.Write(myPortfolio.Returns, myPortfolio.Ticker, myPortfolio.Dates, 3);

            app.AppendVaR("VaR Paramétrique", myPortfolio.Var_series["VaR Paramétrique"].Daily, 3);
            app.ChangeSheetName(3, "Rendements + VaR");

            // Afficher les graphs de Prices et returns
            app.LineChart(2, "Prix");
            app.LineChart(3, "Rendements");

            // On affiche la valeur du portefeuille et du benchmark puis on trace les graphiques :
            // - Soit sans la stratégie d'allocation
            // - Soit avec la stratégie d'allocation 
            // Rebase en base 100
            if (myPortfolio is Portfolio)
            {
                Portfolio myPort = (Portfolio)myPortfolio;
                if (myPort.Bench != null) // Il faut qu'il y ait un benchmark 
                {
                    Matrix compare = new Matrix(new Matrix1D[] { myPort.Prices.Rebase(), myPort.Bench.Prices.Rebase() });
                    app.Write(compare, new string[] { myPort.Ticker, myPort.Bench.Ticker }, myPort.Bench.Dates, 4);
                    app.LineChart(4, "Prix rebasés vs Prix du bench rebasés");
                }
            }
            else if (myPortfolio is Strategy)
            {
                Strategy myStrat = (Strategy)myPortfolio;
                if (myStrat.Assets.Bench != null)
                {
                    Matrix compare = new Matrix(new Matrix1D[] { myStrat.Prices.Rebase(), myStrat.Bench_prices.Rebase() });
                    app.Write(compare, new string[] { myStrat.Ticker, myStrat.Assets.Bench.Ticker }, myStrat.Dates, 4);
                    app.LineChart(4, "Prix rebasés vs Prix du bench rebasés");
                }
            }

            app.Save(myPortfolio.Ticker + "_backtest");
        }

        // Méthode permettant de suivre l'évolution des returns (ou autre) en partant d'une base 100 (ou autre) avec la première donnée disponible
        // Inputs : 
        // - returns : Matrice des rendements (peut prendre plusieurs colonnes de rendements)
        // - start_base : base sur laquelle on part (généralement 100 ou 100)
        // Output : Matrix avec l'évolution des returns par rapport à la base (cumulé)
        public static Matrix Base(Matrix returns, int start_base = 100)
        {
            Matrix res = new Matrix(new double[returns.Height + 1, returns.Width]);

            for (int i = 0; i < returns.Width; i++)
            {
                res[0, i] = 1;

                for (int j = 0; j < returns.Height; j++)
                {
                    res[j + 1, i] = (1 + returns[j, i]);
                }
            }

            res = res.CumProd();

            for (int i = 0; i < res.Width; i++)
            {
                for (int j = 0; j < res.Height; j++)
                {
                    res[j, i] = res[j, i] * 100;
                }
            }
            
            return res;
        }

        // Méthode calculant l'excess performance du portfeuille par rapport au benchmark en base 100
        // Inputs : 
        // - returnsPtf : rendements historiques du portefeuille
        // - returnsBench : rednements historiques du benchmark
        // Output : 
        // - vecteur des résultats

        public static Matrix ExcessPerf(Column returnsPtf, Column returnsBench)
        {

            // On les met en base 100
            Matrix A = Base(returnsPtf);
            Matrix B = Base(returnsBench);

            // Calcul des log returns de cette nouvelle série de returns (qui est en base = 100)

            Column returnsA = new Column(A.Height);
            Column returnsB = new Column(B.Height);

            returnsA[0] = 0;
            returnsB[0] = 0;
            for (int i = 1; i < A.Length; ++i)
            {
                returnsA[i] = Math.Log(A[i,0] / A[i - 1,0]);
                returnsB[i] = Math.Log(B[i,0] / B[i - 1,0]);
            }

            //Ecart relatif 
            Column C = returnsA.DiffMat(returnsB);

            // Repassage en base 100
            Matrix res = Base(C);
            res = res.Row(1, res.Height-1);
            return res;
        }

        // Méthode effectuant un test de Jarque-Bera : voir si la serie suit ou non une loi normale
        // Input : vecteur de rendements historiques
        // Output : booleen (True : suit une loi normale, False : non)
        public static bool JBTest(Matrix returns)
        {
            double S = returns.Skewness()[0]; // Calcul du skewness
            double K = returns.Kurtosis()[0]; // Calcul du kurtosis
            double JB = 0.0;

            JB = (returns.Height / 6) * (S * S + 0.25 * Math.Pow((K - 3), 2)); // Statistique de test

            return JB < 0.05; // Condition pour que la série suive une loi normale

        }

        // Méthode calculant la Tracking Error (TE)
        // Input : 
        // - returnsPortfolio : vecteur des rendements historiques du portefeuille
        // - returnsBenchmark : vecteur des rendements historiques du benchmark
        // Output : 
        // - valeur de la Tracking Error

        // Interprétation : 
        // - Si la TE est comprise en 0 et 2 : Gestion passive
        // - Si la TE est comprise entre 2 et 10 : Gestion active
        // - Si la TE est supérieure à 10 : le benchmark ne convient pas
        public static double TrackingError(Column returnsPortfolio, Column returnsBenchmark)
        {

            
            Column returnsDiff = returnsPortfolio.DiffMat(returnsBenchmark); // Vecteur des différences entre les rendements du portefeuille et ceux du benchmark

            return returnsDiff.StdDev(); // Ecart-type pour avoir la TE
        }

        // Calcul du bêta d'une série (via régression linéaire simple)
        // Inputs : 
        // - returns : rendements historiques
        // - benchmark : rendements historiques du benchmakr
        // Output : valeur du beta

        // Interprétation : 
        // Si le beta < 0 : 
        // Si le beta = 0 : 
        // Si le 0 < beta < 1 : 
        // Si le beta > 1 : 
        public static double Beta(Column returns, Column benchmark)
        {
            
            (double alpha, double beta) = Stats.SimpleLinearRegression(returns, benchmark); // Régression linéaire sur les returns et le benchmark pour obtenir le beta de la série
            return beta;
        }

        // Méthode calculant le risque systématique et le risque spécifique d'un portefeuille/titre
        // Inputs : 
        // - returns : vecteur des rendements historiques
        // - benchmark : vecteur des rendements historiques du benchmark
        // Outputs : 
        // - risque systématique
        // - risque spécifique
        public static (double, double) Systematic_Specific_Risk(Column returns, Column benchmark)
        {
            //Calcul beta
            (double alpha, double beta) = Stats.SimpleLinearRegression(returns, benchmark);

            // Vecteurs contenant les parts systémiques et spécifiques du vecteur returns
            Column ExplainedX = new Column(benchmark.Length);
            Column NonExplainedX = new Column(benchmark.Length);

            //Calcul Explained et NonExplained
            for (int i = 0; i < benchmark.Length; i++)
            {
                ExplainedX[i] = beta * benchmark[i]; // à partir du b de la série 
            }
            NonExplainedX = benchmark.DiffMat(ExplainedX); //on fait la différence pour avoir la partie spécifique 

            // On prend l'écart type des séries (annualisé) pour avoir la part systémique et la part spécifique 
            double systRisk = ExplainedX.StdDev() * Math.Sqrt(252);
            double specRisk = NonExplainedX.StdDev() * Math.Sqrt(252);

            return (systRisk, specRisk);
        }

        // Méthode retournant le risque total annualisé du portefeuille 
        // Calcul de l'écart-type annualisé
        public static double TotalRisk(Matrix returns)
        {
            return returns.StdDev() * Math.Sqrt(252);
        }

    }

    // Classe avec des méthodes statistiques/économétriques utilisées pour effectuer les opérations de backtests
    public static class Stats
    {
        // Méthode permettant de retourner l'inverse de la fonction de répartition de la loi normale
        // Renvoie, pour une probabilité donnée, la valeur d’une variable aléatoire suivant une loi normale standard.
        // Cette distribution a une moyenne égale à zéro et un écart type égal à 1.
        // Input : proba comprise entre 0 et 1
        // Output : résultat
        // Inspiré de Acklam's Algorithm for the Inverse Normal CDF (pseudo code)
        public static double InverseNormalDistribution(double proba)
        {
            // Coefficients en approximation diophantienne (approximaiton rationnelle d'un réel)
            // Coefficients calculés par un autre algorithme de Acklam
            const double a1 = -3.969683028665376e+01;
            const double a2 = 2.209460984245205e+02;
            const double a3 = -2.759285104469687e+02;
            const double a4 = 1.383577518672690e+02;
            const double a5 = -3.066479806614716e+01;
            const double a6 = 2.506628277459239e+00;

            const double b1 = -5.447609879822406e+01;
            const double b2 = 1.615858368580409e+02;
            const double b3 = -1.556989798598866e+02;
            const double b4 = 6.680131188771972e+01;
            const double b5 = -1.328068155288572e+01;

            const double c1 = -7.784894002430293e-03;
            const double c2 = -3.223964580411365e-01;
            const double c3 = -2.400758277161838e+00;
            const double c4 = -2.549732539343734e+00;
            const double c5 = 4.374664141464968e+00;
            const double c6 = 2.938163982698783e+00;

            const double d1 = 7.784695709041462e-03;
            const double d2 = 3.224671290700398e-01;
            const double d3 = 2.445134137142996e+00;
            const double d4 = 3.754408661907416e+00;

            // Définition des points de ruptures (paliers)
            // Avec deux paliers, nous avons trois groupes (les deux queues et le centre)
            const double low = 0.02425;
            const double high = 1 - low;

            double q, x;

            // Vérification et renvoie de resultat en fonction de proba
            // Connaît certaines valeurs à retourner en fonctio de la valeur de proba, donc on les renvoie ici 
            if (proba < 0 || proba > 1.0)
            {
                throw new ArgumentException("Probabilité doit être entre 0 et 1");
            }
            else if (proba == 0.5)
            {
                return 0.0;
            }
            else if (proba == 0)
            {
                return double.NegativeInfinity;
            }
            else if (proba == 1)
            {
                return double.PositiveInfinity;
            }
            // Approximation diophantienne pour le groupe sous le premier palier 
            else if (0 < proba && proba < low)
            {
                q = Math.Sqrt(-2 * Math.Log(proba));
                x = (((((c1 * q + c2) * q + c3) * q + c4) * q + c5) * q + c6) / ((((d1 * q + d2) * q + d3) * q + d4) * q + 1);
                return x;
            }
            // Approximation diophantienne pour le groupe entre les deux paliers
            else if (low <= proba && proba <= high)
            {
                q = proba - 0.5;
                double r = q * q;
                x = (((((a1 * r + a2) * r + a3) * r + a4) * r + a5) * r + a6) * q / (((((b1 * r + b2) * r + b3) * r + b4) * r + b5) * r + 1);
                return x;
            }
            // Approximation diophantienne pour le groupe après le deuxième palier 
            else
            {
                q = Math.Sqrt(-2 * Math.Log(1 - proba));
                x = -(((((c1 * q + c2) * q + c3) * q + c4) * q + c5) * q + c6) / ((((d1 * q + d2) * q + d3) * q + d4) * q + 1);
                return x;
            }
        }

        // Méthode effectuant un test binomial
        // Utilisaiton : Comparer les fréquences observées (vos résultats) aux fréquences théoriques (ce que prédit le hasard).
        // Inputs : 
        // - succes : modalité 1 ou 0 (oui ou non)
        // - size : taille de l'échantillon
        // - proba : niveau de signification
        // Output : résultat du test 
        public static double BinomialTest(long sucess, long size, double proba)
        {
            BigInteger c = Choose(size, sucess); // k parmi n

            double result = (double)c * Math.Pow(proba, sucess) * Math.Pow((1.0 - proba), (size - sucess)); // Formule du test
            return result;
        }

        // Méthode retournant un "BigInteger" : il faut un objet "BigInteger" car les nombre stockés et calculés par les factioriels sont trop gros.
        // Méthode permettant de calculer : k parmi n soit n! / (k!*(n-k)!)
        public static BigInteger Choose(long n, long k)
        {
            // Différents cas où ça ne marche pas en fonction des valeurs des inputs
            if (n < 0 || k < 0)
            {
                throw new Exception("Inputs négatifs");
            }
            else if (n < k)
            {
                return 0;
            }
            else if (k == 0)
            {
                return 1;
            }
            else if (n == k)
            {
                return 1;
            }

            // Calculs 
            long temp, max;
            if (k < n - k)
            {
                temp = n - k;
                max = k;
            }
            else
            {
                temp = k;
                max = n - k;
            }

            BigInteger result = temp + 1;
            for (int i = 2; i <= max; i++)
            {
                result = (result * (temp + i)) / i;
            }
            return result;

        }

        // Méthode effecutant une régression linéaire simple
        // Metre en Y les rdts du pf et en x les rdts du bench pour obtenir le béta et alpha du pf
        // Interprétation financière : 
        // Si le -1 < beta < 0 : le portefeuille réagit inversement proportionnellement à son bench
        // Si le beta = 0 : le portefeuille ne réagit pas comme son bench
        // Si le 0 < beta < 1 : le portefeuille réagit proportionnellement à son bench
        // Si le beta > 1 ou beta < -1 : Erreur
        public static (double, double) SimpleLinearRegression(Column Y, Column x)
        {
            // Check sur X et Y : même taille pour les vecteurs
            if (Y.Length != x.Length)
            {
                throw new Exception("X et Y must be the same length.");
            }

            // Création d'une X sur laquelle on effectue les opéréations de régressions 
            // Sa première colonne n'est remplie qu'avec des 1.0
            Matrix X = new Matrix(x.Length, 2, 1.0);

            // Remplissage de la deuxième colonne de X
            for (int j = 0; j < x.Length; j++)
            {
                X[j, 1] = x[j];
            }

            // Calculs de la régression linéaire de X sur Y
            Column res = X.TransMat().MultMat(X).Inverse().MultMat(X.TransMat()).MultMat(Y);

            // res est un vecteur de 2x1
            double alpha = res[0];
            double beta = res[1];

            return (alpha, beta);
        }

    }

    // Classe permettant de calculer et récupérer facilement les différente VaR du portefeuille
    public class VaR
    {
        private double daily;
        private double annual;
        private double pctBreach;
        private double testBinomial;

        public VaR(Func<Column, int, double, int, double> VaRFunction, Column returns, int horizon = 252, double niveau_confiance = 0.95, int nbSimuls = 1000)
        {
            this.daily = VaRFunction(returns, 1, niveau_confiance, nbSimuls);
            this.annual = this.daily * Math.Sqrt(252);
            try
            {
                ValueTuple<double, double> res = Backtest.BacktestVaR(VaRFunction, returns, horizon, niveau_confiance, nbSimuls);
                this.pctBreach = res.Item1;
                this.testBinomial = res.Item2;
            }
            catch (IndexOutOfRangeException e)
            {
                this.pctBreach = Double.NaN;
                this.testBinomial = Double.NaN;
            }
        }

        #region Accesseurs
        public double Daily { get => daily; }
        public double Annual { get => annual; }
        public double PctBreach { get => pctBreach; }
        public double TestBinomial { get => testBinomial; }
        #endregion
    }
}
