using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryModel
{
    public class Wallet
    {
        private object _walletLocker = new object();
        private decimal _cashLeft;

        public decimal CashLeft
        {
            get
            {
                return _cashLeft;
            }
        }

        public Wallet(decimal cashAtStart)
        {
            _cashLeft = cashAtStart;
        }

        /// <summary>
        /// Decrease amount of money in the wallet
        /// </summary>
        /// <param name="cashToSpended"></param>
        public void SpendCash(decimal cashToSpended)
        {
            lock (_walletLocker)
            {
                if (_cashLeft >= cashToSpended)
                    _cashLeft -= cashToSpended;
                else Console.WriteLine("Not enough cash in your wallet");
            }
        }
    }
}
