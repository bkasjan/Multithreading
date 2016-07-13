using System;
using System.Threading.Tasks;

namespace ThreadSafeCollections
{
    class Program
    {
        static void Main(string[] args)
        {
            // Dictionary and ConcurrentDictionary comparision
            var dic = new DictionaryDemo();
            dic.FeedConcurrentDictionaryByTasks();
            dic.FeedDictionaryByTasks();

            // Processing the queues
            Console.WriteLine($"{Environment.NewLine}Processing the queues");


            var queDemo = new QueDemo();
            // Processing regular que, if not locked will thow an exception
            var tasks = new Task[3];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 5; j++)
                        queDemo.ProcessQueue();
                });
            }

            Task.WaitAll(tasks);

            // Processing concurrentQue with try dequeue
            var tasksConcurrent = new Task[3];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasksConcurrent[i] = Task.Run(() =>
                {
                    for (int j = 0; j < 5; j++)
                        queDemo.ProcessConcurrentQueue();
                });
            }

            Task.WaitAll(tasks);
            Console.ReadKey();
        }
    }
}
