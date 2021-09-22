using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Primes
{
    static class FunctionalExtensions
    {
        public static IEnumerable<int> GreaterThan(this int number) =>
            Enumerable.Range(number + 1, int.MaxValue - number);

        public static IEnumerable<int> PotentialDivisors(this int number) =>
            1.GreaterThan().PotentialDivisorsOf(number);

        public static IEnumerable<int> PotentialDivisorsOf(this IEnumerable<int> divisors, int number) =>
            divisors.TakeWhile(divisor => divisor <= (int)Math.Sqrt(number));

        public static bool NoneDivides(this IEnumerable<int> divisors, int number) =>
            divisors.All(divisor => number % divisor != 0);

        public static IEnumerable<int> PrimeCandidates() =>
            PrimeCandidates(1);

        public static IEnumerable<int> PrimeCandidates(int greaterThan, int range) => 
            PrimeCandidates(greaterThan).TakeWhile(candidate => candidate <= greaterThan + range);

        public static IEnumerable<int> PrimeCandidates(int greaterThan)
        {
            if (2 > greaterThan) yield return 2;
            if (3 > greaterThan) yield return 3;

            int candidate = (greaterThan - 1) / 6 * 6 - 1;
            int step = 2;
            while (candidate <= greaterThan - step)
            {
                candidate += step;
                step = 6 - step;
            }

            while (candidate <= int.MaxValue - step)
            {
                candidate += step;
                step = 6 - step;
                yield return candidate;
            }
        }

        public static IEnumerable<int> PotentialPrimeDivisorsOf(int number) =>
            PrimeCandidates().TakeWhile(candidate => candidate <= (int)Math.Sqrt(number));

        public static IEnumerable<int> AppendThrough(this List<int> list, IEnumerable<int> append)
        {
            foreach (int value in append)
            {
                list.Add(value);
                yield return value;
            }
        }

        public static bool IsPowerOf2(this int number)
        {
            int onesCount = 0;
            while (number != 0)
            {
                onesCount += number & 1;
                if (onesCount > 1) return false;
                number >>= 1;
            }
            return onesCount == 1;
        }
    }
}
