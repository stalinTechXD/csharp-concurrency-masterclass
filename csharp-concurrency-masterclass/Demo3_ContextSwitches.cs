using System.Diagnostics;

namespace ConcurrencyMaster;

// ============================================================
//  03 — Context Switches
//  A core runs one thread per instant. The OS time-slices: it saves
//  one thread's registers/stack pointer and loads another's. Many
//  blocking threads => a "switch storm" that wastes CPU.
// ============================================================
public static class Demo3_ContextSwitches
{
    public static async Task RunAsync()
    {
        Console.WriteLine("\n=== 03  Context Switches ===");

        const int workItems = 2_000;

        // ---------- BAD: one blocking thread per item ----------
        // Each Thread.Sleep parks the thread; the scheduler must switch
        // constantly and each thread costs ~1MB of stack.
        var sw = Stopwatch.StartNew();
        var threads = new List<Thread>();
        for (int i = 0; i < 200; i++)   // 200 is already heavy; 2000 would thrash
        {
            var t = new Thread(() => Thread.Sleep(50));
            t.Start();
            threads.Add(t);
        }
        foreach (var t in threads) t.Join();
        sw.Stop();
        Console.WriteLine($"[BAD ] 200 blocking threads  => {sw.ElapsedMilliseconds}ms, " +
                          "many context switches, ~200MB of stacks");

        // ---------- GOOD: async releases the thread during the wait ----------
        // Task.Delay does NOT hold a thread; a handful of pool threads
        // service thousands of overlapping waits with minimal switching.
        sw.Restart();
        var tasks = new Task[workItems];
        for (int i = 0; i < workItems; i++)
            tasks[i] = Task.Delay(50);
        await Task.WhenAll(tasks);
        sw.Stop();
        Console.WriteLine($"[GOOD] {workItems} async waits      => {sw.ElapsedMilliseconds}ms, " +
                          "no threads blocked, few switches");

        // Peek at how busy the scheduler was for this process.
        Console.WriteLine($"Threads currently in process: {Process.GetCurrentProcess().Threads.Count}");
    }
}
