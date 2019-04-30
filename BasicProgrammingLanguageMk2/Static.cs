using System;
using System.Collections.Generic;
using System.Text;

namespace BasicProgrammingLanguageMk2
{
    public static class Static
    {
        public static readonly string[] declareComment = new string[] { "//", "/*" };
        public static readonly string[] endComment = new string[] { "*/" };
        public static readonly string[] declareAndEndString = new string[] { "'", "\"" };

        public static readonly char endOfStatement = ';';
        public static readonly char beginBlock = '{';
        public static readonly char endBlock = '}';

        public static readonly char lineFeed = '\u000A';
        public static readonly char carriageReturn = '\u000D';
    }
}
