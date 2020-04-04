using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace ClassLibrary1
{
    public class ExcelApplication
    {
        //We declare the excel app so that it is globally accessible
        private Excel.Application objApp;
        private Excel._Workbook objBook;
        private Excel._Worksheet objSheet;
        private bool writing;

        public ExcelApplication(bool writing = false)
        {
            this.writing = writing;
            objApp = new Excel.Application
            {
                Visible = writing
            };

            objBook = null;
            objSheet = null;

            if (writing)
            {
                objBook = objApp.Workbooks.Add();
                objSheet = objBook.Worksheets[1];
                objSheet.Name = "Charts";

                // prevent excel from recalculating after each cell change
                objApp.Calculation = XlCalculation.xlCalculationManual;
            }
        }

        ~ExcelApplication()
        {
            try
            {
                if (writing)
                {
                    // revert change on calculation
                    objApp.Calculation = XlCalculation.xlCalculationAutomatic;
                }
                else
                {
                    if (objBook != null)
                        objBook.Close(0);
                    objApp.Quit();
                }
            }
            catch
            {
                return;
            }
        }

        public void OpenWorkbook(string excelFileName, string path)
        {
            // Normally we use a path, but to stay compatible with tests we'll keep this
            if (path == null)
                path = Path.Combine(Directory.GetCurrentDirectory(), excelFileName);

            // Instantiate Excel and start a new workbook.
            try
            {
                this.objBook = this.objApp.Workbooks.Open(path);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                throw new Exception(string.Format("Le fichier {0} demandé n'existe pas.", path));
            }

            this.objSheet = this.objBook.Worksheets.get_Item(1);
        }

        private dynamic LoadDataFromOpenWk()
        {
            //Retrieve the data from the used range of the workbook.
            //1-based array
            return this.objSheet.UsedRange.get_Value(Missing.Value);
        }

        private Excel.Worksheet GetSheet(int sheet)
        {
            while (sheet > objBook.Worksheets.Count)
                objBook.Worksheets.Add(Missing.Value, objBook.Worksheets[objBook.Worksheets.Count], sheet - objBook.Worksheets.Count);

            return objBook.Worksheets[sheet];
        }

        private DateTime[,] ColumnDateTime(DateTime[] dates)
        {
            DateTime[,] res = new DateTime[dates.Length, 1];
            for (int i = 0; i < dates.Length; ++i)
                res[i, 0] = dates[i];

            return res;
        }

        public void ChangeSheetName(int sheet, string name)
        {
            Excel.Worksheet ws = GetSheet(sheet);
            ws.Name = name;
        }

        public void WriteCompo(Column weights, string[] tickers, int sheet)
        {
            // Necessaire car un array unidimensionel n'est pas writable dans un Excel.Range[n, 1], le premier element serait répété n fois
            string[,] tickers_multi = new string[tickers.Length, 1];
            for (int i = 0; i < tickers.Length; ++i)
            {
                tickers_multi[i, 0] = tickers[i];
            }

            Excel.Worksheet ws = GetSheet(sheet);

            ws.Cells[1, 1] = "Tickers";
            ws.Cells[1, 2] = "Poids";

            // Create ranges
            Range tickerRange = ws.Range[ws.Cells[2, 1], ws.Cells[tickers.Length + 1, 1]];
            Range weightRange = ws.Range[ws.Cells[2, 2], ws.Cells[weights.Length + 1, 2]];

            // Write array (fast)
            tickerRange.Value = tickers_multi;
            weightRange.Value = weights.GetArray();
            weightRange.NumberFormat = "0.00%";
        }

        public void Write(Matrix data, string col_name, DateTime[] dates, int sheet)
        {
            Write(data, new string[] { col_name }, dates, sheet);
        }

        public void Write(Matrix data, string[] col_names, DateTime[] dates, int sheet)
        {
            // Necessaire car un array unidimensionel n'est pas writable dans un Excel.Range[n, 1], le premier element serait répété n fois
            DateTime[,] dates_multi = ColumnDateTime(dates);

            Excel.Worksheet ws = GetSheet(sheet);

            // Headers
            ws.Cells[1, 1] = "date";

            // Write by cell (slow but not a lot of column name)
            for (int j = 0; j < col_names.Length; j++)
            {
                ws.Cells[1, j + 2] = col_names[j];
            }

            // Create ranges
            Range formatRange;
            Range datesRange = ws.Range[ws.Cells[2, 1], ws.Cells[dates.Length + 1, 1]];
            Range dataRange = ws.Range[ws.Cells[2, 2], ws.Cells[data.Height + 1, data.Width + 1]];

            // Write array (fast)
            datesRange.Value = dates_multi;
            dataRange.Value = data.GetArray();

            // Format by column (faster as nb of rows increase)  
            datesRange.NumberFormat = "DD/MM/YYYY";
            for (var i = 0; i < data.Width; ++i)
            {
                formatRange = ws.Range[ws.Cells[2, i + 2], ws.Cells[data.Height + 1, i + 2]];
                formatRange.NumberFormat = "0.000";
            }
        }

        public void AppendVaR(string name, double value, int sheet)
        {
            Excel.Worksheet ws = GetSheet(sheet);

            int width = ws.UsedRange.Columns.Count;
            int height = ws.UsedRange.Rows.Count;

            ws.Cells[1, width + 1] = name;

            Range varRange = ws.Range[ws.Cells[2, width + 1], ws.Cells[height, width + 1]];
            varRange.Value = - value; // - var
            varRange.NumberFormat = "0.000";
        }

        // Prend en input l'index d'une feuille, et un titre
        public ChartObject LineChart(int sheet, string title)
        {
            // Create Line Chart from data
            ChartObject chartObject = objSheet.ChartObjects().Add(60 + (300 * (sheet-2)), 10, 300, 300);
            chartObject.Chart.SetSourceData(GetSheet(sheet).UsedRange);
            chartObject.Chart.HasTitle = true;
            chartObject.Chart.ChartTitle.Text = title;
            chartObject.Chart.ChartType = XlChartType.xlLine;
            if (title == "returns")
            {
                Axis theAxe = chartObject.Chart.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary);
                theAxe.TickLabelPosition = XlTickLabelPosition.xlTickLabelPositionLow;
                //theAxe.CategoryType = XlCategoryType.xlTimeScale;
            }

            return chartObject;
        }

        public void Save(string filename)
        {
            objApp.DisplayAlerts = false;
            objBook.SaveAs(filename, XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, true, false, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);
            objApp.DisplayAlerts = true;
        }

        public Asset[] ParseAssets(string method = "fillprev")
        {
            dynamic data = this.LoadDataFromOpenWk(); // /!\ data is 1-based
            int height = data.GetLength(0);
            int width = data.GetLength(1);

            Debug.Assert((string)data[1, 1] == "date");

            DateTime[] dates = new DateTime[height - 1];
            Asset[] assets = new Asset[width - 1];

            for (int i = 0; i < height - 1; ++i)
                dates[i] = data[i + 2, 1];

            for (int k = 0; k < width - 1; ++k)
            {
                double[] prices = Parse(method, data, height, k);

                assets[k] = new Asset(data[1, k + 2], dates, prices);
            }

            return assets;
        }

        public Tuple<DateTime[], double[]> ParseAsset(string method = "fillprev")
        {
            dynamic data = this.LoadDataFromOpenWk(); // /!\ data is 1-based
            int height = data.GetLength(0);
            int width = data.GetLength(1);

            Debug.Assert((string)data[1, 1] == "date");
            Debug.Assert(width == 2);

            DateTime[] dates = new DateTime[height - 1];

            for (int i = 0; i < height - 1; ++i)
                dates[i] = data[i + 2, 1];

            double[] prices = Parse(method, data, height, 0);

            return Tuple.Create(dates, prices);
        }

        private double[] Parse(string method, dynamic data, int height, int col)
        {
            double[] prices = new double[height - 1];

            for (int i = 0; i < height - 1; ++i)
            {
                if (data[i + 2, col + 2] == null)
                {
                    switch (method)
                    {
                        case "fillprev":
                            if (i == 0)
                                return Parse("zero", data, height, col); //Valeur nulle dans la premiere date du tableau, 'fillprev' impossible.
                            prices[i] = prices[i - 1];
                            break;

                        case "zero":
                            prices[i] = 0;
                            break;

                        default:
                            throw new ArgumentException(string.Format("La méthode de remplacement des valeurs nulles {0} n'existe pas.", method));
                    }
                }
                else
                    prices[i] = (double)data[i + 2, col + 2];
            }

            return prices;
        }
    }
}
