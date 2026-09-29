# 🧵 C# Concurrency & Parallelism Masterclass

> A hands-on, deep-dive journey through concurrency, parallelism, and thread safety in C# / .NET.
> Every concept is paired with runnable code, classic problems, and modern .NET alternatives.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](CONTRIBUTING.md)

---

## 🎯 Why This Repo?

Most concurrency tutorials stop at `lock (obj) { }`. This repo goes **from first principles to production patterns**:

- 🔬 **What actually happens** at the CPU/CLR level (context switches, memory barriers, thread stacks)
- ⚠️ **Why things break** (race conditions, memory visibility, deadlock, livelock)
- 🛠️ **How to fix them** (monitors, volatile, explicit locks, synchronizers, atomics)
- 🏭 **How real systems do it** (thread pools, `ExecutorService`, lock striping, non-blocking algorithms)

Every chapter ships as a **runnable console project** so you can experiment, break things, and fix them.

---

## 🗺️ Roadmap

| # | Chapter | Topics | Project |
|---|---------|--------|---------|
| 1 | **Fundamentals** | Concurrency vs Parallelism, Context switches, Task scheduling, Thread stack, Race conditions, Locks, Reentrancy | [`01.Fundamentals`](src/01.Fundamentals) |
| 2 | **Memory & Signaling** | `volatile`, Memory visibility, Monitors, Busy-waiting vs `wait/notify` | [`02.VolatileMonitors`](src/02.VolatileMonitors) |
| 3 | **Classic Problems** | Deadlock, Livelock, Producer–Consumer, Dining Philosophers | [`03.ClassicProblems`](src/03.ClassicProblems) |
| 4 | **Locks & Synchronizers** | ReadWriteLock, BlockingQueue, Hand-over-hand locking, Custom reentrant locks, Latch, Semaphore, Barrier, FutureTask | [`04.LocksAndSynchronizers`](src/04.LocksAndSynchronizers) |
| 5 | **Pools & Atomics** | Custom ThreadPool, `ExecutorService`, Lock striping, Thread-safe HashMap, Atomic types | [`05.ThreadPoolsAndAtomics`](src/05.ThreadPoolsAndAtomics) |

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Any editor: VS 2022, VS Code, or Rider

### Clone & Run

```bash
git clone https://github.com/<your-user>/csharp-concurrency-masterclass.git
cd csharp-concurrency-masterclass

# Restore everything
dotnet restore

# Run a specific chapter
dotnet run --project src/01.Fundamentals
dotnet run --project src/03.ClassicProblems
```

Each chapter's `Program.cs` contains a **menu** to run individual demos.

---

## 📚 Chapter Highlights

### 1️⃣ Fundamentals
- **Concurrency ≠ Parallelism** — one is *structure*, the other is *execution*.
- Simulate **context switches** with `Thread.Yield()` and measure them.
- Inspect **thread stack sizes** and `Thread.CurrentThread` properties.
- Demonstrate a **race condition** with `counter++` and fix it with `lock`.

### 2️⃣ Memory Visibility & Signaling
- See a real **`volatile`** bug: an infinite loop caused by CPU caching.
- Compare **busy-waiting** vs **`Monitor.Wait/Pulse`** for CPU efficiency.
- Ping-pong exercise: two threads alternating turns safely.

### 3️⃣ Classic Problems
- **Deadlock** reproduced with two locks acquired in opposite order.
- **Livelock** with two polite threads forever yielding.
- **Producer–Consumer** in 3 flavors: Monitor, BlockingCollection, Channel.
- **Dining Philosophers** with a resource hierarchy solution.

### 4️⃣ Locks & Synchronizers
- Build your own **ReentrantLock** from scratch (owner thread + hold count).
- **ReadWriteLockSlim** for reader-heavy workloads.
- **CountdownEvent**, **SemaphoreSlim**, **Barrier**, and **TaskCompletionSource** (FutureTask equivalent).
- **Hand-over-hand locking** in a linked list.

### 5️⃣ Pools & Atomics
- Build a **SimpleThreadPool** using `BlockingCollection`.
- Compare with `Task.Run`, `Parallel.For`, and `Channel<T>`.
- **Lock striping**: a `StripedHashMap` with 16 shards → 16× throughput.
- **`Interlocked`**, `Volatile.Read/Write`, and `System.Threading.Atomics`.

---

## 🧪 Testing & Benchmarking

```bash
dotnet test tests/Concurrency.Tests
dotnet run -c Release --project benchmarks/Concurrency.Benchmarks
```

Benchmarks use **BenchmarkDotNet** to compare:
- `lock` vs `Interlocked` vs `SemaphoreSlim`
- Single-lock HashMap vs Striped HashMap
- ThreadPool vs `Task.Run` vs raw threads

---

## 🧠 Mental Model Cheat Sheet

| Problem | Tool |
|---------|------|
| Data race | `lock`, `Interlocked`, `Monitor` |
| Stale reads | `volatile`, `Volatile.Read/Write`, memory barriers |
| Deadlock | Lock ordering, `Monitor.TryEnter` with timeout |
| High contention | Lock striping, `ConcurrentDictionary`, `Interlocked` |
| Signaling | `ManualResetEventSlim`, `SemaphoreSlim`, `Channel<T>` |
| Fan-out / fan-in | `Task.WhenAll`, `Parallel.ForEachAsync` |
| Producer–Consumer | `BlockingCollection<T>`, `Channel<T>` |
| Coordination | `Barrier`, `CountdownEvent`, `TaskCompletionSource` |

---

## 🤝 Contributing

PRs welcome! Please:
1. Keep each demo **self-contained** in a single file when possible.
2. Add a short comment block at the top explaining *what* and *why*.
3. Add a test in `tests/` if it's a new algorithm.

---

## 📜 License

MIT — see [LICENSE](LICENSE).

---

## ⭐ Star History

If this repo helped you land an interview or fix a bug, drop a ⭐ — it helps others find it!
