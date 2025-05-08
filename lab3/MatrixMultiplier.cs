using System;
using System.Threading; 
using System.Threading.Tasks; 

namespace lab3
{

    internal class MatrixMultiplier
    {
        // Generuje kwadratową macierz o podanym rozmiarze wypełnioną losowymi wartościami double.
        public static double[,] GenerateMatrix(int size)
        {
            var rand = new Random();
            var matrix = new double[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    matrix[i, j] = rand.NextDouble() * 10;
            return matrix;
        }

        // Mnoży dwie macierze kwadratowe (A i B) sekwencyjnie i zapisuje wynik do macierzy 'result'.
        public static void MultiplySequential(double[,] A, double[,] B, double[,] result, int size)
        {
            // Pętla po wierszach macierzy wynikowej
            for (int i = 0; i < size; i++)
                // Pętla po kolumnach macierzy wynikowej
                for (int j = 0; j < size; j++)
                {
                    double sum = 0;
                    // Pętla po elementach do sumowania (kolumny A i wiersze B)
                    for (int k = 0; k < size; k++)
                        sum += A[i, k] * B[k, j]; // C[i, j] = sum(A[i, k] * B[k, j]) dla k od 0 do size-1
                    result[i, j] = sum;
                }
        }

        // Mnoży dwie macierze kwadratowe (A i B) równolegle przy użyciu Parallel.Fori zapisuje wynik do macierzy 'result'. Równoległość dotyczy zewnętrznej pętli (wierszy).
        public static void MultiplyParallel(double[,] A, double[,] B, double[,] result, int size, int maxThreads)
        {
            // Ustawienie opcji równoległości z maksymalną liczbą wątków
            var options = new ParallelOptions() { MaxDegreeOfParallelism = maxThreads };

            // Parallel.For równolegla zewnętrzną pętlę (po wierszach 'i')
            Parallel.For(0, size, options, i =>
            {
                // Wewnętrzne pętle (po kolumnach 'j' i elementach sumy 'k')
                for (int j = 0; j < size; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < size; k++)
                        sum += A[i, k] * B[k, j];
                    result[i, j] = sum;
                }
            });
        }

        // Mnoży dwie macierze kwadratowe (A i B) równolegle przy użyciu manualnie tworzonych wątków i zapisuje wynik do macierzy 'result'. Dzieli wiersze macierzy wynikowej między wątki.
        public static void MultiplyThreaded(double[,] A, double[,] B, double[,] result, int size, int threadCount)
        {
            // Tablica do przechowywania referencji do wątków
            Thread[] threads = new Thread[threadCount];
            // Obliczanie liczby wierszy do przetworzenia przez każdy wątek
            int rowsPerThread = size / threadCount;

            // Tworzenie i uruchamianie wątków
            for (int t = 0; t < threadCount; t++)
            {
                // Określenie zakresu wierszy dla bieżącego wątku
                int startRow = t * rowsPerThread;
                // Ostatni wątek przetwarza pozostałe wiersze (obsługa przypadku, gdy size nie jest podzielne przez threadCount)
                int endRow = (t == threadCount - 1) ? size : startRow + rowsPerThread;

                // Tworzenie nowego wątku z lambdą zawierającą logikę mnożenia dla danego zakresu wierszy
                threads[t] = new Thread(() =>
                {
                    // Pętla po wierszach przypisanych do tego wątku
                    for (int i = startRow; i < endRow; i++)
                    {
                        // Wewnętrzne pętle po kolumnach 'j' i elementach sumy 'k'
                        for (int j = 0; j < size; j++)
                        {
                            double sum = 0;
                            for (int k = 0; k < size; k++)
                                sum += A[i, k] * B[k, j];
                            result[i, j] = sum;
                        }
                    }
                });
                threads[t].Start(); // Uruchomienie wątku
            }

            // Czekanie na zakończenie wszystkich wątków
            foreach (Thread thread in threads)
                thread.Join();
        }

        // Porównuje dwie macierzy kwadratowe, sprawdzając, czy ich elementy są prawie równe, z uwzględnieniem tolerancji dla błędów zmiennoprzecinkowych.
        public static bool CompareMatrices(double[,] m1, double[,] m2, double tolerance = 1e-6)
        {
            // Sprawdzenie, czy macierze mają ten sam rozmiar
            if (m1.GetLength(0) != m2.GetLength(0) || m1.GetLength(1) != m2.GetLength(1))
            {
                return false;
            }

            int size = m1.GetLength(0);
            // Porównywanie elementów macierzy
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    // Sprawdzenie bezwzględnej wartości różnicy. Jeśli jest większa niż tolerancja, macierze są różne.
                    if (Math.Abs(m1[i, j] - m2[i, j]) > tolerance)
                        return false; // Znaleziono znaczącą różnicę

            return true; // Wszystkie elementy są w granicach tolerancji
        }
    }
}