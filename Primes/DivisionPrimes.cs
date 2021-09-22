using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Primes
{
    class DivisionPrimes : IPrimesGenerator
    {
        public static IPrimesGenerator Create() =>
            new DivisionPrimes();

        public IEnumerable<int> GetAll()
        {
            yield return 2;
            yield return 3;

            int candidate = 1;
            int step = 4;
            while (candidate <= int.MaxValue - step)
            {
                candidate += step;
                step = 6 - step;
                if (this.IsPrime(candidate)) 
                    yield return candidate;
            }
        }

        private bool IsPrime(int value)
        {
            int step = 2;
            int divisor = 5;
            int maxDivisor = (int)Math.Sqrt(value);
            while (divisor <= maxDivisor)
            {
                if (value % divisor == 0) 
                    return false;
                divisor += step;
                step = 6 - step;
            }
            return true;
        }
    }
}
