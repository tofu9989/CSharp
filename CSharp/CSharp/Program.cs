using System;
using System.Threading.Tasks;

namespace CSharp
{
    internal class Program
    {
        // Run without arguments for all examples, or pass a question number: 1..10.
        static void Main(string[] args)
        {
            if (args.Length > 1 || (args.Length == 1 &&
                (!int.TryParse(args[0], out var number) || number < 1 || number > 10)))
            {
                Console.WriteLine("Usage: CSharp.exe [1..10]");
                Environment.ExitCode = 1;
                return;
            }

            int selected = args.Length == 0 ? 0 : int.Parse(args[0]);
            RunAsync(selected).GetAwaiter().GetResult();
        }

        private static async Task RunAsync(int selected)
        {
            Action[] syncExamples = {
                InterviewExamples.Q01_ValueAndReferenceTypes,
                InterviewExamples.Q02_BoxingAndUnboxing,
                InterviewExamples.Q03_StringAndStringBuilder
            };
            for (int question = 1; question <= 10; question++)
            {
                if (selected != 0 && selected != question) continue;
                Console.WriteLine($"\n--- Q{question:00} ---");
                if (question <= 3) syncExamples[question - 1]();
                else switch (question)
                {
                    case 4: await InterviewExamples.Q04_AsyncAwait(); break;
                    case 5: await InterviewExamples.Q05_TaskAndTaskOfT(); break;
                    case 6: await InterviewExamples.Q06_ConfigureAwait(); break;
                    case 7: InterviewExamples.Q07_DisposableAndUsing(); break;
                    case 8: InterviewExamples.Q08_GenericsAndConstraints(); break;
                    case 9: InterviewExamples.Q09_DelegatesAndEvents(); break;
                    case 10: InterviewExamples.Q10_GarbageCollection(); break;
                }
            }
        }
    }
}
