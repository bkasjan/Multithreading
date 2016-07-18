using System;
using System.Linq;
using System.Threading.Tasks;

namespace MergeSort
{
    class Program
    {
        static void Main(string[] args)
        {
            var rand = new Random();
            var mergeSorts = new MergeSorts();

            for (int i = 1000; i < 10000000; i *= 10)
            {

                Console.WriteLine($"Collection size: {i}");
                int[] randomTab = Enumerable.Repeat(0, i).Select(x => rand.Next(0, 1000)).ToArray();
                int[] randTabMulti = (int[])randomTab.Clone();

                // Single thread
                var startSingle = DateTime.Now;
                //var sortedSingle = MergeSort<int>(randomTab, 0, randomTab.Length);
                var sortedSingle = mergeSorts.MergeSort(randomTab, 0, randomTab.Length - 1);
                var endSingle = DateTime.Now;
                Console.WriteLine($"Single thread MergeSort: {endSingle - startSingle}");

                // Multi-thread
                var startMulti = DateTime.Now;
                var sortedMulti = mergeSorts.ParallelMergeSort<int>(randTabMulti, 0, randTabMulti.Length);
                var endMulti = DateTime.Now;
                Console.WriteLine($"Multi-thread MergeSort: {endMulti - startMulti}");
            }

            Console.ReadKey();
        }
    }
    public class MergeSorts
    {
        public T[] MergeSort<T>(T[] input, int startIndex, int endIndex) where T : IComparable<T>
        {
            if (endIndex - startIndex < 2)
                return new T[] { input[startIndex] };

            var middle = (endIndex + startIndex) / 2;

            var left = MergeSort(input, startIndex, middle);
            var right = MergeSort(input, middle, endIndex);
            var result = Merge(left, right);

            return result;
        }

        public T[] ParallelMergeSort<T>(T[] input, int startIndex, int endIndex) where T : IComparable<T>
        {
            if (endIndex - startIndex < 2)
                return new T[] { input[startIndex] };

            var middle = (endIndex + startIndex) / 2;

            var taskLeft = Task<T[]>.Factory.StartNew(() => ParallelMergeSort(input, startIndex, middle));
            var taskRight = Task<T[]>.Factory.StartNew(() => ParallelMergeSort(input, middle, endIndex));
            T[] leftResult = taskLeft.Result;
            T[] rightResult = taskRight.Result;

            var result = Merge(leftResult, rightResult);
            return result;
        }

        private T[] Merge<T>(T[] left, T[] right) where T : IComparable<T>
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

            while (rightIndex < rightSize)
                result[resultIndex++] = right[rightIndex++];

            while (leftIndex < leftSize)
                result[resultIndex++] = left[leftIndex++];

            return result;
        }
    }
}