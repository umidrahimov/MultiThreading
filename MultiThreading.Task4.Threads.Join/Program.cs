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
        private static CountdownEvent countdownEvent;

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
            Console.WriteLine("Implementation A: using Thread class and Join for waitin threads. \n");

            Thread thread = new Thread(() => DoWork(numOfThreads));
            thread.Start();
            thread.Join();

            //b) ThreadPool class for this task and Semaphore for waiting threads.
            Console.WriteLine("\nImplementation B: using ThreadPool class and Semaphore for waitin threads. \n");

            _pool = new Semaphore(initialCount: 0, maximumCount: 1);
            ThreadPool.QueueUserWorkItem(DoWork2, numOfThreads);
            _pool.WaitOne();
        }

        private static void DoWork(int input)
        {
            Console.WriteLine($"Thread ID #{Thread.CurrentThread.ManagedThreadId} started working on item #{input}");

            if (--input > 0)
            {
                Thread t = new Thread(() => DoWork(input));

                Console.WriteLine($"Thread ID #{Thread.CurrentThread.ManagedThreadId} decremented value and passed {input} to the next thread\n");

                t.Start();
                t.Join();
            }

            Console.WriteLine($"Thread ID #{Thread.CurrentThread.ManagedThreadId} completed working on item #{++input}\n");
        }

        private static void DoWork2(object input)
        {
            int num = (int)input;

            Console.WriteLine($"Thread with ID {Thread.CurrentThread.ManagedThreadId} started working on item #{num}");

            if (--num > 0)
            {
                ThreadPool.QueueUserWorkItem(DoWork2, num);

                Console.WriteLine($"Thread ID #{Thread.CurrentThread.ManagedThreadId} decremented value and passed {num} to the next thread");
            }
            else _pool.Release(1);

            Console.WriteLine($"Thread ID #{Thread.CurrentThread.ManagedThreadId} completed working on item #{num++}\n");
        }
    }
}
