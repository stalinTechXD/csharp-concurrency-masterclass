using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace csharp_concurrency_masterclass
{
    public  class Demo1_ConcurrencyVsParallelism
    {

        public static async Task RunAsync()
        {
            Console.WriteLine("\n=== 01  Concurrency vs Parallelism ===");
            // ---------- CONCURRENCY: I/O-bound, async, few threads ----------
            // await frees the thread while the "download" is in flight, so a
            // single thread can have many operations in flight at once.

            var sw = Stopwatch.StartNew();
            Task<int>[] downloads =
            {
            FakeDownloadAsync("page-A", 300),
            FakeDownloadAsync("page-B", 300),
            FakeDownloadAsync("page-C", 300),
             };
            int[] sizes = await Task.WhenAll(downloads);   // all overlap in ~300ms
            sw.Stop();

            Console.WriteLine($"[Concurrency] 3x300ms I/O finished in {sw.ElapsedMilliseconds}ms " +
                        $"(overlapped, not 900ms). Bytes: {string.Join(", ", sizes)}");

            // ---------- PARALLELISM: CPU-bound, many cores ----------
            var data = new double[20_000_000];
            for (int i = 0; i < data.Length; i++) data[i] = i;

            // Sequential
            sw.Restart();
            double seq = 0;
            for (int i = 0; i < data.Length; i++) seq += Math.Sqrt(data[i]);
            sw.Stop();
            long seqMs = sw.ElapsedMilliseconds;


            // Parallel across all cores (PLINQ)

            sw.Restart();
            double par = data.AsParallel().Sum(Math.Sqrt);
            sw.Stop();

            Console.WriteLine($"[Parallelism] CPU sum  sequential={seqMs}ms  " +
                              $"parallel={sw.ElapsedMilliseconds}ms on {Environment.ProcessorCount} cores");



        }

        private static async Task<int> FakeDownloadAsync(string name, int ms)
        { 
            await Task.Delay(ms);
            return name.Length * 1000;
        }
    }
}
