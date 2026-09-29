using System;
using System.Collections.Generic;
using System.Text;

namespace csharp_concurrency_masterclass
{
    public static class Demo2_ThreadSafety
    {
        private const int PerThread = 1_000_000;
        private static readonly object _gate = new();

        public static void Run()
        {
            Console.WriteLine("\n=== 02  Thread Safety ===");
            Console.WriteLine($"Expected total (all methods): {2 * PerThread:N0}\n");

            Console.WriteLine($"UNSAFE  count++        => {RunUnsafe():N0}   (lost updates!)");
            Console.WriteLine($"SAFE    lock           => {RunWithLock():N0}");
            Console.WriteLine($"SAFE    Interlocked    => {RunWithInterlocked():N0}   (lock-free, fastest)");
        }

        // ---------- NOT thread-safe: race condition ----------
        private static long RunUnsafe()
        {
            long count = 0;
            var t1 = new Thread(() => { for (int i = 0; i < PerThread; i++) count++; });
            var t2 = new Thread(() => { for (int i = 0; i < PerThread; i++) count++; });
            t1.Start(); t2.Start();
            t1.Join(); t2.Join();
            return count;   // almost always < 2,000,000
        }

        // ---------- Safe via mutual exclusion ----------
        private static long RunWithLock()
        {
            long count = 0;
            void Work()
            {
                for (int i = 0; i < PerThread; i++)
                    lock (_gate) { count++; }   // only one thread in the critical section
            }
            var t1 = new Thread(Work);
            var t2 = new Thread(Work);
            t1.Start(); t2.Start();
            t1.Join(); t2.Join();
            return count;
        }

        // ---------- Safe via atomic hardware instruction ----------
        private static long RunWithInterlocked()
        {
            long count = 0;
            void Work()
            {
                for (int i = 0; i < PerThread; i++)
                    Interlocked.Increment(ref count);   // atomic increment, lock-free
            }
            var t1 = new Thread(Work);
            var t2 = new Thread(Work);
            t1.Start(); t2.Start();
            t1.Join(); t2.Join();
            return count;
        }
    }
}
