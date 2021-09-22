using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Primes
{
    class PrimeNumbers : IPrimesGenerator
    {
        public static IPrimesGenerator Create() =>
            new PrimeNumbers();

        public IEnumerable<int> GetAll() => GetAllPrimes();

        public static IEnumerable<int> GetAllPrimes() =>
            FunctionalExtensions.PrimeCandidates().Where(HasNoDivisors);

        private static bool HasNoDivisors(int number) =>
            FunctionalExtensions.PotentialPrimeDivisorsOf(number).NoneDivides(number);

        private static bool HasNoDivisors(int number, List<int> divisors)
        {
            int maxDivisor = (int) Math.Sqrt(number);
            int pos = 0;
            while (pos < divisors.Count && divisors[pos] <= maxDivisor && number % divisors[pos] != 0)
                pos += 1;
            return pos >= divisors.Count || divisors[pos] > maxDivisor;
        }

        public static IEnumerable<int> GetAllPrimes(int greaterThan, List<int> primes) =>
            GetAllPrimes(FunctionalExtensions.PrimeCandidates(greaterThan), primes);

        public static IEnumerable<int> GetAllPrimes(int greaterThan, int range, List<int> primes) =>
            GetAllPrimes(FunctionalExtensions.PrimeCandidates(greaterThan, range), primes);

        private static IEnumerable<int> GetAllPrimes(IEnumerable<int> candidates, List<int> primes) =>
            candidates.Where(candidate => HasNoDivisors(candidate, primes));
    }
}
