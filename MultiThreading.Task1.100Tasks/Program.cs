/*
 * 1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.
 * Each Task should iterate from 1 to 1000 and print into the console the following string:
 * “Task #0 – {iteration number}”.
 */
using System;
using System.Threading.Tasks;

namespace MultiThreading.Task1._100Tasks
{
    class Program
    {
        const int TaskAmount = 100;
        const int MaxIterationsCount = 1000;

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. Multi threading V1.");
            Console.WriteLine("1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.");
            Console.WriteLine("Each Task should iterate from 1 to 1000 and print into the console the following string:");
            Console.WriteLine("“Task #0 – {iteration number}”.");
            Console.WriteLine();
            
            //Using new Task and Start methods
            //HundredTasks();

            //Using TaskFactory.StartNew()
            HundredTasks2();

            Console.ReadLine();
        }

        static void HundredTasks()
        {
            Task[] taskArray = new Task[TaskAmount];

            for (int i = 0; i < taskArray.Length; i++)
            {
                int taskIndex = i; 
                taskArray[i] = new Task(() => Print1000(taskIndex));
                taskArray[i].Start();
            }
            Task.WaitAll(taskArray);
            Console.WriteLine("END OF TASK");
            Console.ReadLine();
        }

                static void HundredTasks2()
        {
            Task[] taskArray = new Task[TaskAmount];

            for (int i = 0; i < taskArray.Length; i++)
            {
                int taskIndex = i; 
                taskArray[i] = Task.Factory.StartNew(() => Print1000(taskIndex));
            }
            Task.WaitAll(taskArray);
            Console.WriteLine("END OF TASK");
            Console.ReadLine();
        }

        static void Print1000(int taskID)
        {
            for (int i = 1; i <= 1000; i++)
            {
                Console.WriteLine("Task {0} – {1}.", taskID, i);
            }
        }

        static void Output(int taskNumber, int iterationNumber)
        {
            Console.WriteLine($"Task #{taskNumber} – {iterationNumber}");
        }
    }
}
