using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using System.Numerics;
using ClassLibrary1;

namespace Main
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            // *.xlsx sont copiés dans /bin/Debug ou /bin/Release
            CopyFilesToWorkingDirectory();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Display display = new Display();
            display.Show();
            Application.Run();

        }



        private static void CopyFilesToWorkingDirectory()
        {
            string root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string work = Directory.GetCurrentDirectory();
            foreach (string file in Directory.GetFiles(root, "*.xlsx"))
            {
                File.Copy(file, Path.Combine(work, Path.GetFileName(file)), true);
            }
        }
    }
}
