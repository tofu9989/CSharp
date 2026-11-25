using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CSharp
{
    // Question numbers follow the C# section of:
    // https://interviewchamp.ai/learn/dotnet-interview-questions-2026
    // Examples demonstrate behavior; timings and GC scheduling are not guaranteed.
    internal static class InterviewExamples
    {
        private sealed class Person { public string Name; }

        // Q1. Value types vs reference types
        public static void Q01_ValueAndReferenceTypes()
        {
            int original = 10;
            int copy = original;
            copy = 20;
            Console.WriteLine($"Value: original={original}, copy={copy}"); // 10, 20

            var person = new Person { Name = "Alice" };
            var alias = person;
            alias.Name = "Bob";
            Console.WriteLine($"Shared object: {person.Name}"); // Bob
            Reassign(person);
            Console.WriteLine($"After parameter reassignment: {person.Name}"); // Bob
            // Reference parameters are passed by value unless ref/out is specified.
            // Value type does not imply stack allocation in every context.
        }

        private static void Reassign(Person person)
        {
            person = new Person { Name = "Charlie" };
        }

        // Q2. Boxing and unboxing
        public static void Q02_BoxingAndUnboxing()
        {
            int number = 5;
            object boxed = number; // Copy the value into a boxed object.
            number = 10;
            int unboxed = (int)boxed; // Must unbox as the actual boxed type.
            Console.WriteLine($"number={number}, unboxed={unboxed}"); // 10, 5
            var numbers = new List<int> { number }; // Stores int without boxing.
            Console.WriteLine($"Generic list: {numbers[0]}");
        }

        // Q3. string vs StringBuilder
        public static void Q03_StringAndStringBuilder()
        {
            string text = "Hello";
            string original = text;
            text += " World";
            Console.WriteLine($"Original: {original}; new string: {text}");
            // original still references the old string; it is not garbage.
            var builder = new StringBuilder("Hello");
            var alias = builder;
            builder.Append(" World");
            builder.Insert(0, "Say: ");
            builder.Replace("World", "C#");
            Console.WriteLine($"Same builder: {ReferenceEquals(builder, alias)}; {alias}");
            // The builder may allocate more storage as it grows.
        }

        // Q4. async/await and the compiler-generated state machine
        public static async Task Q04_AsyncAwait()
        {
            Console.WriteLine("Before await");
            await Task.Delay(20); // Suspend if incomplete; does not block this thread.
            Console.WriteLine("After await");
            // The compiler preserves state across await. async does not itself
            // create a new thread. Inspect generated IL to see the state machine.
        }

        // Q5. Task vs Task<T>
        public static async Task Q05_TaskAndTaskOfT()
        {
            await SaveAsync(); // Completion, without a result value.
            int result = await GetCountAsync(); // Completion with an int result.
            Console.WriteLine($"Count: {result}");
        }

        private static async Task SaveAsync()
        {
            await Task.Delay(10);
            Console.WriteLine("Saved (simulation)");
        }

        private static async Task<int> GetCountAsync()
        {
            await Task.Delay(10);
            return 42;
        }

        // Q6. ConfigureAwait(false)
        public static async Task Q06_ConfigureAwait()
        {
            string result = await LibraryOperationAsync();
            Console.WriteLine(result);
            // A console app normally has no SynchronizationContext, so it does
            // not visibly demonstrate the UI-context difference.
        }

        private static async Task<string> LibraryOperationAsync()
        {
            await Task.Delay(10).ConfigureAwait(false);
            // No requirement to resume on the caller's captured context.
            // This does not guarantee a different thread.
            return "Library operation completed";
        }

        // Q7. IDisposable and using
        public static void Q07_DisposableAndUsing()
        {
            var stream = new MemoryStream();
            using (stream)
            {
                stream.WriteByte(65);
                Console.WriteLine($"Inside using: CanWrite={stream.CanWrite}"); // True
            }
            Console.WriteLine($"After Dispose: CanWrite={stream.CanWrite}"); // False
            // using also disposes when the body exits through an exception.
        }

        // Q8. Generics and constraints
        public static void Q08_GenericsAndConstraints()
        {
            Console.WriteLine($"Max int: {Max(3, 7)}");
            Console.WriteLine($"Max decimal: {Max(2.5m, 1.2m)}");
            // Max(new Person(), new Person()) would fail to compile:
            // Person does not implement IComparable<Person>.
        }

        private static T Max<T>(T left, T right) where T : IComparable<T>
        {
            return left.CompareTo(right) >= 0 ? left : right;
        }

        // Q9. Delegates vs events
        public static void Q09_DelegatesAndEvents()
        {
            Func<int, int> square = value => value * value;
            Console.WriteLine($"Delegate: square(4)={square(4)}");
            var publisher = new Publisher();
            EventHandler handler = (sender, args) => Console.WriteLine("Event received");
            publisher.Changed += handler;
            publisher.Notify();
            publisher.Changed -= handler;
            publisher.Notify(); // No handler is subscribed now.
            // publisher.Changed(...) is not allowed outside Publisher.
        }

        private sealed class Publisher
        {
            public event EventHandler Changed;
            public void Notify() { Changed?.Invoke(this, EventArgs.Empty); }
        }

        // Q10. Garbage collector and generations
        public static void Q10_GarbageCollection()
        {
            var retained = new object();
            Console.WriteLine($"Generation (runtime-dependent): {GC.GetGeneration(retained)}");
            Console.WriteLine($"Estimated managed memory: {GC.GetTotalMemory(false):N0} bytes");
            Console.WriteLine($"Collections: Gen0={GC.CollectionCount(0)}, " +
                $"Gen1={GC.CollectionCount(1)}, Gen2={GC.CollectionCount(2)}");
            GC.KeepAlive(retained); // Keep the object reachable through this point.
            // Unreachable objects become eligible for collection; reclamation
            // timing is controlled by the runtime. No forced GC in this example.
        }
    }
}
