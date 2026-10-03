# MicroBundleDomain Benchmarking

The companion benchmark project is:

**[MicroBundleDomain_Benchmarks](https://github.com/TrentBest/MicroBundleDomain_Benchmarks)**

This benchmark measures the cost of the **domain contract itself**. It does not measure FSM_COS composition.

That distinction is important.

~~~text
MicroBundleDomain
    │
    ├── identity
    ├── version
    ├── dependencies
    └── providers
          │
          ▼
     benchmark
          │
          X
       no FSM_COS
~~~

## What is being measured?

The benchmark constructs `MicroBundleDescriptor` instances with different numbers of dependencies and providers.

The workload matrix is:

~~~text
0
1
4
16
64
~~~

For each count it measures:

- empty descriptor;
- dependencies only;
- providers only;
- dependencies and providers together.

The benchmark uses BenchmarkDotNet's memory diagnostics on .NET 8.

## Recorded first-run results

The first Release benchmark was run with BenchmarkDotNet 0.15.2 on .NET 8.0.31 using:

- Windows 10 22H2;
- Intel Core i5-10400F;
- 6 physical / 12 logical cores;
- X64 RyuJIT AVX2.

The baseline is `EmptyDescriptor`.

| Count | Scenario | Mean | Allocated |
|---:|---|---:|---:|
| 0 | Empty descriptor | 163.5 ns | 304 B |
| 0 | Dependencies only | 156.0 ns | 304 B |
| 0 | Providers only | 156.2 ns | 304 B |
| 0 | Dependencies + providers | 150.2 ns | 304 B |
| 1 | Empty descriptor | 154.9 ns | 304 B |
| 1 | Dependencies only | 214.1 ns | 552 B |
| 1 | Providers only | 223.7 ns | 552 B |
| 1 | Dependencies + providers | 281.5 ns | 800 B |
| 4 | Empty descriptor | 157.1 ns | 304 B |
| 4 | Dependencies only | 276.4 ns | 768 B |
| 4 | Providers only | 338.7 ns | 768 B |
| 4 | Dependencies + providers | 474.1 ns | 1,232 B |
| 16 | Empty descriptor | 158.9 ns | 304 B |
| 16 | Dependencies only | 405.8 ns | 1,256 B |
| 16 | Providers only | 633.5 ns | 1,256 B |
| 16 | Dependencies + providers | 903.7 ns | 2,208 B |
| 64 | Empty descriptor | 159.7 ns | 304 B |
| 64 | Dependencies only | 959.6 ns | 4,264 B |
| 64 | Providers only | 1,958.7 ns | 4,264 B |
| 64 | Dependencies + providers | 2,761.9 ns | 8,224 B |

## What the first run tells us

### Empty descriptors stay essentially flat

The empty descriptor remains around 150–160 ns across the matrix.

That is useful because the descriptor's fixed construction cost is not growing with the requested dependency/provider count.

### Dependencies add cost as composition grows

Dependency-only construction rises from 214.1 ns at one dependency to 959.6 ns at 64.

### Providers add more measured construction cost

Provider-only construction rises from 223.7 ns at one provider to 1,958.7 ns at 64.

### Combining both is additive in the expected direction

At 64 entries, dependencies plus providers measured 2,761.9 ns and allocated 8,224 B.

These numbers establish a baseline for the descriptor contract. They do not tell us whether an Experience or composition runtime is fast, because those are different workloads.

## Why this benchmark should not be confused with FSM_COS

MicroBundleDomain answers:

> **What is a MicroBundle?**

FSM_COS answers:

> **How do capabilities become a runtime composition?**

The benchmark follows the same boundary:

~~~text
MicroBundleDomain benchmark
    │
    └── descriptor construction
          │
          ▼
       evidence

FSM_COS benchmark
    │
    └── dependency traversal / loading / arbitration
          │
          ▼
       different evidence
~~~

If the composition layer becomes performance-sensitive, it should receive its own benchmark rather than making this domain benchmark responsible for measuring it.

## How to recognize the benchmark

The source file is:

~~~text
MicroBundleDomain_Benchmarks.cs
~~~

Look for:

- `[MemoryDiagnoser]`;
- `[SimpleJob(RuntimeMoniker.Net80)]`;
- `[Params(0, 1, 4, 16, 64)]`;
- `[Benchmark(Baseline = true)]`.

The benchmark setup creates the dependency/provider arrays before measurement. The measured methods then construct the descriptor.

That matters: the setup cost is intentionally kept outside the measured operation.

## Reproducing the experiment

The benchmark project references:

~~~text
TheSingularityWorkshop.MicroBundleDomain 0.1.0-alpha.1
BenchmarkDotNet 0.15.2
.NET 8
~~~

Run:

~~~bash
dotnet run -c Release
~~~

These results are environment-specific observations. Hardware, runtime, OS, benchmark version, and implementation changes can move them.

## Why benchmark the domain at all?

A domain package can be architecturally clean and still have accidental runtime cost.

The benchmark gives us a simple way to observe:

~~~text
number of relationships
        │
        ▼
descriptor construction cost
        │
        ▼
allocation cost
~~~

That becomes especially useful if the descriptor representation changes later.

## When should we rerun it?

Rerun after changes to:

- `MicroBundleDescriptor`;
- dependency representation;
- provider representation;
- validation;
- collection materialization;
- read-only wrapping;
- identity/version representation;
- allocation strategy.

Again, the purpose is not to produce a magic number.

**The purpose is to know what the implementation costs.**
