using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Decomposition
    {
        public Matrix Mat;
        public Matrix Matrice_modif;
        public Matrix PIVOT;


        public static (Matrix, Matrix, int) pivot2(Matrix Matrice_modif) // Technique pivot 2
        {
            int N = Matrice_modif.Height;
            int NC = Matrice_modif.Width;
            if (N != NC)
            {
                throw new Exception("Unable to compute Matrix Pivot Operation : Not squared Matrix.");
            }
            else
            {

                Matrix permut = new Matrix(Matrice_modif);
                Matrix identite = new Matrix(new double[Matrice_modif.Height, Matrice_modif.Width]);
                identite = identite.Identite(); //Matrice identitée de départ 
                int t = 0; //Nb de permutations 

                for (int i = 0; i < N - 1; i++)
                {
                    double max = 0;
                    double elt = 0;
                    int row = 0;
                    double tmp = 0;
                    for (int j = i; j < N; j++)
                    {
                        elt = permut[j, i];

                        if (Math.Abs(elt) > max)
                        {
                            max = Math.Abs(elt);
                            row = j;
                        }
                    }
                    if (i != row)
                    {
                        t++;
                        for (int j = 0; j < N; j++)
                        {

                            tmp = identite[i, j];
                            identite[i, j] = identite[row, j];
                            identite[row, j] = tmp;
                            tmp = permut[i, j];
                            permut[i, j] = permut[row, j];
                            permut[row, j] = tmp;
                        }
                    }
                }
                return (permut, identite, t);
            }
        }


    }

    public class LU : Decomposition
    {
        public Matrix lower;
        public Matrix upper;
        public Matrix LU_mat;
        public int[] pivot_mat;
        public int t; //Nombre de permutations de la matrice de permutation (pour le calcul du déterminant)

        public LU(Matrix M)
        {
            this.Mat = M;
            (this.Matrice_modif, this.PIVOT, t) = pivot2(M);
            //this.Matrice_modif = M;

            lower = new Matrix(new double[M.Height, M.Width]);
            upper = new Matrix(new double[M.Height, M.Width]);
            //LU_mat = new Matrix(new double[M.Height, M.Width]);
            LU_mat = new Matrix(this.Matrice_modif);

        }

        public (Matrix, Matrix, Matrix, int) LU_Matrices()
        {
            //Matrix LU_mat = new Matrix(this.Matrice_modif); // Matrix.Dupliquer(this.Matrice_modif);
            int n = Mat.Height;

            for (int i = 0; i < n; i++)
            {
                // Upper Triangular 
                for (int k = i; k < n; k++)
                {

                    // Summation of L(i, j) * U(j, k) 
                    double sum = 0;
                    for (int j = 0; j < i; j++)
                        sum += (lower[i, j] * upper[j, k]);

                    // Evaluating U(i, k) 
                    upper[i, k] = Matrice_modif[i, k] - sum;
                }
                //Lower triangle
                for (int k = i; k < n; k++)
                {
                    if (i == k)
                        lower[i, i] = 1; // Diagonal as 1 
                    else
                    {

                        // Summation of L(k, j) * U(j, i) 
                        double sum = 0;
                        for (int j = 0; j < i; j++)
                            sum += (lower[k, j] * upper[j, i]);

                        // Evaluating L(k, i) 
                        lower[k, i] = (Matrice_modif[k, i] - sum) / upper[i, i];
                    }
                }

                for (int k = i + 1; k < n; k++)
                {
                    LU_mat[k, i] = LU_mat[k, i] / LU_mat[i, i];
                    for (int j = i + 1; j < n; j++)
                    {
                        LU_mat[k, j] = LU_mat[k, j] - (LU_mat[k, i] * LU_mat[i, j]);
                    }
                }

            }
            return (lower, upper, this.PIVOT, t);
        }
    }
    public class Thomas : Decomposition
    {
        public Matrix lower;
        public Matrix upper;
        public int[] pivot_mat;
        public Matrix Thomas_mat;

        public Thomas(Matrix M)
        {
            this.Mat = M;
            //(this.Matrice_modif, this.pivot_mat) = pivot(M);

            lower = new Matrix(new double[M.Height, M.Width]);
            upper = new Matrix(new double[M.Height, M.Width]);
            //Thomas_mat = new Matrix(new double[M.Height, M.Width]);
            //Thomas_mat = Matrice.Dupliquer(this.Matrice_modif);
        }

        public (Matrix, Matrix) Thomas_Matrices()
        {
            //Vérifier si la matrice entrée en constructeur est bien carrée 
            bool tri;
            tri = Matrix.Tridiag_verif(Mat);

            if (tri == false)
            {
                throw new Exception("La matrice n'est pas tridiagonale.");
            }
            else
            {

                int n = Mat.Height;
                //Thomas_mat = Matrice.Dupliquer(this.Matrice_modif);
                double[] mid = new double[n]; double[] up = new double[n - 1]; double[] down = new double[n - 1];
                double[] alpha = new double[n]; double[] beta = new double[n - 1];

                for (int i = 0; i < n; i++)
                {

                    for (int j = i; j < n; j++)
                    {
                        if (i == j)
                        {
                            mid[i] = Mat[j, i];
                        }
                        else
                        {
                            up[i] = Mat[i, i + 1];
                            down[i] = Mat[i + 1, i];
                        }
                    }
                }
                alpha[0] = mid[0];
                for (int k = 1; k < n; k++)
                {
                    beta[k - 1] = down[k - 1] / alpha[k - 1];
                    alpha[k] = mid[k] - beta[k - 1] * up[k - 1];
                }

                for (int i = 0; i < n; i++)
                {
                    for (int j = i; j < n; j++)
                    {
                        if (i == j)
                        {
                            lower[j, i] = 1;
                            upper[j, i] = alpha[i];
                        }
                        else
                        {
                            upper[i, i + 1] = up[i];
                            lower[i + 1, i] = beta[i];
                        }
                    }
                }
                return (lower, upper);

            }

        }
    }
    public class Solver
    {
        public Matrix result;
        public Matrix result_T;

        public Solver()
        {
            result = new Matrix(new double[1, 1]);

        }

        public Matrix Thomas_solver(Matrix A, Matrix b)
        {
            int n = A.Height;
            int nb = b.Height;
            if (n != nb)
            {
                throw new Exception("Unable to compute Matrix Thomas Decomposition : not the same number of rows.");
            }
            else
            {
                result = new Matrix(new double[n, 1]);
                double[] y = new double[n];
                double[] alpha = new double[n]; double[] c = new double[n - 1]; double[] beta = new double[n - 1];

                //Décomposition de Thomas
                Thomas M = new Thomas(A);
                (M.lower, M.upper) = M.Thomas_Matrices();

                for (int i = 0; i < n; i++)
                {

                    for (int j = i; j < n; j++)
                    {
                        if (i == j)
                        {
                            alpha[i] = M.upper[j, i];
                        }
                        else
                        {
                            c[i] = M.upper[i, i + 1];
                            beta[i] = M.lower[i + 1, i];
                        }
                    }
                }
                y[0] = b[0, 0];
                for (int k = 1; k < n; k++)
                {
                    y[k] = b[k, 0] - beta[k - 1] * y[k - 1];
                    //beta[k - 1] = down[k - 1] / alpha[k - 1];
                    //alpha[k] = mid[k] - beta[k - 1] * up[k - 1];
                }

                result[n - 1, 0] = y[n - 1] / alpha[n - 1];
                for (int l = n - 2; l >= 0; l--)
                {
                    result[l, 0] = (y[l] - c[l] * result[l + 1, 0]) / alpha[l];
                }
                return result;
            }
        }

        public Matrix LUP_solver(Matrix A, Matrix b)
        {
            int n = A.Height;
            int nb = b.Height;
            if (n != nb)
            {
                throw new Exception("Unable to compute Matrix Thomas Decomposition : Matrix do not have the same number of rows.");
            }
            else
            {
                result = new Matrix(new double[n, 1]);
                double[] y = new double[n];
                double sum_l = 0;
                double sum_u = 0;
                double c = 0; double d = 0;
                int t;

                //Décomposition LUP
                LU LU = new LU(A);
                (LU.lower, LU.upper, LU.PIVOT, t) = LU.LU_Matrices();

                //Faire les permutations pour la matrice de résultats
                Matrix bp = LU.PIVOT.MultMat(b);
                //Forward
                for (int i = 0; i < n; i++)
                {
                    sum_l = 0;
                    for (int j = 0; j < n; j++)
                    {
                        c = LU.lower[i, j];
                        sum_l += c * y[j];
                    }
                    y[i] = bp[i, 0] - sum_l;
                }

                //Backward
                for (int i = n - 1; i >= 0; i--)
                {
                    sum_u = 0;
                    for (int j = n - 1; j >= 0; j--)
                    {
                        d = LU.upper[i, j];
                        sum_u += d * result[j, 0];
                    }
                    result[i, 0] = (y[i] - sum_u) / LU.upper[i, i];

                }

                return result;
            }
        }
    }
}
