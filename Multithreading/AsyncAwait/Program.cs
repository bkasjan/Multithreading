using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    /// <summary>
    /// Demonstrate: async/await usage
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var task = new Task(ProcessCalculation);
            task.Start();
            

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Main thread doing some actions");
                Thread.Sleep(1000);
            }

            task.Wait();

            Console.WriteLine($"Done");
            Console.ReadKey();
        }

        static async void ProcessCalculation()
        {
            var rand = new Random();
            int[] array = Enumerable
                .Repeat(0, 1000000)
                .Select(i => rand.Next(0, 1000))
                .ToArray();

            Task<int> task = GetMaxMinSum(array);

            // While is calculation let's do sth else
            Console.WriteLine("Calculation is in process");
            var otherCalc = AdditionalQuickCalculation();

            // No we need total result to do some other math, so let's wait
            int result = await task;
            var finalResult = result + otherCalc;
            Console.WriteLine($"Final result is {finalResult}");
        }

        /// <summary>
        /// Fakes additional, quick operations
        /// </summary>
        /// <returns></returns>
        static int AdditionalQuickCalculation()
        {
            Console.WriteLine("Doing additional operations");
            Thread.Sleep(1000);
            var rand = new Random();
            return rand.Next(0, 1000);
        }

        /// <summary>
        /// Return max+min of the table
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        static async Task<int> GetMaxMinSum(int[] array)
        {
            return await Task.Run(() =>
            {
                Array.Sort(array);
                // Slow it down
                Thread.Sleep(2000);
                // No data validation like empty array. Just showing the idea
                return array[0] + array[array.Length - 1];
            });
        }
    }
}