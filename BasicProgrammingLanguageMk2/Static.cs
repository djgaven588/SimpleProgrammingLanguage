using System;
using System.Collections.Generic;
using System.Text;

namespace BasicProgrammingLanguageMk2
{
    public static class Static
    {
        public static readonly string[] declareAndEndString = new string[] { "'", "\"" };

        public static readonly string singleLineComment = "//";
        public static readonly string multiLineCommentStart = "/*";
        public static readonly string multiLineCommentEnd = "*/";

        public static readonly char endOfStatement = ';';
        public static readonly char beginBlock = '{';
        public static readonly char endBlock = '}';

        public static readonly char lineFeed = '\u000A';
        public static readonly char carriageReturn = '\u000D';
        public static readonly char tab = '	';
        public static readonly char space = ' ';
        public static readonly char property = '.';
        //state.currentChar == '	'
    }
}
