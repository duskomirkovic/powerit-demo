using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Primes
{
    class EratosthenesSieve : IPrimesGenerator
    {
        private BitRange Divided { get; }
        private int MaxStep { get; }

        public EratosthenesSieve() : this(10_000_000)
        {
        }

        public EratosthenesSieve(int maxStep)
        {
            this.Divided = new BitRange(2, 2);
            this.MaxStep = maxStep;
        }

        public static IPrimesGenerator Create() =>
            new EratosthenesSieve();

        public IEnumerable<int> GetAll()
        {
            int lastIssued = this.Divided.FromValue - 1;
            while (true)
            {
                foreach (int value in this.Divided.GetZeros(lastIssued + 1))
                    yield return value;

                lastIssued = this.Divided.ToValue;

                if (this.Divided.ToValue < int.MaxValue)
                    this.Expand();
                else
                    break;
            }
        }

        private void Expand()
        {
            int prevMaximum = this.Divided.ToValue;
            int nextToValue =
                this.Divided.ToValue <= int.MaxValue / 2 ? this.Divided.ToValue * 2
                : int.MaxValue;
            int step = Math.Min(nextToValue - this.Divided.ToValue, this.MaxStep);

            this.Divided.ToValue += step;

            int candidate = 1;
            while (candidate < prevMaximum)
            {
                candidate += 1;
                if (!this.Divided[candidate])
                {
                    this.Divided.SetMultipliesOf(candidate, prevMaximum + 1);
                }
            }
        }
    }
}