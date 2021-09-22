using System;
using System.Collections.Generic;

namespace Demo.Primes
{
    class ExceptionThrowingGenerator : IPrimesGenerator
    {
        public static IPrimesGenerator Create() =>
            new ExceptionThrowingGenerator();

        public IEnumerable<int> GetAll() => throw new InvalidOperationException("Any operation is invalid on this generator.");
    }
}
