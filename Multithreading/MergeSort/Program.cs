using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeSort
{
    public class MergeSort
    {

    }

    public class ParallelMergeSort
    {

    }

    class Program
    {
        static void Main(string[] args)
        {
            var rand = new Random();
            for (int i = 10; i < 1000; i*=10)
            {
                int[] randomTab = Enumerable.Repeat(0, 1000 * i).Select(x => rand.Next(0, 1000)).ToArray();

                // Single thread
                var startSingle = DateTime.Now;
                var sortedSingle = MergeSort< int>(randomTab, 0, randomTab.Length);
                var endSingle = DateTime.Now;

                // Multi-thread
                var startMulti = DateTime.Now;
                var sortedMulti = ParallelMergeSort<int>(randomTab, 0, randomTab.Length);
                var endMulti = DateTime.Now;

                Console.WriteLine($"Collection size: {1000 * i}");
                Console.WriteLine($"Single thread MergeSort: {endSingle - startSingle}");
                Console.WriteLine($"Multi-thread MergeSort: {endMulti - startMulti}");

                //foreach (var item in sortedMulti)
                //{
                //    Console.WriteLine($"{item} ");
                //}
            }

            Console.ReadKey();
        }

        static T[] MergeSort<T>(T[] input, int startIndex, int endIndex) where T : IComparable<T>
        {
            if (endIndex - startIndex < 2)
                return new T[] { input[startIndex] };

            var middle = (endIndex + startIndex) / 2;

            var left = MergeSort(input, startIndex, middle);
            var right = MergeSort(input, middle, endIndex);
            var result = Merge(left, right);

            return result;
        }

        static T[] ParallelMergeSort<T>(T[] input, int startIndex, int endIndex) where T : IComparable<T>
        {
            if (endIndex - startIndex < 2)
                return new T[] { input[startIndex] };

            var middle = (endIndex + startIndex) / 2;

            var taskLeft = Task<T[]>.Factory.StartNew(() => ParallelMergeSort(input, startIndex, middle));
            var taskRight = Task<T[]>.Factory.StartNew(() => ParallelMergeSort(input, middle, endIndex));
            T[] leftResult = taskLeft.Result;
            T[] rightResult = taskRight.Result;

            //var result = Task<T[]>.Factory.StartNew(() => { return Merge(left, right); }).Result;
            var result = Merge(leftResult, rightResult);
            return result;
        }

        static T[] Merge<T>(T[] left, T[] right) where T : IComparable<T>
        {
            var leftIndex = 0;
            var rightIndex = 0;
            var resultIndex = 0;
            var leftSize = left.Length;
            var rightSize = right.Length;
            T[] result = new T[leftSize + rightSize];

            while (rightIndex < rightSize && leftIndex < leftSize)
            {
                if (left[leftIndex].CompareTo(right[rightIndex]) < 0)
                {
                    result[resultIndex++] = left[leftIndex++];
                }
                else
                {
                    result[resultIndex++] = right[rightIndex++];
                }
            }

            var taskLeft = Task.Factory.StartNew(() =>
            {
                while (rightIndex < rightSize)
                {
                    result[resultIndex++] = right[rightIndex++];
                }                 
            });

            var taskRight = Task.Factory.StartNew(() =>
            {
                while (leftIndex < leftSize)
                {
                    result[resultIndex++] = left[leftIndex++];
                }
            });

            Task.WaitAll(taskLeft, taskRight);
            return result;
        }
    }
}
