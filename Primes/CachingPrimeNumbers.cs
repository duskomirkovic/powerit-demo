using System.Collections.Generic;
using System.Linq;

namespace Demo.Primes
{
    class CachingPrimeNumbers : IPrimesGenerator
    {
        private List<int> Primes { get; } = new List<int>() {2, 3};
        public int LastChecked => this.Primes[^1];
        public IEnumerable<int> Cached => this.Primes;

        public static IPrimesGenerator Create() =>
            new CachingPrimeNumbers();

        public IEnumerable<int> GetAll() =>
            this.Primes.Concat(this.Primes.AppendThrough(this.GetExtension()));

        private IEnumerable<int> GetExtension() =>
            PrimeNumbers.GetAllPrimes(this.LastChecked, this.Primes);

        public IEnumerable<int> GetExtension(int greaterThan, int range) =>
            PrimeNumbers.GetAllPrimes(greaterThan, range, this.Primes);

        public IEnumerable<int> WriteThrough(IEnumerable<int> newPrimes) =>
            this.Primes.AppendThrough(newPrimes);
    }
}