using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderWriterLock
{
    /// <summary>
    /// Demonstrate: Reader-Writer lock pattern
    /// </summary>
    class Program
    {
        

        static void Main(string[] args)
        {
            var myGarage = new Garage();
            new Thread(() => myGarage.ListCarsInGarage("1st")).Start();
            new Thread(() => myGarage.ListCarsInGarage("2nd")).Start();

            new Thread(() => myGarage.AddCarToGarage("Skoda Octavia")).Start();
            new Thread(() => myGarage.AddCarToGarage("Fiat Punto")).Start();
            new Thread(() => myGarage.ListCarsInGarage("3rd")).Start();
            new Thread(() => myGarage.AddCarToGarage("Porsche 911")).Start();

            Console.ReadKey();
        }
    }

    /// <summary>
    /// Ilustraing garage with list of cars names inside
    /// </summary>
    public class Garage
    {
        private ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        private List<string> _carsNames = new List<string>();

        public Garage()
        {
            // Add some test data
            _carsNames.Add("Kia Ceed");
            _carsNames.Add("Skoda Fabia");
        }

        public void ListCarsInGarage(string threadNbr)
        {
            _readWriteLock.EnterReadLock();
            try
            {
                foreach (var carName in _carsNames)
                {
                    
                    Console.WriteLine($"{threadNbr} : {carName}");
                    Thread.Sleep(10);
                }
                Console.WriteLine($"{_readWriteLock.CurrentReadCount} Current Readers");
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }
        }

        public void AddCarToGarage(string carName)
        {
            _readWriteLock.EnterWriteLock();
            try
            {
                _carsNames.Add(carName);
                Console.WriteLine($"New car in your garage: {carName}");
                Thread.Sleep(100);
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }       
        }
    }
}
