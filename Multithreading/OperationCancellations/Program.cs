using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OperationCancellations
{
    /// <summary>
    /// Demonstrate: Cancelations and continuations
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Cancelations using CancellationToken and continuation
            var cancelSource = new CancellationTokenSource();
            var cancelToken = cancelSource.Token;

            var task1 = Task.Factory.StartNew(() =>
            {
                DoSomeAction(cancelToken);
            });

            // Continuation task of task1
            var task2 = task1.ContinueWith(prev =>
            {
                Console.WriteLine("Let's continue after 1st task");
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine("Doing additional work");
                    Thread.Sleep(100);
                }
            });

            cancelSource.Cancel();

            try
            {
                task1.Wait();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException is OperationCanceledException)
                    Console.WriteLine("Task action was cancelled");
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Some actions to do. In the middle checking if task is being cancelled.
        /// </summary>
        /// <param name="cancelToken"></param>
        static void DoSomeAction(CancellationToken cancelToken)
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Working");
                Thread.Sleep(1000);
            }

            cancelToken.ThrowIfCancellationRequested();

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Doing rest of the work");
                Thread.Sleep(1000);
            }
        }
    }
}
