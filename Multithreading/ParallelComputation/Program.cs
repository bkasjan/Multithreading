using System;
using System.Threading.Tasks;

namespace ParallelComputation
{
    /// <summary>
    /// Demonstrate: Parallel computation
    /// No additional data validation etc. just showing exaple of the parallel computation
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var matrix1 = new int[1000, 1000];
            var matrix2 = new int[1000, 1000];
            var rand = new Random();

            // Fill in matrixes with some random values
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix2.GetLength(0); j++)
                {
                    matrix1[i, j] = rand.Next(0, 100);
                    matrix2[j, i] = rand.Next(0, 100);
                }
            }
            
            var result = MatrixMultiplication(matrix1, matrix1);
            var result2 = MatrixMultiplicationInParallel(matrix1, matrix1);

            Console.ReadKey();
        }

        /// <summary>
        /// Multiply matrix
        /// </summary>
        /// <param name="matrix1">1st matrix to multiply</param>
        /// <param name="matrix2">2nd matrix to multiply</param>
        /// <returns></returns>
        static int[,] MatrixMultiplication(int[,] matrix1, int[,] matrix2)
        {
            var start = DateTime.Now;

            var resultMatrix = new int[matrix1.GetLength(0), matrix2.GetLength(1)];
            for (int i = 0; i < resultMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < resultMatrix.GetLength(1); j++)
                {
                    resultMatrix[i, j] = 0;
                    for (int k = 0; k < matrix1.GetLength(1); k++)
                        resultMatrix[i, j] = resultMatrix[i, j] + matrix1[i, k] * matrix2[k, j];
                }
            }

            var end = DateTime.Now;
            Console.WriteLine($"Time span using regular loops: {end - start}");

            return resultMatrix;
        }

        /// <summary>
        /// Multiply matrix using parallel
        /// </summary>
        static int[,] MatrixMultiplicationInParallel(int[,] matrix1, int[,] matrix2)
        {
            var start = DateTime.Now;
            var resultMatrix = new int[matrix1.GetLength(0), matrix2.GetLength(1)];

            Parallel.For(0, matrix1.GetLength(0), delegate (int i)
            {
                for (int j = 0; j < resultMatrix.GetLength(1); j++)
                {
                    resultMatrix[i, j] = 0;
                    for (int k = 0; k < matrix1.GetLength(1); k++)
                        resultMatrix[i, j] = resultMatrix[i, j] + matrix1[i, k] * matrix2[k, j];
                }
            });

            var end = DateTime.Now;
            Console.WriteLine($"Time span using parallel: {end - start}");

            return resultMatrix;
        }
    }
}
