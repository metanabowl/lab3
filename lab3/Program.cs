using System;
using System.Diagnostics;
using System.IO;

namespace lab3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] sizes = { 200, 500, 1000, 2000 };
            int[] threadsList = { 1, 2, 4, 8, 12, 24 };
            int repetitions = 3;
            string fileName = "wyniki.csv";

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("Rozmiar,Metoda,Wątki,ŚredniCzas(ms),Poprawność");

                foreach (int size in sizes)
                {
                    double[,] A = MatrixMultiplier.GenerateMatrix(size);
                    double[,] B = MatrixMultiplier.GenerateMatrix(size);
                    double[,] resultSeq = new double[size, size];
                    MatrixMultiplier.MultiplySequential(A, B, resultSeq, size);

                    foreach (int threads in threadsList)
                    {
                        Console.WriteLine($"[{DateTime.Now}] Rozmiar: {size}, Wątki: {threads}");

                        double avgTimeHigh = 0, avgTimeLow = 0;
                        bool correctHigh = true, correctLow = true;

                        for (int i = 0; i < repetitions; i++)
                        {
                            double[,] resultPar = new double[size, size];
                            var sw = Stopwatch.StartNew();
                            MatrixMultiplier.MultiplyParallel(A, B, resultPar, size, threads);
                            sw.Stop();
                            avgTimeHigh += sw.ElapsedMilliseconds;
                            correctHigh &= MatrixMultiplier.CompareMatrices(resultSeq, resultPar);
                        }
                        avgTimeHigh /= repetitions;
                        writer.WriteLine($"{size},Wysokopoziomowa,{threads},{avgTimeHigh},{correctHigh}");

                        for (int i = 0; i < repetitions; i++)
                        {
                            double[,] resultLow = new double[size, size];
                            var sw = Stopwatch.StartNew();
                            MatrixMultiplier.MultiplyThreaded(A, B, resultLow, size, threads);
                            sw.Stop();
                            avgTimeLow += sw.ElapsedMilliseconds;
                            correctLow &= MatrixMultiplier.CompareMatrices(resultSeq, resultLow);
                        }
                        avgTimeLow /= repetitions;
                        writer.WriteLine($"{size},Niskopoziomowa,{threads},{avgTimeLow},{correctLow}");

                        writer.Flush(); // na wypadek awarii
                    }
                }
            }

            Console.WriteLine("\nZakończono testy. Wyniki zapisano do pliku: wyniki.csv");
        }
    }
}
