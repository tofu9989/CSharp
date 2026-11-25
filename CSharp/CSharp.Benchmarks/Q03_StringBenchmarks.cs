using System.Text;
using BenchmarkDotNet.Attributes;

namespace CSharp.Benchmarks;

// Q3: string vs StringBuilder. Both methods return the same final text.
[MemoryDiagnoser]
public class Q03_StringBenchmarks
{
    [Params(1000, 10000, 20000)]
    public int Count { get; set; }

    [Benchmark(Baseline = true)]
    public string StringConcatenation()
    {
        var result = string.Empty;
        for (var i = 0; i < Count; i++)
            result += "ab";
        return result;
    }

    [Benchmark]
    public string StringBuilderAppend()
    {
        var builder = new StringBuilder();
        for (var i = 0; i < Count; i++)
            builder.Append("ab");
        return builder.ToString();
    }
}
