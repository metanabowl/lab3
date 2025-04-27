# Laboratorium 3 - Obliczenia wielowątkowe w technologii .NET

## Informacje ogólne
- **Przedmiot:** Platformy Programistyczne .NET i Java
- **Prowadzący:** mgr inż. Michał Jaroszczuk
- **Temat:** Przyspieszenie mnożenia macierzy z użyciem wielowątkowości
- **Technologia:** .NET 8.0, C#
- **Cel:** Porównanie czasów mnożenia macierzy przy wykorzystaniu podejścia sekwencyjnego oraz wielowątkowego (`Parallel.For`)

---

## Opis projektu

Program umożliwia:
- Generowanie dwóch losowych macierzy o zadanym rozmiarze
- Mnożenie macierzy:
  - w sposób **sekwencyjny** (jeden wątek),
  - w sposób **równoległy** z określoną liczbą wątków.
- Pomiar czasu wykonania obu metod
- Porównanie wyników oraz sprawdzenie poprawności poprzez porównanie macierzy wynikowych.

---

## Struktura projektu

- **Program.cs** – obsługuje wczytywanie danych od użytkownika, wywołanie metod oraz prezentację wyników.
- **MatrixMultiplier.cs** – zawiera implementację logiki generowania macierzy, ich mnożenia (sekwencyjnego i równoległego) oraz porównywania wyników.

---

## Uruchomienie programu

Po uruchomieniu aplikacji użytkownik zostaje poproszony o:
1. Podanie rozmiaru macierzy (`size`).
2. Podanie liczby wątków (`threads`).

Program następnie:
- Generuje dwie losowe macierze `A` i `B` o rozmiarze `size x size`.
- Wykonuje mnożenie macierzy:
  - Sekwencyjnie (`MultiplySequential`).
  - Równolegle (`MultiplyParallel`) przy zadanej liczbie wątków.
- Wyświetla:
  - Czas mnożenia sekwencyjnego
  - Czas mnożenia równoległego
  - Informację czy wyniki obu metod są identyczne.

