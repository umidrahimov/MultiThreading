/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static Semaphore _pool;

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            int numOfThreads = 10;

            // a) Use Thread class for this task and Join for waiting threads.
            Thread thread = new Thread(() => DoWork(numOfThreads));
            thread.Start();
            thread.Join();

            //b) ThreadPool class for this task and Semaphore for waiting threads.
            _pool = new Semaphore(initialCount: 1, maximumCount: 1);
            ThreadPool.QueueUserWorkItem(DoWork2, numOfThreads);
            //_pool.WaitOne();
            Console.ReadLine();
        }

        private static void DoWork(int input)
        {
            Console.WriteLine(input);
            input--;

            if (input > 0)
            {
                Thread t = new Thread(() => DoWork(input));
                t.Start();
                t.Join();
            }
        }

        private static void DoWork2(object input)
        {
            int num = (int)input;
            System.Console.WriteLine($"Thread {Thread.CurrentThread.Name} with #{num} is waiting for Semaphore ");
            _pool.WaitOne();
            System.Console.WriteLine($"Thread {Thread.CurrentThread.Name} with #{input} started work");
            Console.WriteLine(num);
            num--;
            if (num > 0)
            {
                ThreadPool.QueueUserWorkItem(DoWork2, num);
            }
            _pool.Release(1);
            System.Console.WriteLine($"Thread {Thread.CurrentThread.Name} with #{input} completed work");
        }
    }
}
