using System;
using System.Diagnostics;

namespace lab3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Podaj rozmiar macierzy: ");
            int size = int.Parse(Console.ReadLine());

            Console.Write("Podaj ilość wątków: ");
            int threads = int.Parse(Console.ReadLine());

            Console.WriteLine($"\nMnożenie macierzy {size}x{size} - liczba wątków: {threads}");

            double[,] A = MatrixMultiplier.GenerateMatrix(size);
            double[,] B = MatrixMultiplier.GenerateMatrix(size);
            double[,] resultSeq = new double[size, size];
            double[,] resultPar = new double[size, size];

            var sw = Stopwatch.StartNew();
            MatrixMultiplier.MultiplySequential(A, B, resultSeq, size);
            sw.Stop();
            Console.WriteLine($"Czas (sekwencyjnie): {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            MatrixMultiplier.MultiplyParallel(A, B, resultPar, size, threads);
            sw.Stop();
            Console.WriteLine($"Czas (równolegle): {sw.ElapsedMilliseconds} ms");

            bool areEqual = MatrixMultiplier.CompareMatrices(resultSeq, resultPar);
            Console.WriteLine($"Macierze wynikowe są identyczne? {(areEqual ? "Tak" : "Nie")}");
        }
    }
}
