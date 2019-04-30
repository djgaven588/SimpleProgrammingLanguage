using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BasicProgrammingLanguageMk2
{
    public static class Lexer
    {
        public static void Run(string file)
        {
            LexState state = new LexState(file.ToCharArray());

            for (int i = 0; i < state.charLength; i++)
            {
                if (state.currentChar == '	' || state.currentChar == ' ')
                {
                    AddPhraseToken(ref state);
                }
                else if (state.currentChar == Static.lineFeed || state.currentChar == Static.carriageReturn)
                {
                    state.currentLine++;
                    AddPhraseToken(ref state);
                }
                else if (state.currentChar == Static.endOfStatement)
                {
                    AddPhraseToken(ref state);
                    state.tokens.Enqueue(new Token(LexState.Action.EndStatement, ""));
                }
                else
                {
                    state.currentPhrase += state.currentChar;
                }

                if (state.charLength - 1 > i)
                {
                    state.currentIndex++;
                }
                else
                {
                    AddPhraseToken(ref state);
                }
            }


            //Properly formatted token displaying.
            //The index displayed on the left will never move anything over
            //when this is used. It makes it easier to compare things.
            Queue<Token> tokenCopy = new Queue<Token>(state.tokens);
            int index = 0;
            int extraSpaces = (int)Math.Log10(tokenCopy.Count) + 1;
            while (tokenCopy.Count > 0)
            {
                Token token = tokenCopy.Dequeue();
                Output.WriteDebug($"{String.Format("{0, " + extraSpaces + ":#0}", index)} - {token.action.ToString()}: {token.data}");
                index++;
            }
        }

        private static void AddPhraseToken(ref LexState state)
        {
            if (state.currentPhrase.Length < 1)
                return;

            bool isValid = Keywords.IsKeyword(state.currentPhrase);
            if (isValid)
            {
                state.tokens.Enqueue(new Token(LexState.Action.Keyword, state.currentPhrase));
                state.currentPhrase = "";
                return;
            }

            isValid = Operations.IsOperation(state.currentPhrase);
            if (isValid)
            {
                state.tokens.Enqueue(new Token(LexState.Action.Operation, state.currentPhrase));
                state.currentPhrase = "";
                return;
            }

            isValid = Comparisons.IsComparison(state.currentPhrase);
            if (isValid)
            {
                state.tokens.Enqueue(new Token(LexState.Action.Comparison, state.currentPhrase));
                state.currentPhrase = "";
                return;
            }

            state.tokens.Enqueue(new Token(LexState.Action.SpecialPhrase, state.currentPhrase));
            //Output.WriteError($"Line {state.currentLine} has the phrase '{state.currentPhrase.ToString()}' which is invalid!");
            state.currentPhrase = "";
        }
    }
}
