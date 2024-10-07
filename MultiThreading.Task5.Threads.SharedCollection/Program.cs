/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static List<String> collection;
        static object _lock = new object();
        static AutoResetEvent newItemAddedEvent = new AutoResetEvent(false);
        static AutoResetEvent newItemPrintedEvent = new AutoResetEvent(true);

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            // feel free to add your code
            int collectionSize = 10;
            collection = new List<string>(collectionSize);
            Task writerTask = Task.Run(() => WriteToCollection(collectionSize));
            Task readerTask = Task.Run(() => PrintCollection(10));

            Task.WaitAll(writerTask, readerTask);

            Console.ReadLine();
        }

        private static void WriteToCollection(int count)
        {
            for (int i = 0; i < count; i++)
            {
                newItemPrintedEvent.WaitOne();
                collection.Add($"Element {i}");
                Console.WriteLine($"Writer task #{Thread.CurrentThread.ManagedThreadId} added element {i} to the collection.");
                newItemAddedEvent.Set();
            }
        }
        private static void PrintCollection(int count)
        {
            for (int i = 0; i < count; i++)
            {
                newItemAddedEvent.WaitOne();
                Console.WriteLine($"Reader task #{Thread.CurrentThread.ManagedThreadId} printed collection itme {i}: {collection[i]}\n");
                newItemPrintedEvent.Set();
            }
        }
    }
}
