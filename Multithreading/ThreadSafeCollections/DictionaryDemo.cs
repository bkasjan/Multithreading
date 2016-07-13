using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSafeCollections
{
    public class DictionaryDemo
    {
        ConcurrentDictionary<string, decimal> carPricesConcurrent = new ConcurrentDictionary<string, decimal>();
        Dictionary<string, decimal> carPrices = new Dictionary<string, decimal>();

        /// <summary>
        /// Adds and updates concurrent dictionary with sample data
        /// Without additional checks and conditions Tasks can fail to update/add entires
        /// </summary>
        public void FeedDictionaryByTasks()
        {
            Task[] tasks = new Task[3];

            tasks[0] = Task.Run(() =>
            {
                try
                {
                    carPrices.Add("Fiat Panda", 40000);
                    Console.WriteLine($"Added Fiat Panda, thread {Thread.CurrentThread.ManagedThreadId}");

                    carPrices.Add("Skoda Octavia", 66000);
                    Console.WriteLine($"Added Skoda Octavia, thread {Thread.CurrentThread.ManagedThreadId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            });

            tasks[1] = Task.Run(() =>
            {
                try
                {
                    carPrices.Add("Kia Sportage", 70000);
                    Console.WriteLine($"Added Kia Sportage, thread {Thread.CurrentThread.ManagedThreadId}");

                    carPrices.Add("Skoda Octavia", 68000);
                    Console.WriteLine($"Added Skoda Octavia, thread {Thread.CurrentThread.ManagedThreadId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            });

            tasks[2] = Task.Run(() =>
            {
                try
                {
                    carPrices["Kia Sportage"] = 20000;
                    Console.WriteLine("Price of Kia Sportage was updated");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });

            Task.WaitAll(tasks);

            foreach (var car in carPrices)
            {
                Console.WriteLine($"{car.Key} costs {car.Value}");
            }
        }

        /// <summary>
        /// Adds and updates concurrent dictionary with sample data
        /// </summary>
        public void FeedConcurrentDictionaryByTasks()
        {
            Task[] tasks = new Task[3];

            tasks[0] = Task.Run(() =>
            {
                if (carPricesConcurrent.TryAdd("Fiat Panda", 40000))
                    Console.WriteLine($"Added Fiat Panda, thread {Thread.CurrentThread.ManagedThreadId}");
                else
                    Console.WriteLine("Could not add Fiat Panda");

                if (carPricesConcurrent.TryAdd("Skoda Octavia", 66000))
                    Console.WriteLine($"Added Skoda Octavia, thread {Thread.CurrentThread.ManagedThreadId}");
                else
                    Console.WriteLine("Could not add Skoda Octavia");
            });

            tasks[1] = Task.Run(() =>
            {
                if (carPricesConcurrent.TryAdd("Kia Sportage", 70000))
                    Console.WriteLine($"Added Kia Sportage, thread {Thread.CurrentThread.ManagedThreadId}");
                else
                    Console.WriteLine("Could not add Kia Sportage");

                if (carPricesConcurrent.TryAdd("Skoda Octavia", 68000))
                    Console.WriteLine($"Added Skoda Octavia, thread {Thread.CurrentThread.ManagedThreadId}");
                else
                    Console.WriteLine("Could not add Skoda Octavia");
            });

            tasks[2] = Task.Run(() =>
            {
                if (carPricesConcurrent.TryUpdate("Kia Sportage", 70000, 20000))
                {
                    Console.WriteLine("Price of Kia Sportage was updated");
                }
                else
                {
                    Console.WriteLine("Price of Kia Sportage was not updated");
                }
            });

            Task.WaitAll(tasks);

            foreach (var car in carPricesConcurrent)
            {
                Console.WriteLine($"{car.Key} costs {car.Value}");
            }
        }
    }
}