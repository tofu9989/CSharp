using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Tests
{
    [TestClass]
    [DoNotParallelize]
    public class StringTimingTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DataRow(1000)]
        [DataRow(10000)]
        [DataRow(20000)]
        public void BuildingStrings_ReportsElapsedTime(int count)
        {
            // Warm up both code paths before measuring. Timings are illustrative.
            Concatenate(100);
            Build(100);

            var timer = Stopwatch.StartNew();
            var concatenated = Concatenate(count);
            timer.Stop();
            var concatenationMs = timer.Elapsed.TotalMilliseconds;

            timer.Restart();
            var built = Build(count);
            timer.Stop();
            var builderMs = timer.Elapsed.TotalMilliseconds;

            Assert.AreEqual(count * 2, concatenated.Length);
            Assert.AreEqual(concatenated, built);
            TestContext.WriteLine(
                "Iterations: {0:N0} | string +=: {1:F3} ms | StringBuilder (including ToString): {2:F3} ms",
                count, concatenationMs, builderMs);
        }

        private static string Concatenate(int count)
        {
            var result = string.Empty;
            for (var i = 0; i < count; i++)
                result += "ab";
            return result;
        }

        private static string Build(int count)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < count; i++)
                builder.Append("ab");
            return builder.ToString();
        }
    }
}
