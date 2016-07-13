using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPooling
{
    /// <summary>
    /// Demonstrate: ThreadPools usage
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            ulong n = 40;

            // Using Task Factory
            Task<ulong> task1 = Task.Factory.StartNew<ulong>
              (() => FibonacciTailRec(n));

            Task<ulong> task2 = Task.Factory.StartNew<ulong>
              (() => FibonacciRec(n));

            DoSomeAdditionalActions(1000);

            // Let's wait for the result and print it
            ulong result = task1.Result;
            Console.WriteLine($"1st tasks finished. Fib(n={n}) = {result}");

            Console.WriteLine($"Status of 1st task: {task1.Status}");
            Console.WriteLine($"Status of 2nd task: {task2.Status}");

            ulong result2 = task2.Result;
            Console.WriteLine($"2nd tasks finished. Fib(n={n}) = {result2}");

            // Using ThreadPool
            // Limit the thread pool active request to present queued threads
            ThreadPool.SetMaxThreads(5, 5);
            for (int i = 0; i < 20; i++)
            {
                ThreadPool.QueueUserWorkItem(delegate { DoSomeAdditionalActions(5000); });
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Calculates n'th Fibbonaci number. Recursive, slow.
        /// </summary>
        static ulong FibonacciRec(ulong number)
        {
            if ((number == 0) || (number == 1))
            {
                return number;
            }
            else
                return FibonacciRec(number - 1) + FibonacciRec(number - 2);
        }

        /// <summary>
        /// Calculates n'th Fibbonaci number. Tail recursive, fast.
        /// </summary>
        static ulong FibonacciTailRec(ulong n, ulong value = 1, ulong previous = 0)
        {
            if (n == 0) return previous;
            if (n == 1) return value;
            return FibonacciTailRec(n - 1, value + previous, value);
        }

        /// <summary>
        /// Pretends some time consuming operations
        /// </summary>
        /// <param name="miliSec">Miliseconds for thread sleep</param>
        static void DoSomeAdditionalActions(int miliSec)
        {
            // Do sth
            Thread.Sleep(miliSec);
            Console.WriteLine($"Some additioal actions: {miliSec} milisec.");
        }
    }
}
