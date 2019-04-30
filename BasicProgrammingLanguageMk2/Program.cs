using System;
using System.IO;
using System.Threading.Tasks;

namespace BasicProgrammingLanguageMk2
{
    class Program
    {
        public const bool TestingMode = false;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Output.WriteDebug("Console Application Started!", "Program", true);
            new Program().Start();
        }

        void Start()
        {
            string sourceCode;
            if (TestingMode == true)
            {
#pragma warning disable CS0162
                Output.WriteDebug("Started Test...", "Program", true);
#pragma warning restore CS0162
                sourceCode = GetFile("Test.txt");
            }
            else
            {
                sourceCode = GetFile("Code.txt");
            }

            Output.WriteDebug("Source Code: ", ConsoleColor.Green);
            Output.WriteDebug(sourceCode, true);

            Lexer.Run(sourceCode);

            Console.ReadKey();
        }

        string GetFile(string fileName)
        {
            string data = File.ReadAllText(fileName);
            return data;
        }
    }
}
