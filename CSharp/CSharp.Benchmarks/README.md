# Benchmark in Visual Studio

1. Open ../CSharp.slnx.
2. Right-click CSharp.Benchmarks > Set as Startup Project.
3. Select Release configuration.
4. Press Ctrl+F5 (Start Without Debugging).
5. Wait for the summary table in the console. The default run may take several minutes.

Columns:
- Mean: average time to build one complete string (check ns/us/ms units).
- Ratio: time relative to StringConcatenation for the same Count (lower is faster).
- Allocated: managed memory allocated per operation, not peak memory.
- Gen0/Gen1/Gen2: GC collections per 1,000 operations.

Reports are saved under BenchmarkDotNet.Artifacts/results in the working directory.
The console prints report paths. Open the HTML report in a browser.

These benchmarks target .NET 10, whereas the existing examples/tests target
.NET Framework 4.8.1. Results describe the runtime used by this benchmark.
They do not prove a universal fragment-count threshold or Big-O complexity.

CLI (from repository root):
    dotnet run --project CSharp/CSharp.Benchmarks -c Release

Quick smoke run (validates the harness, not reliable performance results):
    dotnet run --project CSharp/CSharp.Benchmarks -c Release -- --filter * --job Dry
