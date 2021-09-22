using System.Collections.Generic;
using System.Linq;

namespace Demo.Primes
{
    class ParallelPrimeNumbers : IPrimesGenerator
    {
        private CachingPrimeNumbers Cache { get; } = new CachingPrimeNumbers();
        private int MaxStep { get; }
        private int RangePerTask { get; }

        public ParallelPrimeNumbers(int maxStep, int rangePerTask)
        {
            this.MaxStep = maxStep;
            this.RangePerTask = rangePerTask;
        }

        public ParallelPrimeNumbers() : this(1_000_000, 10_000)
        {
        }

        public static IPrimesGenerator Create() =>
            new ParallelPrimeNumbers();

        public IEnumerable<int> GetAll()
        {
            foreach (int prime in this.Cache.Cached)
                yield return prime;

            bool added = true;
            while (added)
            {
                (int tasksCount, int taskRange) = this.ScheduleTasks();

                IEnumerable<IEnumerable<int>> newPrimes = Enumerable
                    .Range(0, tasksCount)
                    .Select(taskIndex => this.Cache.LastChecked + taskIndex * taskRange)
                    .AsParallel()
                    .AsOrdered()
                    .Select(greaterThan => this.Cache.GetExtension(greaterThan, taskRange).ToList())
                    .ToList();

                added = false;
                foreach (int prime in this.Cache.WriteThrough(newPrimes.SelectMany(x => x)))
                {
                    added = true;
                    yield return prime;
                }
            }
        }

        private (int tasksCount, int taskRange) ScheduleTasks()
        {
            int newMax =
                this.Cache.LastChecked > int.MaxValue - this.MaxStep ? int.MaxValue
                : this.Cache.LastChecked + this.MaxStep;
            if (this.Cache.LastChecked < this.MaxStep / this.Cache.LastChecked)
                newMax = this.Cache.LastChecked * this.Cache.LastChecked;
            int range = newMax - this.Cache.LastChecked;
            int tasksCount = range / this.RangePerTask;
            int taskRange = this.RangePerTask;

            if (tasksCount == 0)
            {
                tasksCount = 1;
                taskRange = range;
            }

            return (tasksCount, taskRange);
        }
    }
}
