
/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            //Approach 1
            try
            {
                Console.WriteLine("Starting Task 1...");
                Task<int[]> task1 = Task.Run(() => GetRandomNumbers(10));
                Console.WriteLine($"Task 1 executed with the result: [{string.Join(", ", task1.Result)}] \n");

                Console.WriteLine("Starting Task 2...");
                Task<int[]> task2 = task1.ContinueWith(antecedent => DoComputation(task1.Result));
                Console.WriteLine($"Task 2 executed with the result: [{string.Join(", ", task2.Result)}] \n");

                Console.WriteLine("Starting Task 3...");
                Task<int[]> task3 = task2.ContinueWith(antecedent => SortArray(task2.Result));
                Console.WriteLine($"Task 3 executed with the result: [{string.Join(", ", task3.Result)}] \n");

                Console.WriteLine("Starting Task 4...");
                Task<double> task4 = task3.ContinueWith(antecedent => FindAverage(task3.Result));
                Console.WriteLine($"Task 4 executed with the result: {task4.Result} \n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            //Approach 2
            /*
            try
            {
                Console.WriteLine("Starting Task 1...");
                int[] randomNumbers = await Task.Run(() => GetRandomNumbers(10));
                Console.WriteLine($"Task 1 executed with the result: [{string.Join(", ", randomNumbers)}] \n");

                Console.WriteLine("Starting Task 2...");
                int[] multipliedNumbers = await Task.Run(() => DoComputation(randomNumbers));
                Console.WriteLine($"Task 2 executed with the result: [{string.Join(", ", multipliedNumbers)}] \n");

                Console.WriteLine("Starting Task 3...");
                int[] sortedNumbers = await Task.Run(() => SortArray(multipliedNumbers));
                Console.WriteLine($"Task 3 executed with the result: [{string.Join(", ", sortedNumbers)}] \n");

                Console.WriteLine("Starting Task 4...");
                double averageValue = await Task.Run(() => FindAverage(sortedNumbers));
                Console.WriteLine($"Task 4 executed with the result: {averageValue} \n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            */

            Console.ReadLine();
        }

        private static Random random = new Random();

        private static int[] GetRandomNumbers(int count)
        {
            try
            {
                int[] result = new int[count];
                for (int i = 0; i < count; i++)
                {
                    result[i] = random.Next(1, 100);
                    Console.WriteLine($"Element #{i} got a random number {result[i]} assigned.");
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetRandomNumbers: {ex.Message}");
                throw;
            }
        }

        private static int[] DoComputation(int[] numbers)
        {
            try
            {
                int[] result = new int[numbers.Length];
                for (int i = 0; i < numbers.Length; i++)
                {
                    int tempRandom = random.Next(1, 100);
                    result[i] = numbers[i] * tempRandom;
                    Console.WriteLine($"Element #{i} {numbers[i]} multiplied by a random number {tempRandom} and resulted in {result[i]}.");
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DoComputation: {ex.Message}");
                throw;
            }
        }

        private static int[] SortArray(int[] numbers)
        {
            try
            {
                Array.Sort(numbers);
                return numbers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SortNumbers: {ex.Message}");
                throw;
            }
        }

        private static double FindAverage(int[] numbers)
        {
            try
            {
                return numbers.Average();
            }
            catch (Exception ex)
            {
            Console.WriteLine($"Error in FindAverage: {ex.Message}");
            throw;            }
        }
    }
}
