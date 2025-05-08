using System;
using System.Diagnostics;
using System.IO; 
using System.Threading; 

namespace lab3
{
    internal class Program
    {

        static void Main(string[] args)
        {

            int matrixSize;
            int numThreads;

            // Pobieranie rozmiaru macierzy
            while (true)
            {
                Console.Write("Podaj rozmiar macierzy: ");
                string sizeInput = Console.ReadLine();
                if (int.TryParse(sizeInput, out matrixSize) && matrixSize > 0)
                {
                    break; // Poprawne dane, wyjście z pętli
                }
                Console.WriteLine("Nieprawidłowy rozmiar. Podaj dodatnią liczbę całkowitą.");
            }

            // Pobieranie liczby wątków
            while (true)
            {
                Console.Write("Podaj liczbę wątków: ");
                string threadsInput = Console.ReadLine();
                if (int.TryParse(threadsInput, out numThreads) && numThreads > 0)
                {
                    break; // Poprawne dane, wyjście z pętli
                }
                Console.WriteLine("Nieprawidłowa liczba wątków. Podaj dodatnią liczbę całkowitą.");
            }

            double[,] A = MatrixMultiplier.GenerateMatrix(matrixSize);
            double[,] B = MatrixMultiplier.GenerateMatrix(matrixSize);

            // Wykonanie mnożenia sekwencyjnego jako punktu odniesienia
            double[,] resultSeq = new double[matrixSize, matrixSize];
            var swSeq = Stopwatch.StartNew();
            MatrixMultiplier.MultiplySequential(A, B, resultSeq, matrixSize);
            swSeq.Stop();
            Console.WriteLine($"Sekwencyjne zakończone. Czas: {swSeq.ElapsedMilliseconds} ms.");

            // Wykonanie mnożenia równoległego (wysokopoziomowego - Parallel.For)
            double[,] resultParHigh = new double[matrixSize, matrixSize];
            var swHigh = Stopwatch.StartNew();
            MatrixMultiplier.MultiplyParallel(A, B, resultParHigh, matrixSize, numThreads);
            swHigh.Stop();
            bool correctHigh = MatrixMultiplier.CompareMatrices(resultSeq, resultParHigh);
            Console.WriteLine($"Parallel.For zakończone. Czas: {swHigh.ElapsedMilliseconds} ms, Poprawność: {correctHigh}.");

            // Wykonanie mnożenia równoległego (niskopoziomowego - manualne wątki)
            double[,] resultParLow = new double[matrixSize, matrixSize];
            var swLow = Stopwatch.StartNew();
            MatrixMultiplier.MultiplyThreaded(A, B, resultParLow, matrixSize, numThreads);
            swLow.Stop();
            bool correctLow = MatrixMultiplier.CompareMatrices(resultSeq, resultParLow);
            Console.WriteLine($"Manualne wątki zakończone. Czas: {swLow.ElapsedMilliseconds} ms, Poprawność: {correctLow}.");


            // Część do testów wydajności
            /*
            int[] sizes = { 200, 500, 1000, 2000 };
            int[] threadsList = { 1, 2, 4, 8, 12, 24 };
            int repetitions = 3;
            string fileName = "wyniki.csv";

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("Rozmiar,Metoda,Wątki,ŚredniCzas(ms),Poprawność");

                foreach (int size in sizes)
                {
                    // Generowanie macierzy tylko raz dla danego rozmiaru
                    double[,] A = MatrixMultiplier.GenerateMatrix(size);
                    double[,] B = MatrixMultiplier.GenerateMatrix(size);

                    // Wykonanie mnożenia sekwencyjnego raz dla danego rozmiaru, aby uzyskać wynik odniesienia
                    double[,] resultSeq = new double[size, size];
                    MatrixMultiplier.MultiplySequential(A, B, resultSeq, size);

                    foreach (int threads in threadsList)
                    {
                        Console.WriteLine($"[{DateTime.Now}] Rozmiar: {size}, Wątki: {threads}");

                        double avgTimeHigh = 0, avgTimeLow = 0;
                        bool correctHigh = true, correctLow = true;

                        // Testowanie metody wysokopoziomowej (Parallel.For)
                        for (int i = 0; i < repetitions; i++)
                        {
                            double[,] resultPar = new double[size, size]; // Tworzenie nowej macierzy wynikowej dla każdego przebiegu
                            var sw = Stopwatch.StartNew();
                            MatrixMultiplier.MultiplyParallel(A, B, resultPar, size, threads);
                            sw.Stop();
                            avgTimeHigh += sw.ElapsedMilliseconds;
                            correctHigh &= MatrixMultiplier.CompareMatrices(resultSeq, resultPar); // Sprawdzanie poprawności wyniku
                        }
                        avgTimeHigh /= repetitions; // Obliczanie średniego czasu
                        writer.WriteLine($"{size},Wysokopoziomowa,{threads},{avgTimeHigh},{correctHigh}"); // Zapis do pliku

                        // Testowanie metody niskopoziomowej (manualne wątki)
                        for (int i = 0; i < repetitions; i++)
                        {
                            double[,] resultLow = new double[size, size]; // Tworzenie nowej macierzy wynikowej dla każdego przebiegu
                            var sw = Stopwatch.StartNew();
                            MatrixMultiplier.MultiplyThreaded(A, B, resultLow, size, threads);
                            sw.Stop();
                            avgTimeLow += sw.ElapsedMilliseconds;
                            correctLow &= MatrixMultiplier.CompareMatrices(resultSeq, resultLow); // Sprawdzanie poprawności wyniku
                        }
                        avgTimeLow /= repetitions; // Obliczanie średniego czasu
                        writer.WriteLine($"{size},Niskopoziomowa,{threads},{avgTimeLow},{correctLow}"); // Zapis do pliku

                        writer.Flush(); // Zapis bufora do pliku na wypadek awarii programu
                    }
                }
            }

            Console.WriteLine("\nZakończono testy. Wyniki zapisano do pliku: wyniki.csv");
        }*/
        }
    }
}
