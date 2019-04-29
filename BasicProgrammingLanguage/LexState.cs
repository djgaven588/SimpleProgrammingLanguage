using System;
using System.Collections.Generic;
using System.Text;

namespace BasicProgrammingLanguage
{
    public class LexState
    {
        public int index;

        public char[] chars;

        public List<Token> tokens;

        public LexCurrentState currentState;

        public int currentLine;
        public int commentStartLine;

        public string currentToken;
        public string currentString;
        public string currentExpression;
        public string currentVariable;

        public LexState(char[] startChars)
        {
            index = 0;
            commentStartLine = 0;
            chars = startChars;
            tokens = new List<Token>();
            currentState = LexCurrentState.None;
            currentLine = 1;
            currentToken = "";
            currentString = "";
            currentExpression = "";
            currentVariable = "";
        }
    }
}
