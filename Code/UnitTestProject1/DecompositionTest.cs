using System;
using NUnit.Framework;
using ClassLibrary1;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class DecompositionTest
    {
        double[,] arr_basic = new double [3,3] { { 2, 7, 6 }, { 9, 5, 1 }, { 4, 3, 8 } };
        double[,] arr_pivot = new double [3,3] { { 0, 1, 0 }, { 1, 0, 0 }, { 0, 0, 1 } };
        double[,] arr_rearangee = new double[3, 3] { { 9, 5, 1 }, { 2, 7, 6 }, { 4, 3, 8 } };
        double[,] arr_thomas = new double [3,3] { { 2, -1, 0 }, { 0, 3.0 / 2, -1 }, { 0, 0, 4.0 / 3 } };
        

        [TestMethod]
        public void pivot()
        {
            Matrix m = new Matrix(arr_basic);
            Matrix m2 = new Matrix(arr_rearangee);
            Matrix m3 = new Matrix(arr_pivot);
            Decomposition Mat_Pivot = new Decomposition();
            int t;
            (Mat_Pivot.Matrice_modif, Mat_Pivot.PIVOT, t) = Decomposition.pivot2(m);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(t, 1);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(Mat_Pivot.Matrice_modif[i, j], m2[i, j]);
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(Mat_Pivot.PIVOT[i, j], m3[i, j]);
                }
            }
        }

        [TestMethod]
        public void LU()
        {
            int t;
            Matrix m = new Matrix(arr_basic);

            Decomposition Mat_Pivot = new Decomposition();
            (Mat_Pivot.Matrice_modif, Mat_Pivot.PIVOT, t) = Decomposition.pivot2(m);

            LU LU = new LU(m);
            (LU.lower, LU.upper, LU.PIVOT, t) = LU.LU_Matrices();

            Matrix res_LU = new Matrix(new double[m.Height, m.Width]);
            res_LU = LU.lower.MultMat(LU.upper);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(res_LU[i, j], Mat_Pivot.Matrice_modif[i, j]);
                }
            }
        }

        [TestMethod]
        public void Thomas()
        {
            Matrix m = new Matrix(arr_thomas);
            Thomas Thomas = new Thomas(m);
            (Thomas.lower, Thomas.upper) = Thomas.Thomas_Matrices();
            Matrix res_Thom = new Matrix(new double[m.Height, m.Width]);
            res_Thom = Thomas.lower.MultMat(Thomas.upper);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(res_Thom[i, j], m[i, j]);
                }
            }
        }
        [TestMethod]
        public void Determinant()
        {
            Matrix m = new Matrix(arr_basic);
            double det = m.Determinant_LUP();
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(det, -360);
        }
    }
}
