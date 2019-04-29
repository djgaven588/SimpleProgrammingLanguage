using System;
using System.Collections.Generic;
using System.Text;

namespace BasicProgrammingLanguage
{
    public static class Static
    {
        public static class CharArrays
        {
            public static char[] Number =
                { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.' };
            public static char[] Expression =
                { '+', '-', '*', '/', '%'};
            public static char[] VariableCharacters =
                { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
            public static char[] Spacing =
                { ' ', '\n', '\r'};
        }

        public static class StringArrays
        {
            public static string[] Types =
                { "STRING:", "NUMBER:", "EXPRESSION:", "VARIABLE:", "NULL"};
            public static string[] Comparison =
                { "==", "!=", ">=", "<=", ">", "<"};
            public static string[] ComparisonParsed =
                { "ISEQUAL", "ISNOTEQUAL", "ISGREATEROREQUAL", "ISLESSOREQUAL", "ISGREATER", "ISLESS"};
        }
    }
}
