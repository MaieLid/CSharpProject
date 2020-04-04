using System;
using NUnit.Framework;
using ClassLibrary1;
using Main;
using System.IO;

namespace UnitTestProject1
{
    [SetUpFixture]
    public class OneTime
    {
        [OneTimeSetUp]
        public void BAE()
        {
            string root = Directory.GetParent(TestContext.CurrentContext.TestDirectory).Parent.Parent.FullName;
            string temp = Path.GetTempPath();
            foreach (string file in Directory.GetFiles(root, "*.xlsx"))
            {
                File.Copy(file, Path.Combine(temp, Path.GetFileName(file)), true);
            }
        }
    }
}
