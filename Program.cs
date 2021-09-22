using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using Demo.Primes;

namespace Demo
{
    class Program
    {
        private static ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
        
        static void Main(string[] args)
        {
            StartNewThread(() => Demonstrate("Naive division (Procedural)", DivisionPrimes.Create));
            StartNewThread(() => Demonstrate("Caching division-based (Object-oriented)", PrimeList.Create));
            StartNewThread(() => Demonstrate("Sieve of Eratosthenes (Object-oriented)", EratosthenesSieve.Create));
            StartNewThread(() => Demonstrate("Caching Sieve of Eratosthenes (Object-oriented)", CachingEratosthenesSieve.Create));
            StartNewThread(() => Demonstrate("Division-based (Functional)", PrimeNumbers.Create));
            StartNewThread(() => Demonstrate("Caching division-based (Functional)", CachingPrimeNumbers.Create));
            StartNewThread(() => Demonstrate("Parallel division-based (Functional)", ParallelPrimeNumbers.Create));
            StartNewThread(() => Demonstrate("Exception eating", ExceptionThrowingGenerator.Create));
            
            long exampleDirSize = GetFileLengthsWHash(@".\ExampleFiles");
            Console.WriteLine("\nExample directory size is {0} bytes.\n", exampleDirSize);
        }

        private static void StartNewThread(ThreadStart threadStart)
        {
            new Thread(threadStart).Start();
        }

        static void Demonstrate(string label, Func<IPrimesGenerator> primesFactory)
        {
            try
            {
                locker.EnterWriteLock();
                Console.WriteLine(label);

                IPrimesGenerator generator = primesFactory();

                int start = 1_000_000;
                int step = 1_000_000;
                TimeSpan maxTime = TimeSpan.FromSeconds(5);

                for (int pass = 0; pass < 2; pass += 1)
                {
                    Stopwatch simulationClock = Stopwatch.StartNew();
                    Console.WriteLine();
                    int max = start - step;
                    while (simulationClock.Elapsed < maxTime)
                    {
                        max += step;
                        Stopwatch clock = Stopwatch.StartNew();
                        int count = generator.GetAll()
                            .TakeWhile(prime => prime <= max).Count();

                        Console.WriteLine(
                            $"{max,15:#,##0} " +
                            $"{count,10:#,##0} " +
                            $"{clock.Elapsed,12:mm\\:ss\\:fff}");

                        if (pass > 0 && max >= start + 2 * step) break;
                    }

                    start = Math.Max(max - 2 * step, start);
                }
                Console.WriteLine(new string('-', 60));

            }
            catch (Exception){}
            finally
            {
                locker.ExitWriteLock();
            }
        }
    
        private static long GetFileLengthsWHash(string filePath)
        {
            if (!Directory.Exists(filePath))
            {
                throw new DirectoryNotFoundException("Invalid directory name.");
            }
            else
            {
                string[] files = Directory.GetFiles(filePath);
                string[] dirs = Directory.GetDirectories(filePath);
                if (files.Length + dirs.Length == 0)
                    return 0L;
                else
                {
                    long total = 0;
                    foreach (string fileName in files)
                    {                        
                        using (var hashFunc = MD5.Create())
                        {
                            using (var fs = new FileStream(fileName, FileMode.Open,
                                                    FileAccess.Read, FileShare.ReadWrite,
                                                    256, false))
                            {
                                long length = fs.Length;
                                Interlocked.Add(ref total, length);
                                hashFunc.ComputeHash(fs);
                                fs.Close();
                            }
                        }
                    }

                    return total + dirs.Select(dir => GetFileLengthsWHash(dir)).Sum();
                }
            }

        }

    }
}
