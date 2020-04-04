using System;
using NUnit.Framework;
using ClassLibrary1;
using Main;
using System.IO;

namespace UnitTestProject1
{
    [TestFixture]
    public class MatrixTest
    {
        double[,] arr_basic;
        double[,] arr_zero;
        double[,] arr_float;
        double[,] arr_3x4;
        double[,] arr_4x2;
        double[,] arr_1x4;
        double[,] arr_4x1;

        [SetUp]
        public void SetUp()
        {
            arr_basic = new double[3, 3]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 }
            };

            arr_zero = new double[3, 3]
            {
                { 0, 0, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 }
            };

            arr_float = new double[3, 3]
            {
                { 0.6, 0.3, 0.6 },
                { 0.3, 0.4, 0.9 },
                { 0.7, 0.8, 0.1 }
            };

            arr_3x4 = new double[3, 4]
            {
                { 0.6, 2.3, 0.6, 1 },
                { 5.3, 1, 1.9, 4 },
                { 0.7, 2, 0.1, 5.3 }
            };

            arr_4x2 = new double[4, 2]
            {
                { 3.4, 2.1 },
                { 5.3, 8.1 },
                { 4, 1.3 },
                { 0.7, 0.1 }
            };

            arr_1x4 = new double[1, 4]
            {
                { 3.4, 2.1, 3.1, 2.3 }
            };

            arr_4x1 = new double[4, 1]
            {
                { 3.4 },
                { 2.1 },
                { 3.1 },
                { 2.3 },
            };
        }

        [Test]
        public void Creation()
        {
            Matrix m = new Matrix(arr_basic);
            CollectionAssert.AreEqual(arr_basic, m);
        }

        [TestCase(-1)]
        [TestCase(1)]
        public void Plus(int scalar)
        {
            Matrix m = new Matrix(arr_basic);
            Matrix m2 = m + scalar;
            CollectionAssert.AreNotEqual(m, m2);
        }

        [Test]
        public void Sum()
        {
            Matrix m1 = new Matrix(arr_3x4);
            Assert.AreEqual(24.8, m1.Sum(), 0.0000000001);
        }

        [Test]
        public void TimesZero()
        {
            Matrix m = new Matrix(arr_basic);
            Matrix m2 = m * 0;
            CollectionAssert.AreNotEqual(m, m2);
            Assert.AreEqual(0, m2.Sum());
        }

        [Test]
        public void SumMat()
        {
            Matrix m1 = new Matrix(arr_basic);
            Matrix m2 = new Matrix(arr_float);
            Matrix result = m1.SumMat(m2);
            CollectionAssert.AreNotEqual(m1, result);
            CollectionAssert.AreEqual(new Matrix(new double[3, 3] {
                { 1.6, 2.3, 3.6 },
                { 4.3, 5.4, 6.9 },
                { 7.7, 8.8, 9.1 }
            }), result);
        }

        [Test]
        public void DiffMat()
        {
            Matrix m1 = new Matrix(arr_basic);
            Matrix m2 = new Matrix(arr_float);
            Matrix result = m1.DiffMat(m2);
            CollectionAssert.AreNotEqual(m1, result);
            CollectionAssert.AreEqual(new Matrix(new double[3, 3] {
                { 0.4, 1.7, 2.4 },
                { 3.7, 4.6, 5.1 },
                { 6.3, 7.2, 8.9 }
            }), result);
            Matrix result2 = m2.DiffMat(m1);
            CollectionAssert.AreEqual(new Matrix(new double[3, 3] {
                { -0.4, -1.7, -2.4 },
                { -3.7, -4.6, -5.1 },
                { -6.3, -7.2, -8.9 }
            }), result2);
        }

        [Test]
        public void MultMat()
        {
            Matrix m1 = new Matrix(arr_3x4);
            Matrix m2 = new Matrix(arr_4x2);
            Matrix result = m1.MultMat(m2);
            CollectionAssert.AreNotEqual(m1, result);
            CollectionAssert.AreEqual(new Matrix(new double[3, 2] {
                { 17.33, 20.77 },
                { 33.72, 22.1 },
                { 17.09, 18.33 }
            }), result, new DoubleComparer(0.001));
        }

        [Test]
        public void SumMatDifferentSize()
        {
            Matrix m1 = new Matrix(arr_3x4);
            Matrix m2 = new Matrix(arr_4x2);
            Assert.Throws<InvalidOperationException>(() => m1.SumMat(m2));
        }

        [Test]
        public void DiffMatDifferentSize()
        {
            Matrix m1 = new Matrix(arr_3x4);
            Matrix m2 = new Matrix(arr_4x2);
            Assert.Throws<InvalidOperationException>(() => m1.DiffMat(m2));
        }

        [Test]
        public void MultMatDifferentSize()
        {
            Matrix m1 = new Matrix(arr_3x4);
            Matrix m2 = new Matrix(arr_4x2);
            Matrix result = m1.MultMat(m2);
            CollectionAssert.AreNotEqual(m1, result);
            CollectionAssert.AreEqual(new Matrix(new double[3, 2] {
                { 17.33, 20.77 },
                { 33.72, 22.1 },
                { 17.09, 18.33 }
            }), result, new DoubleComparer(0.001));
        }

        [Test]
        public void MultMat2()
        {
            Matrix m1 = new Matrix(arr_4x1);
            Matrix m2 = new Matrix(arr_1x4);
            Matrix m3 = m1.MultMat(m2);
            CollectionAssert.AreEqual(new double[,] {
                { 11.56, 7.14, 10.54, 7.82 },
                { 7.14, 4.41, 6.51, 4.83 },
                { 10.54, 6.51, 9.61, 7.13 },
                { 7.82, 4.83, 7.13, 5.29 }
            }, m3, new DoubleComparer(0.0001));
        }

        [Test]
        public void MultMat3()
        {
            Matrix m1 = new Matrix(arr_1x4);
            Matrix m2 = new Matrix(arr_4x1);
            Matrix m3 = m1.MultMat(m2);
            CollectionAssert.AreEqual(new double[] { 30.87 }, m3, new DoubleComparer(0.0001));
        }

        [Test]
        public void MultInMat()
        {
            Column m1 = new Column(arr_4x1);
            Row m2 = new Row(arr_1x4);
            Matrix res = m1.MultMat(m2);
            CollectionAssert.AreEqual(new double[,] {
                { 11.56, 7.14, 10.54, 7.82 },
                { 7.14, 4.41, 6.51, 4.83 },
                { 10.54, 6.51, 9.61, 7.13 },
                { 7.82, 4.83, 7.13, 5.29 }
            }, res, new DoubleComparer(0.0001));
        }

        [Test]
        public void MultInScalar()
        {
            Row m1 = new Row(arr_1x4);
            Column m2 = new Column(arr_4x1);
            double res = m1.MultMat(m2);
            Assert.AreEqual(30.87, res, 0.0001);
        }

        [Test]
        public void MultIn1D()
        {
            Row m1 = new Row(arr_1x4);
            Matrix m2 = new Matrix(arr_4x2);
            Matrix1D res = m1.MultMat(m2);
            CollectionAssert.AreEqual(new double[,] { { 36.7, 28.41 } }, res, new DoubleComparer(0.0001));
        }

        [Test]
        public void Transpose1D()
        {
            Row m1 = new Row(arr_1x4);
            Column m2 = new Column(arr_4x1);
            Column m3 = m1.TransMat();
            CollectionAssert.AreEqual(m2, m3);
        }

        [Test]
        public void TransposeMat()
        {
            Matrix m1 = new Matrix(arr_3x4);
            Matrix m2 = new Matrix(new double[4, 3]
            {
                { 0.6, 5.3, 0.7 },
                { 2.3, 1, 2 },
                { 0.6, 1.9, 0.1 },
                { 1, 4, 5.3 }
            });
            Matrix m3 = m1.TransMat();
            CollectionAssert.AreEqual(m2, m3);
            CollectionAssert.AreNotEqual(m1, m3);
        }
    }
}
