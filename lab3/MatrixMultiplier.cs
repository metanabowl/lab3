using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    internal class MatrixMultiplier
    {
            public static double[,] GenerateMatrix(int size)
            {
                var rand = new Random();
                var matrix = new double[size, size];
                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                        matrix[i, j] = rand.NextDouble() * 10;
                return matrix;
            }

            public static void MultiplySequential(double[,] A, double[,] B, double[,] result, int size)
            {
                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                    {
                        double sum = 0;
                        for (int k = 0; k < size; k++)
                            sum += A[i, k] * B[k, j];
                        result[i, j] = sum;
                    }
            }

            public static void MultiplyParallel(double[,] A, double[,] B, double[,] result, int size, int maxThreads)
            {
                var options = new ParallelOptions() { MaxDegreeOfParallelism = maxThreads };
                Parallel.For(0, size, options, i =>
                {
                    for (int j = 0; j < size; j++)
                    {
                        double sum = 0;
                        for (int k = 0; k < size; k++)
                            sum += A[i, k] * B[k, j];
                        result[i, j] = sum;
                    }
                });
            }

            public static bool CompareMatrices(double[,] m1, double[,] m2, double tolerance = 1e-6)
            {
                int size = m1.GetLength(0);
                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                        if (Math.Abs(m1[i, j] - m2[i, j]) > tolerance)
                            return false;
                return true;
            }
            public static void MultiplyThreaded(double[,] A, double[,] B, double[,] result, int size, int threadCount)
            {
                Thread[] threads = new Thread[threadCount];
                int rowsPerThread = size / threadCount;

                for (int t = 0; t < threadCount; t++)
                {
                    int startRow = t * rowsPerThread;
                    int endRow = (t == threadCount - 1) ? size : startRow + rowsPerThread;

                    threads[t] = new Thread(() =>
                    {
                        for (int i = startRow; i < endRow; i++)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                double sum = 0;
                                for (int k = 0; k < size; k++)
                                    sum += A[i, k] * B[k, j];
                                result[i, j] = sum;
                            }
                        }
                    });
                    threads[t].Start();
                }

                foreach (Thread thread in threads)
                    thread.Join();
            }
    }

}
