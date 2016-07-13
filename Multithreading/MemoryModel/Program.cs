using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MemoryModel
{
    /// <summary>
    /// Demonstrate: Memory model in multithreaded environment (synchronization, volatile keyword)
    /// </summary>
    class Program
    {
        private static volatile bool _workDone = false;
        private static object _listLock = new object();

        static void Main()
        {
            //BasicSyncDemo();

            //VolatileDemo();

            //Locks Demo
            var myWallet = new Wallet(100);
            Thread t1 = new Thread(() => myWallet.SpendCash(50));
            Thread t2 = new Thread(() => myWallet.SpendCash(40));

            t1.Start();
            t2.Start();
            myWallet.SpendCash(30);

            Console.WriteLine($"Cash left: {myWallet.CashLeft}");

            Console.ReadKey();
        }

        /// <summary>
        /// Usage of volatile. Will hang if workDone is not volatile
        /// </summary>
        static void VolatileDemo()
        {
            var thread = new Thread(() =>
            {
                Console.WriteLine("Begin thread");
                bool flag = false;
                while (!_workDone)
                {
                    // Do some actions, calc, read etc.
                    flag = !flag;
                }
                Console.WriteLine("End of thread");
            });
            thread.Start();

            Thread.Sleep(1000);
            _workDone = true;
            Console.WriteLine("Set _workDone = true");

            thread.Join();
        }

        /// <summary>
        /// Sync demo. Wait for 1st thread
        /// </summary>
        static void BasicSyncDemo()
        {
            var thread1 = new Thread(() => 
            {
                // Do some actions, calc, read etc.
                Thread.Sleep(2000);
                Console.WriteLine("1st thread");
            });
            var thread2 = new Thread(() =>
            {
                // Do some actions, calc, read etc.
                for (int i = 0; i < 100; i++)
                    Console.WriteLine($"2nd thread: {i}");
                Thread.Sleep(500);
            });
            thread1.Start();
            thread1.Join();
            thread2.Start();
        }
    }
}
