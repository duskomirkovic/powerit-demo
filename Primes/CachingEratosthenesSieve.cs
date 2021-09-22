using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Primes
{
    class CachingEratosthenesSieve : IPrimesGenerator
    {
        private List<int> Primes { get; }
        private int LastChecked { get; set; }
        private int MaxStep { get; }

        public CachingEratosthenesSieve(int maxStep)
        {
            this.Primes = new List<int>() { 2, 3 };
            this.LastChecked = 4;
            this.MaxStep = maxStep;
        }

        public CachingEratosthenesSieve() : this(16_000_000)
        {
        }

        public static IPrimesGenerator Create() =>
            new CachingEratosthenesSieve();

        public IEnumerable<int> GetAll()
        {
            int pos = 0;
            while (pos < this.Primes.Count)
            {
                yield return this.Primes[pos];
                pos += 1;
            }

            while (this.LastChecked < int.MaxValue)
            {
                this.Expand();

                while (pos < this.Primes.Count)
                {
                    yield return this.Primes[pos];
                    pos += 1;
                }
            }
        }

        private void Expand()
        {
            int step = Math.Min(this.LastChecked, this.MaxStep);
            int nextMaximum =
                this.LastChecked <= int.MaxValue - step ? this.LastChecked + step
                : int.MaxValue;

            BitRange bits = new BitRange(this.LastChecked + 1, nextMaximum);
            bits.SetMultipliesOf(this.Primes.TakeWhile(prime => prime <= nextMaximum / 2));
            this.Primes.AddRange(bits.GetZeros());

            this.LastChecked = bits.ToValue;
        }
    }
}
