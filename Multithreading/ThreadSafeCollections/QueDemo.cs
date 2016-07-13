using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSafeCollections
{
    /// <summary>
    /// Ilustrates some operations on Que and ConcurrentQue
    /// </summary>
    public class QueDemo
    {
        Queue<int> _que;
        ConcurrentQueue<int> _queConcurrent;
        Random _rand = new Random();

        /// <summary>
        /// Feed queues with sample data
        /// </summary>
        public QueDemo()
        {
            _que = new Queue<int>(Enumerable.Range(1,10));
            _queConcurrent = new ConcurrentQueue<int>(Enumerable.Range(1, 10));
        }

        /// <summary>
        /// Process reqular que, without lock can throw and exception
        /// </summary>
        public void ProcessQueue()
        {
            try
            {
                lock (_que)
                {
                    if (_que.Count > 0)
                    {
                        Thread.Sleep(_rand.Next(100, 1000));
                        Console.WriteLine($"Item taken: {_que.Dequeue()}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception thrown: {ex.Message}");
            }
        }

        /// <summary>
        /// Process concurrentQue
        /// </summary>
        public void ProcessConcurrentQueue()
        {
            try
            {
                Thread.Sleep(_rand.Next(100, 1000));

                int result;
                if (_queConcurrent.TryDequeue(out result))
                {               
                    Console.WriteLine($"Item taken: {result}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception thrown: {ex.Message}");
            }
        }
    }
}
