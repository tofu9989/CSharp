using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharp.Tests
{
    [TestClass]
    public class StringBehaviorTests
    {
        [TestMethod]
        public void Concatenation_LeavesOriginalUnchanged_AndCreatesDifferentInstance()
        {
            var original = new string(new[] { 'a', 'b' });
            var alias = original;
            var combined = string.Concat(original, "cd");

            Assert.AreEqual("ab", original);
            Assert.AreSame(original, alias);
            Assert.AreEqual("abcd", combined);
            Assert.AreNotSame(original, combined);
        }

        [TestMethod]
        public void Reassignment_DoesNotChangeExistingAlias()
        {
            var value = new string(new[] { 'a', 'b' });
            var alias = value;
            value += "cd";

            Assert.AreEqual("ab", alias);
            Assert.AreEqual("abcd", value);
            Assert.AreNotSame(alias, value);
            // The original is still reachable through alias, so it is not garbage.
        }

        [TestMethod]
        public void StringBuilder_OperationsMutateTheSameBuilder()
        {
            var builder = new StringBuilder("cat");
            var alias = builder;

            Assert.AreSame(builder, builder.Append("!"));
            Assert.AreEqual("cat!", alias.ToString());
            Assert.AreSame(builder, builder.Insert(0, "A "));
            Assert.AreEqual("A cat!", alias.ToString());
            Assert.AreSame(builder, builder.Replace("cat", "dog"));
            Assert.AreEqual("A dog!", alias.ToString());
        }

        [TestMethod]
        public void ToString_ReturnsSnapshotUnaffectedByLaterAppends()
        {
            var builder = new StringBuilder("hello");
            var snapshot = builder.ToString();
            builder.Append(" world");

            Assert.AreEqual("hello", snapshot);
            Assert.AreEqual("hello world", builder.ToString());
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(1000)]
        public void LoopConcatenation_AndBuilder_ProduceSameContent(int count)
        {
            var concatenated = string.Empty;
            var builder = new StringBuilder();
            for (var i = 0; i < count; i++)
            {
                concatenated += "ab";
                builder.Append("ab");
            }

            Assert.AreEqual(count * 2, concatenated.Length);
            Assert.AreEqual(concatenated, builder.ToString());
        }
    }
}
