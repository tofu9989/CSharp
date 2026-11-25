using BenchmarkDotNet.Running;
using CSharp.Benchmarks;

BenchmarkSwitcher.FromTypes(new[] { typeof(Q03_StringBenchmarks) })
    .Run(args.Length == 0 ? new[] { "--filter", "*" } : args);
