/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            CancellationToken cancellationToken = cancellationTokenSource.Token;

            Task parentTask = Task.Run(() => DoSomeWork(cancellationToken), cancellationToken);

            //Task A: Continuation task should be executed regardless of the result of the parent task.
            Task continuationTaskA = parentTask.ContinueWith((t) => ContinueSomeWork('A'));

            //Task B: Continuation task should be executed when the parent task finished without success.
            Task continuationTaskB = parentTask.ContinueWith((t) => ContinueSomeWork('B'), TaskContinuationOptions.OnlyOnFaulted);

            //Task C: Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.
            Task continuationTaskC = parentTask.ContinueWith((t) => ContinueSomeWork('C'), TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnFaulted);

            //Task D: Continuation task should be executed outside of the thread pool when the parent task would be cancelled.
            Task continuationTaskD = parentTask.ContinueWith((t) => ContinueSomeWork('D'), TaskContinuationOptions.LongRunning | TaskContinuationOptions.OnlyOnCanceled);

            Console.ReadLine();
        }

        private static void DoSomeWork(CancellationToken cancellation)
        {
            Console.WriteLine("Thread ID #{0} started working on main task.", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Parent task is running.");

            //Uncomment below line to cancel a task
            //cancellationTokenSource.Cancel();

            if (cancellation.IsCancellationRequested)
            {
                Console.WriteLine("Parent task was cancelled.");
                cancellation.ThrowIfCancellationRequested();
            }

            //Uncomment the below try-catch section if you want the parent task to fail.
            /*
            try
            {
                throw new Exception("Parent task has failed.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                throw;
            }
            */

            //Simulates successful task completion.
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Parent task completed successfully.");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void ContinueSomeWork(char subtask)
        {
            Console.WriteLine($"Thread ID #{Thread.CurrentThread.ManagedThreadId} started working on continuation task {subtask}.");
            Console.WriteLine("Continuation task {0} is running.", subtask);
            Thread.Sleep(1000); //Simulates some work
            Console.WriteLine("Continuation task {0} completed successfully.", subtask);
        }
    }
}
