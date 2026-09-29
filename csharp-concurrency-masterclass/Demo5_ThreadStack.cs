namespace ConcurrencyMaster;

// ============================================================
//  05 — Thread Stack
//  Each thread has its OWN private stack (~1MB default). Every call
//  pushes a frame (args, locals, return address); returning pops it.
//  The heap is SHARED across threads — that's why shared objects
//  need thread safety, but locals never do.
// ============================================================
public static class Demo5_ThreadStack
{
    public static void Run()
    {
        Console.WriteLine("\n=== 05  Thread Stack ===");

        // ---------- Each thread's stack is independent ----------
        // Same method, same local name, different value per thread —
        // because each thread has its own copy on its own stack.
        var t1 = new Thread(() => UseLocal("Thread-1", 111));
        var t2 = new Thread(() => UseLocal("Thread-2", 222));
        t1.Start(); t2.Start();
        t1.Join(); t2.Join();

        // ---------- Frames push and pop as calls nest ----------
        Console.WriteLine($"Recursive countdown depth reached: {CountDown(5)} frames deep");

        // ---------- Deep recursion needs a bigger stack ----------
        // Give a worker a 16MB stack so it can recurse far deeper than
        // the default 1MB would allow before StackOverflowException.
        int deep = 0;
        var big = new Thread(() => deep = Recurse(0, 50_000), maxStackSize: 16 * 1024 * 1024);
        big.Start();
        big.Join();
        Console.WriteLine($"With a 16MB stack, recursed {deep:N0} frames without overflow.");

        // NOTE: real infinite recursion throws StackOverflowException,
        // which is uncatchable and terminates the process — so we don't
        // actually trigger it here.
    }

    private static void UseLocal(string who, int value)
    {
        // 'value' lives on THIS thread's private stack frame.
        Console.WriteLine($"  {who}: my private local = {value}");
    }

    private static int CountDown(int n)
    {
        if (n == 0) return 0;
        return 1 + CountDown(n - 1);   // each call adds one stack frame
    }

    private static int Recurse(int depth, int max)
    {
        if (depth >= max) return depth;
        return Recurse(depth + 1, max);
    }
}
