using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BasicProgrammingLanguage
{
    class Program
    {
        public static bool TestingMode = false;
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Output.WriteDebug("Console Application Started!", "Program", true);
            new Program().Start().GetAwaiter().GetResult();
        }

        async Task Start()
        {
            string sourceCode;
            if (TestingMode == true)
            {
                Output.WriteDebug("Started Test...", "Program", true);
                sourceCode = await GetFile("Test.basic");
            }
            else
            {
                sourceCode = await GetFile("Code.basic");
            }

            Output.WriteDebug("Source Code: ", ConsoleColor.Green);
            Output.WriteDebug(sourceCode, true);

            Token[] tokenList = await Lex.DoLex(sourceCode);

            await Parse.DoParse(tokenList);

            if (TestingMode)
            {
                string correctOutput = await GetFile("Test.correct");
                if (Output.ProgramTest == correctOutput)
                {
                    Output.WriteDebug("Test Complete! No errors!", ConsoleColor.Green);
                }
                else
                {
                    Output.WriteDebug("Test failed, or test output not updated! Output was " + Output.ProgramTest, ConsoleColor.Red);
                }
            }

            await Task.Delay(-1);
        }

        async Task<string> GetFile(string fileName)
        {
            string data = await File.ReadAllTextAsync(fileName);
            return data;
        }
    }
}
