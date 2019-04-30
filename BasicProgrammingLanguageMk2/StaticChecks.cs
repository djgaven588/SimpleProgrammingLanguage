using System;
using System.Collections.Generic;
using System.Text;

namespace BasicProgrammingLanguageMk2
{
    public class Keywords
    {
        private static bool initialized = false;
        private static HashSet<string> keywords = new HashSet<string>();


        public static bool IsKeyword(string phrase)
        {
            if (!initialized)
                Initialize();

            return keywords.Contains(phrase);
        }

        private static void Initialize()
        {
            initialized = true;
            keywords.Add("using");
            keywords.Add("string");
        }
    }

    public class Operations
    {
        private static bool initialized = false;
        private static HashSet<string> keywords = new HashSet<string>();


        public static bool IsOperation(string phrase)
        {
            if (!initialized)
                Initialize();

            return keywords.Contains(phrase);
        }

        private static void Initialize()
        {
            initialized = true;
            keywords.Add("+");
            keywords.Add("-");
            keywords.Add("*");
            keywords.Add("/");
            keywords.Add("%");
            keywords.Add("^");
            keywords.Add("+=");
            keywords.Add("-=");
            keywords.Add("*=");
            keywords.Add("/=");
            keywords.Add("%=");
            keywords.Add("^=");
            keywords.Add("++");
            keywords.Add("--");
        }
    }

    public class Comparisons
    {
        private static bool initialized = false;
        private static HashSet<string> keywords = new HashSet<string>();


        public static bool IsComparison(string phrase)
        {
            if (!initialized)
                Initialize();

            return keywords.Contains(phrase);
        }

        private static void Initialize()
        {
            initialized = true;
            keywords.Add("==");
            keywords.Add("!=");
            keywords.Add("<");
            keywords.Add(">");
            keywords.Add("<=");
            keywords.Add(">=");
        }
    }
}
