using System;
using System.Collections.Generic;
using System.Text;

namespace BasicProgrammingLanguageMk2
{
    class Output
    {
        public static string ProgramTest = "";

        public static void ProgramOut(string text)
        {
            if (Program.TestingMode)
            {
                ProgramTest += text;
            }
            text = "Program Output: " + text;
            Console.WriteLine(text);
        }

        public static void WriteDebug(string text, bool newLine = true)
        {
            if (newLine)
            {
                Console.WriteLine(text);
            }
            else
            {
                Console.Write(text);
            }
        }
        public static void WriteDebug(string text, ConsoleColor color, bool newLine = true)
        {
            Console.ForegroundColor = color;

            if (newLine)
            {
                Console.WriteLine(text);
            }
            else
            {
                Console.Write(text);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void WriteDebug(string text, string orgin, bool inlineText = false, bool newLine = true)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            if (inlineText)
            {
                Console.Write(orgin + ": ");
            }
            else
            {
                Console.WriteLine(orgin + ": ");
            }
            Console.ForegroundColor = ConsoleColor.White;

            if (newLine)
            {
                Console.WriteLine(text);
            }
            else
            {
                Console.Write(text);
            }
        }

        public static void WriteError(string text, bool newLine = true)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (newLine)
            {
                Console.WriteLine("Error Output: " + text);
            }
            else
            {
                Console.Write("Error Output: " + text);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
