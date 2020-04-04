using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Matrix : IEnumerable
    {
        protected double[,] array;
        //public double[,] array;

        public int Height => array.GetLength(0);
        public int Width => array.GetLength(1);       
        public int Length => array.Length;

        #region Constructors
        public Matrix(double[,] array)
        {
            this.array = array;
        }

        public Matrix(Matrix copy)
        {
            this.array = new double[copy.Height, copy.Width];
            Array.Copy(copy.array, this.array, copy.array.Length);
        }

        public Matrix(int height, int width)
        {
            this.array = new double[height, width];
        }

        public Matrix(int height, int width, double default_value)
        {
            this.array = new double[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    this.array[i, j] = default_value;
                }
            }
        }

        public Matrix(Matrix1D[] matrices)
        {
            int myHeight = matrices[0].IsColumn ? matrices[0].Length : matrices.Length;
            int myWidth = matrices[0].IsColumn ? matrices.Length : matrices[0].Length; // TODO check

            foreach (Matrix1D mat in matrices)
            {
                if (mat.IsColumn != matrices[0].IsColumn)
                    throw new Exception(string.Format("Ces matrices ne sont pas toutes {0}.", matrices[0].IsColumn ? "colonne" : "ligne"));

                if (mat.Length != matrices[0].Length)
                    throw new Exception("Ces matrices ne sont pas toutes de la meme taille.");
            }

            this.array = new double[myHeight, myWidth];

            for (int i = 0; i < myHeight; i++)
            {
                for (int j = 0; j < myWidth; j++)
                {
                    this.array[i, j] = matrices[j][i];
                }
            }
        }
        #endregion

        public double this[int x, int y]
        {
            get => this.array[x, y];
            set => this.array[x, y] = value;
        }

        // A n'utiliser que par souci de compatibilité (Interop.Excel.Range par exemple)
        public double[,] GetArray()
        {
            return array;
        }

        #region Slices
        // Méthode récupérant une seule colonne d'une matrice
        public Column Column(int id)
        {
            Column result = new Column(this.Height);

            if (id > this.Width)
            {
                throw new IndexOutOfRangeException("colonnes d'une Matrix hors limite");
            }

            for (int i = 0; i < this.Height; i++)
            {
                result[i] = this.array[i, id];
            }
            return result;
        }

        // Méthode récupérant une ou plusieurs colonnes d'une matrice
        public Matrix Column(int id, int nb)
        {
            Matrix result = new Matrix(this.Height, nb, 0);

            if (id + nb > this.Width)
            {
                throw new IndexOutOfRangeException("colonnes d'une Matrix hors limite");
            }

            for (int i = 0; i < this.Height; i++)
            {
                for (int k = 0; k < nb; k++)
                {
                    result.array[i, k] = this.array[i, id + k];
                }
            }
            return result;
        }

        // Méthode récupérant une seule ligne d'une matrice
        public Row Row(int id)
        {
            Row result = new Row(this.Width);
            if (id > this.Height)
            {
                throw new IndexOutOfRangeException("lignes d'une Matrix hors limite");
            }

            Buffer.BlockCopy(this.array, id * this.Width * 8, result.array, 0, this.Width * 8);

            return result;
        }

        // Méthode récupérant une ou plusieurs lignes d'une matrice
        public Matrix Row(int id, int nb)
        {
            Matrix result = new Matrix(nb, this.Width, 0);
            if (id + nb > this.Height)
            {
                throw new IndexOutOfRangeException("lignes d'une Matrix hors limite");
            }
            Array.Copy(this.array, id * this.Width, result.array, 0, nb * this.Width);

            return result;
        }
        #endregion

        public Matrix Identite()
        {
            Matrix res = new Matrix(new double[this.Height, this.Width]);
            for (int i = 0; i < this.Height; i++)
            {
                for (int j = 0; j < this.Width; j++)
                {

                    if (i == j)
                    {
                        res[i, j] = 1.0;
                    }
                    else
                    {
                        res[i, j] = 0.0;
                    }

                }
            }
            return res;

        }

        public void Show()
        {
            for (int i = 0; i < this.Height; i++)
            {
                for (int j = 0; j < this.Width; j++)
                {
                    Console.Write("{0} ", this.array[i, j]);
                }
                Console.WriteLine();
            }
        }

        #region Operations between matrices
        public Matrix DiffMat(Matrix mat)
        {
            if (mat.Height != this.Height || mat.Width != this.Width)
                throw new InvalidOperationException("Les deux matrices doivent etre de meme taille");

            Matrix diff = new Matrix(this.Height, this.Width, 0);
            for (int i = 0; i < mat.Height; i++)
            {
                for (int j = 0; j < mat.Width; j++)
                {
                    diff.array[i, j] = this.array[i, j] - mat.array[i, j];
                }
            }
            return diff;
        }

        public Matrix SumMat(Matrix mat)
        {
            if (mat.Height != this.Height || mat.Width != this.Width)
                throw new InvalidOperationException("Les deux matrices doivent etre de meme taille");

            Matrix sum = new Matrix(this.Height, this.Width, 0);
            for (int i = 0; i < mat.Height; i++)
            {
                for (int j = 0; j < mat.Width; j++)
                {
                    sum.array[i, j] = this.array[i, j] + mat.array[i, j];
                }
            }
            return sum;
        }
        
        public Column MultMat(Column mat)
        {
            Matrix mult = MultMat((Matrix)mat);

            return new Column(mult);
        }

        public Matrix MultMat(Matrix mat)
        {
            int pivot = this.Width;
            if (pivot != mat.Height)
                throw new InvalidOperationException("Le nombre de colonnes de la matrice 1 doit etre celui des lignes de la 2");

            Matrix mult = new Matrix(this.Height, mat.Width, 0);
            for (int i = 0; i < mult.Height; i++)
            {
                for (int j = 0; j < mult.Width; j++)
                {
                    for (int k = 0; k < pivot; k++)
                    {
                        mult.array[i, j] = mult.array[i, j] + this.array[i, k] * mat.array[k, j];
                    }
                }
            }
            return mult;
        }
        #endregion

        #region Operations between matrices and scalars
        public static Matrix operator *(Matrix m, double scalar)
        {
            Matrix ret = new Matrix(m);
            for (int i = 0; i < m.Height; i++)
            {
                for (int j = 0; j < m.Width; j++)
                {
                    ret.array[i, j] *= scalar;
                }
            }
            return ret;
        }

        // Multiplication élement par élement
        public static Matrix operator *(Matrix m, Matrix m2)
        {
            if (m.Width != m2.Width | m.Height != m2.Height)
                throw new InvalidOperationException("Les matrices doivent avoir les mêmes dimensions");

            Matrix ret = new Matrix(m);

            for (int i = 0; i < m.Height; i++)
            {
                for (int j = 0; j < m.Width; j++)
                {
                    ret.array[i, j] *= m2[i,j];
                }
            }
            return ret;
        }

        public static Matrix operator *(double scalar, Matrix m)
        {
            return m * scalar;
        }

        public static Matrix operator +(Matrix m, double scalar)
        {
            Matrix ret = new Matrix(m);
            for (int i = 0; i < m.Height; i++)
            {
                for (int j = 0; j < m.Width; j++)
                {
                    ret.array[i, j] += scalar;
                }
            }
            return ret;
        }



        


        public static Matrix operator +(double scalar, Matrix m)
        {
            return m + scalar;
        }
        public static Matrix operator -(Matrix m, double scalar)
        {
            Matrix ret = new Matrix(m);
            for (int i = 0; i < m.Height; i++)
            {
                for (int j = 0; j < m.Width; j++)
                {
                    ret.array[i, j] -= scalar;
                }
            }
            return ret;
        }

        public static Matrix operator -(double scalar, Matrix m)
        {
            return m - scalar;
        }
        #endregion

        public Matrix CumProd(int axis = 0)
        {
            int col = this.Width;
            int row = this.Height;
            Matrix res = new Matrix(row, col, 0.0);

            if (axis == 0)
            {
                for (int i = 0; i < col; i++)
                {
                    res[0, i] = this[0, i];

                    for (int j = 1; j < row; j++)
                    {
                        res[j, i] = res[j - 1, i] * this[j, i];
                    }
                }
            }
            else if (axis == 1)
            {
                for (int i = 0; i < row; i++)
                {
                    res[i, 0] = this[i, 0];

                    for (int j = 1; j < col; j++)
                    {
                        res[i, j] = res[i - 1, j] * this[i, j];
                    }
                }
            }
            else
            {
                throw new Exception("Choose (0) for the rows, (1) for the columns");
            }
            return res;
        }

        #region Opérations internes
        // Méthode calculant la moyenne d'une matrice
        public double Average()
        {
            double result = 0;
            foreach (double elmt in this.array)
            {
                result += elmt;
            }
            result /= array.Length;
            return result;
        }

        // Méthode calculant le minimum d'une matrice
        public double Minimum()
        {
            double result = this.array[0, 0];
            foreach (double elmt in this.array)
            {
                if (elmt < result)
                {
                    result = elmt;
                }
            }
            return result;
        }

        // Méthode calculant le maximum d'une matrice
        public double Maximum()
        {
            double result = this.array[0, 0];
            foreach (double elmt in this.array)
            {
                if (elmt > result)
                {
                    result = elmt;
                }
            }
            return result;
        }

        // Méthode calculant l'écart type d'une matrice
        public double StdDev()
        {
            double array_average = this.Average();
            double result = 0;
            foreach (double elmt in this.array)
            {
                result += Math.Pow((elmt - array_average), 2);
            }
            result /= this.array.Length;
            result = Math.Sqrt(result);
            return result;

        }

        // Méthode calculant la somme des éléments d'une matrice
        public double Sum()
        {
            double sum = 0;
            for (int i = 0; i < this.Height; i++)
            {
                for (int j = 0; j < this.Width; j++)
                {
                    sum += this.array[i, j];
                }
            }
            return sum;
        }
        #endregion

        public Matrix1D Skewness(int axis = 0)
        {
            Matrix1D res;
            double average = 0.0;
            double std = 0.0;
            double skew = 0.0;

            // Check size
            if (axis == 0 && Height < 3)
            {
                throw new Exception("Not enough rows");
            }
            else if (axis == 1 && Width < 3)
            {
                throw new Exception("Not enough columns");
            }

            if (axis == 0)
            {
                res = new Row(Width);
                for (int i = 0; i < Width; i++)
                {
                    average = this.Column(i).Average();
                    std = this.Column(i).StdDev();

                    for (int k = 0; k < Height; k++)
                    {
                        skew += Math.Pow(this[k, i] - average, 3.0);
                    }
                    skew /= Math.Pow(std, 3.0);
                    skew = Height * skew / ((Height - 1.0) * (Height - 2.0));

                    res[i] = skew;
                }
            }
            else if (axis == 1)
            {
                res = new Column(Height);
                for (int j = 0; j < Height; j++)
                {
                    average = this.Row(j).Average();
                    std = this.Row(j).StdDev();

                    for (int k = 0; k < Width; k++)
                    {
                        skew += Math.Pow(this[j, k] - average, 3.0);
                    }
                    skew /= Math.Pow(std, 3.0);
                    skew = Width * skew / ((Width - 1.0) * (Width - 2.0));

                    res[j] = skew;
                }
            }
            else
                throw new Exception("Choose (0) for the rows, (1) for the columns");

            return res;
        }

        public Matrix1D Kurtosis(int axis = 0)
        {
            Matrix1D res;
            double average = 0.0;
            double var = 0.0;
            double kurt = 0.0;

            // Check size
            if (axis == 0 && Height < 4)
            {
                throw new Exception("Not enough rows");
            }
            else if (axis == 1 && Width < 4)
            {
                throw new Exception("Not enough columns");
            }

            if (axis == 0)
            {
                res = new Row(Width);
                for (int i = 0; i < Width; i++)
                {
                    average = this.Column(i).Average();
                    var = this.Column(i).Variance()[0];

                    for (int k = 0; k < Height; k++)
                    {
                        kurt += Math.Pow(this[k, i] - average, 4.0);
                    }
                    kurt /= var * var;
                    kurt *= (Height * (Height + 1.0)) / ((Height - 1.0) * (Height - 2.0) * (Height - 3.0));

                    res[i] = kurt;
                }
            }
            else if (axis == 1)
            {
                res = new Column(Height);
                for (int j = 0; j < Height; j++)
                {
                    average = this.Row(j).Average();
                    var = this.Row(j).Variance()[0];

                    for (int k = 0; k < Width; k++)
                    {
                        kurt += Math.Pow(this[j, k] - average, 2.0);
                    }
                    kurt /= var * var;
                    kurt *= (Width * (Width + 1.0)) / ((Width - 1.0) * (Width - 2.0) * (Width - 3.0));

                    res[j] = kurt;
                }
            }
            else
                throw new Exception("Choose (0) for the rows, (1) for the columns");

            return res;
        }

        public Matrix1D Variance(int axis = 0)
        {
            Matrix1D res;
            double average = 0.0;
            double var = 0.0;

            if (axis == 0)
            {
                res = new Row(Width);
                for (int i = 0; i < Width; i++)
                {
                    average = this.Column(i).Average();
                    for (int k = 0; k < Height; k++)
                    {
                        var += Math.Pow(this[k, i] - average, 2.0);
                    }
                    var /= Height;
                    res[i] = var;
                }
            }
            else if (axis == 1)
            {
                res = new Column(Height);
                for (int j = 0; j < Height; j++)
                {
                    average = this.Row(j).Average();
                    for (int k = 0; k < Width; k++)
                    {
                        var += Math.Pow(this[j, k] - average, 2.0);
                    }
                    var /= Width;
                    res[j] = var;
                }
            }
            else
                throw new Exception("Pas bonne dimension");

            return res;
        }

        // TODO code element wise sqrt
        public double Cov(Matrix mat2)
        {
            if (this.Height == mat2.Height && this.Width == 1 && mat2.Width == 1)
            {
                double Covar = (this.TransMat().MultMat(mat2).array[0, 0] / this.Height) - ((this.Sum() / this.Height) * (mat2.Sum() / this.Height));
                return Covar;
            }
            else
            {
                throw new InvalidOperationException("The matrices dimensions do not match");
            }
        }

        // Méthode calculant la matrice de variance-covariance pour une matrice contenant les actifs en colonnes
        public Matrix Varcov() // see if better to take list of Matrices instd of Matrix
        {
            Matrix result = new Matrix(Width, Width, 0);
            for (int i = 0; i < Width; i++)
            {
                for (int j = i; j < Width; j++)
                {
                    result[i, j] = this.Column(i).Cov(this.Column(j));
                }
            }
            return result;
        }

        // Méthode calculant la corrélation entre deux matrices colonne
        public double Corr(Matrix mat2)
        {
            double Correl = this.Cov(mat2) / System.Math.Sqrt(this.StdDev() * mat2.StdDev());
            return Correl;
        }

        public Matrix TransMat()
        {
            Matrix trans = new Matrix(this.Width, this.Height, 0);
            for (int i = 0; i < this.Height; i++)
            {
                for (int j = 0; j < this.Width; j++)
                {
                    trans.array[j, i] = this.array[i, j];
                }
            }
            return trans;
        }

        public bool AreEqual(Matrix mat)
        {
            // true if all values in matrixA == corresponding values in matrixB
            if (this.Height != mat.Height || this.Width != mat.Width)
                throw new Exception("Non-conformable matrices in AreEqualThe matrices passed in AreEqual are not the same size");

            for (int i = 0; i < this.Height; ++i)
                for (int j = 0; j < this.Width; ++j)
                    if (this.array[i, j] != mat.array[i, j])
                        return false;
            return true;
        }

        public double Determinant()
        {
            double det = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (i == j)
                        det = det + array[i, j];
                }
            }
            return det;
        }

        public static bool Tridiag_verif(Matrix a)
        {
            if (a.Width != a.Height)
            {
                throw new Exception("Matrice non carrée");
            }
            else
            {
                double k = 0;
                double l = 0;
                for (int i = 0; i < a.Height - 2; i++)
                {
                    for (int j = a.Width - 1; j > i + 1; j--)
                    {
                        k += a[i, j];
                    }

                    for (int j = i + 2; j < a.Height; j++)
                    {
                        l += a[j, i];
                    }
                }

                if (k == 0 && l == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public double Determinant_LUP()
        {
            //determinant d'une matrice triangulaire est égale au produits des elts de sa diagonale. Ici, 1.
            int t;
            double det = 1;

            LU LU = new LU(this);
            (LU.lower, LU.upper, LU.PIVOT, LU.t) = LU.LU_Matrices();

            for (int i = 0; i < LU.upper.Height; i++)
            {
                det *= LU.upper[i, i];
            }

            if (LU.t % 2 == 0)
            {
                t = 1;
            }
            else
            {
                t = -1;
            }

            return (t * det);

        }

        public Matrix Inverse()
        {
            //Matrix A = this;
            int n = this.Height;
            int nb = this.Width;
            double det = this.Determinant_LUP();
            if (n != nb)
            {
                throw new Exception("Unable to compute the Matrix Inversion : not squared matrix.");
            }
            else if (det == 0)
            {
                throw new Exception("Unable to compute the Matrix Inversion : determinant = 0.");
            }

            else
            {
                Matrix result = new Matrix(new double[n, n]);
                Matrix Identite = this.Identite();
                double[] x = new double[n];
                Matrix y = new Matrix(new double[n, 1]);

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        y[j, 0] = Identite[j, i];
                    }

                    Solver X = new Solver();
                    X.result = X.LUP_solver(this, y);

                    for (int j = 0; j < n; j++)
                    {
                        result[j, i] = X.result[j, 0];
                    }

                }

                return result;
            }
        }

        // Combiner 2 matrix selon la dimension , 0=en lignes , 1 =en colonnes
        public void Concat_Me(Matrix m2, int dim )
        {
            double[,] A_m2 = m2.GetArray();

           Concat_Me(A_m2,dim);

        }

        // Combiner 1 matrix et 1 Array selon la dimension , 1=en lignes , 2 =en colonnes
        public void Concat_Me(Double[,] m2, int dim)
        {
            double[,] A_m2 = m2;
            double[,] A_Final;

            // Concat les lignes
            if (dim == 0 && m2.GetLength(1) == this.Width)
            {
                A_Final = new double[m2.GetLength(0) + this.Height, this.Width];

                for (int i = 0; i < this.Width; i++)
                {
                    for (int j = 0; j < m2.GetLength(0) + this.Height; j++)
                    {
                        if (j < this.Height) { A_Final[j, i] = this.array[j, i]; }
                        else { A_Final[j, i] = m2[j - this.Height, i]; }
                    }
                }
            }
            //Concatener les colonnes
            else if (dim == 1 && m2.GetLength(0) == this.Height)
            {

                A_Final = new double[this.Height, this.Width + m2.GetLength(1)];

                for (int i = 0; i < this.Height; i++)
                {
                    for (int j = 0; j < m2.GetLength(1) + this.Width; j++)
                    {
                        if (j < this.Width) { A_Final[i, j] = this.array[i, j]; }
                        else { A_Final[i, j] = m2[i, j - this.Width]; }
                    }

                }


            }
            else
            {
                throw new ArgumentException("Matrix dimension not consistent with dimension chosen");

            }

            array = A_Final;

        }

        public static Matrix RepRows(Row Rw, int NbRep)
        {

            Row[] Rws = new Row[NbRep];

            for (int i = 0; i < NbRep; i++)
            {
                Rws[i] = Rw;
            }

            return new Matrix(Rws);
        }

        public static Matrix RepColumn(Column cl, int NbRep)
        {

            Column[] Cols = new Column[NbRep];

            for (int i = 0; i < NbRep; i++)
            {
                Cols[i] = cl;
            }

            return new Matrix(Cols);
        }

        #region Implementation of IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return array.GetEnumerator();
        }
        #endregion
    }

    public abstract class Matrix1D : Matrix
    {
        public bool IsColumn { get; }

        [Obsolete("Utilisez Length sur une matrice unidimensionelle", false)]
        new public int Height { get => base.Height; }
        [Obsolete("Utilisez Length sur une matrice unidimensionelle", false)]
        new public int Width { get => base.Width; }

        #region Constructors
        public Matrix1D(int length, bool column = true)
            : base(column ? length : 1, column ? 1 : length)
        {
            this.IsColumn = column;
        }

        public Matrix1D(int length, double default_value, bool column = true)
            : base(column ? length : 1, column ? 1 : length, default_value)
        {
            this.IsColumn = column;
        }

        public Matrix1D(double[] array, bool column = true)
            : base(column ? array.Length : 1, column ? 1 : array.Length)
        {
            this.IsColumn = column;
            Buffer.BlockCopy(array, 0, this.array, 0, array.Length * 8); // double = 8 * byte
        }

        public Matrix1D(double[,] array)
            : base(array.GetLength(0), array.GetLength(1))
        {
            this.IsColumn = array.GetLength(1) == 1;
            Buffer.BlockCopy(array, 0, this.array, 0, array.Length * 8); // double = 8 * byte
        }

        public Matrix1D(Matrix generic)
            : base(generic)
        {
            this.IsColumn = generic.Width == 1;
        }
        #endregion

        public double this[int idx]
        {
            get
            {
                return IsColumn ? this.array[idx, 0] : this.array[0, idx];
            }
            set
            {
                if (IsColumn)
                    this.array[idx, 0] = value;
                else
                    this.array[0, idx] = value;
            }
        }

        [Obsolete("La matrice est unidimensionelle", false)]
        new public double this[int x, int y]
        {
            get => base[x, y];
            set => base[x, y] = value;
        }

        public double[] ArrayFrom1D()
        {
            double[] res = new double[array.Length];

            Buffer.BlockCopy(array, 0, res, 0, array.Length * 8); // double = 8 * byte

            return res;
        }
    
        protected Matrix Rebase(int start_base = 100)
        {
            Matrix res = new Matrix(this.Length, 1);
            for (int i = 0; i < this.Length; ++i)
                res[i, 0] = this[i] / this[0] * start_base;

            return res;
        }
    }

    public class Row : Matrix1D
    {
        public Row(int length)
            : base(length, false)
        { }

        public Row(int length, double default_value)
            : base(length, default_value, true)
        { }

        public Row(double[] array)
            : base(array)
        { }

        public Row(double[,] array)
            : base(array)
        {
            if (array.GetLength(0) != 1)
                throw new FormatException("Cet array ne permet pas la creation d'une Row");
        }

        public Row(Matrix generic)
            : base(generic)
        {
            if (generic.Height != 1)
                throw new FormatException("La matrice ne peut pas etre transformée en Row.");
        }

        public Row Slice(int id, int nb)
        {
            double[] slice = new double[nb];
            Array.Copy(this.array, id, slice, 0, nb);
            return new Row(slice);
        }

        public Row DiffMat(Row mat)
        {
            return new Row(base.DiffMat(mat));
        }

        public Row SumMat(Row mat)
        {
            return new Row(base.SumMat(mat));
        }

        [Obsolete("Tentative de multiplier Row x Row, bizarre bizarre", true)]
        public void MultMat(Row mat)
        { return; }

        // TODO use override instead of new
        new public double MultMat(Column mat)
        {
            return base.MultMat(mat)[0];
        }

        new public Row MultMat(Matrix mat)
        {
            Matrix mult = base.MultMat(mat);

            return new Row(mult);
        }

        new public Column TransMat()
        {
            return new Column(this.ArrayFrom1D());
        }

        // As this method gets one or more Columns, it actually returns a Row for a Row object
        new public Row Column(int id, int nb)
        {
            return new Row(base.Column(id, nb));
        }

        new public Row Rebase(int start_base = 100)
        {
            return new Row(base.Rebase(start_base));
        }
    
    }

    public class Column : Matrix1D
    {
        public Column(int length)
            : base(length, true)
        { }

        public Column(int length, double default_value)
            : base(length, default_value, true)
        { }

        public Column(double[] array)
            : base(array)
        { }

        public Column(double[,] array)
            : base(array)
        {
            if (array.GetLength(1) != 1)
                throw new FormatException("Cet array ne permet pas la creation d'une Column");
        }

        public Column(Matrix generic)
            : base(generic)
        {
            if (generic.Width != 1)
                throw new FormatException("La matrice ne peut pas etre transformée en Column.");
        }

        public Column Slice(int id, int nb)
        {
            double[] slice = new double[nb];
            Array.Copy(this.array, id, slice, 0, nb);
            return new Column(slice);
        }

        public Column DiffMat(Column mat)
        {
            return new Column(base.DiffMat(mat));
        }

        public Column SumMat(Column mat)
        {
            return new Column(base.SumMat(mat));
        }

        [Obsolete("Tentative de multiplier Column x Column, bizarre bizarre", true)]
        new public void MultMat(Column mat)
        { return; }

        public Matrix MultMat(Row mat)
        {
            return base.MultMat(mat);
        }

        new public Row TransMat()
        {
            return new Row(this.ArrayFrom1D());
        }

        // As this method gets one or more Rows, it actually returns a Column for a Column object
        new public Column Row(int id, int nb)
        {
            return new Column(base.Row(id, nb));
        }

        new public Column Rebase(int start_base = 100)
        {
            return new Column(base.Rebase(start_base));
        }

        public Column CalculateReturns(string method = "variation")
        {
            Column returns = new Column(this.Length);
            returns[0] = 0;
            switch (method)
            {
                case "log":
                    for (int i = 1; i < this.Length; ++i)
                        returns[i] = Math.Log(this[i] / this[i - 1]);
                    break;
                case "variation":
                    for (int i = 1; i < this.Length; ++i)
                        returns[i] = this[i] / this[i - 1] - 1;
                    break;
                default:
                    throw new NotImplementedException(); // Important, si on ignore une methode inconnue on ne saura pas d'ou ca casse
            }
            return returns;
        }

        public static Column operator -(Column m, double scalar)
        {
            return new Column((Matrix)m - scalar);
        }

        public static Column operator -(double scalar, Column m)
        {
            return m - scalar;
        }
    }
}
