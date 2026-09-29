using System.Collections.Concurrent;

namespace ConcurrencyMaster;

// ============================================================
//  04 — Task Scheduling & the Thread Pool
//  You don't create an OS thread per Task. The TaskScheduler queues
//  work; the ThreadPool's reusable workers pull from a global queue
//  plus per-thread local queues (work-stealing).
// ============================================================
public static class Demo4_TaskScheduling
{
    public static async Task RunAsync()
    {
        Console.WriteLine("\n=== 04  Task Scheduling ===");

        // Show pool sizing.
        ThreadPool.GetMinThreads(out int minW, out _);
        ThreadPool.GetMaxThreads(out int maxW, out _);
        Console.WriteLine($"ThreadPool workers: min={minW}, max={maxW}, cores={Environment.ProcessorCount}");

        // ---------- Queue many tasks; watch how few threads service them ----------
        var usedThreads = new ConcurrentDictionary<int, int>();
        var tasks = new Task[12];
        for (int i = 0; i < tasks.Length; i++)
        {
            int id = i;
            tasks[i] = Task.Run(() =>
            {
                int tid = Environment.CurrentManagedThreadId;
                usedThreads.AddOrUpdate(tid, 1, (_, c) => c + 1);
                Thread.SpinWait(20_000_000);   // simulate CPU work
                Console.WriteLine($"  Task {id,2} ran on pool thread {tid}");
            });
        }
        await Task.WhenAll(tasks);
        Console.WriteLine($"12 tasks used only {usedThreads.Count} distinct pool threads (reused).");

        // ---------- Control degree of parallelism ----------
        var items = Enumerable.Range(1, 8).ToArray();
        var options = new ParallelOptions { MaxDegreeOfParallelism = 2 };
        Console.WriteLine("Parallel.ForEach with MaxDegreeOfParallelism = 2:");
        Parallel.ForEach(items, options, n =>
            Console.WriteLine($"  item {n} on thread {Environment.CurrentManagedThreadId}"));

        // ---------- Long-running work: don't starve the pool ----------
        // The hint gives this task its own dedicated thread instead of a
        // pool worker, so short queued tasks aren't blocked behind it.
        await Task.Factory.StartNew(
            () => Thread.Sleep(100),
            CancellationToken.None,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);
        Console.WriteLine("LongRunning task completed on a dedicated thread.");
    }
}
