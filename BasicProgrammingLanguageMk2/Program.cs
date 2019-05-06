using System;
using System.IO;
using System.Threading.Tasks;

namespace BasicProgrammingLanguageMk2
{
    class Program
    {
        public static readonly bool TestingMode = false;

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
                Output.WriteDebug("Started Test...", "Program", true);
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
