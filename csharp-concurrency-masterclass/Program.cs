using ConcurrencyMaster;
using System.Diagnostics;

namespace csharp_concurrency_masterclass;

// ============================================================
//  Mastering Concurrency in C# — runnable demos
//  Run a specific demo:  dotnet run -- 1   (1..5)
//  Run all:              dotnet run
// ============================================================

public static class Program
{
    public static async Task Main(string[] args)
    { 
      // await Demo1_ConcurrencyVsParallelism.RunAsync();

       // Demo2_ThreadSafety.Run();

      //  await Demo3_ContextSwitches.RunAsync();

     // await Demo4_TaskScheduling.RunAsync();

        Demo5_ThreadStack.Run();
    }
}