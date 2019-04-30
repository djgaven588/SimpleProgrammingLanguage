using System;
using System.Collections.Generic;
using System.Text;

namespace BasicProgrammingLanguageMk2
{
    public class LexState
    {
        public enum Action
        {
            Keyword,
            Operation,
            Comparison,
            EndStatement,
            SpecialPhrase,
            Modification,
            Property
        }

        public Queue<Token> tokens = new Queue<Token>();

        private char[] chars = new char[0];
        public int charLength => chars.Length;
        public char currentChar => chars[currentIndex];

        public int currentIndex = 0;
        public string currentPhrase = "";
        public int currentLine = 0;

        public int commentStart = -1;
        public bool commentSingleLine = false;

        public LexState(char[] chars)
        {
            this.chars = chars;
        }
    }
}
