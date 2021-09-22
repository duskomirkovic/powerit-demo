using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Primes
{
    class PrimeList : IPrimesGenerator
    {
        private List<int> Primes { get; } = new List<int>();
        private int LastChecked { get; set; } = 1;

        public static IPrimesGenerator Create() =>
            new PrimeList();

        public IEnumerable<int> GetAll()
        {
            foreach (int prime in this.Primes)
                yield return prime;

            while (this.LastChecked <= int.MaxValue - 1)
            {
                this.LastChecked += 1;
                if (this.IsPrime(this.LastChecked))
                {
                    this.Primes.Add(this.LastChecked);
                    yield return this.LastChecked;
                }
            }
        }

        private bool IsPrime(int value)
        {
            int maxDivisor = (int)Math.Sqrt(this.LastChecked);
            foreach (int prime in this.Primes)
            {
                if (prime > maxDivisor)
                    return true;
                if (value % prime == 0)
                    return false;
            }
            return true;
        }
    }
}
