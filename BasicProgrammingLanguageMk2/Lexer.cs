using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
                if ((state.currentChar == Static.tab || state.currentChar == Static.space) && !state.workingOnString)
                {
                    AddPhraseToken(ref state);
                }
                else if (state.currentChar == Static.lineFeed || state.currentChar == Static.carriageReturn)
                {
                    state.currentLine++;
                    if (state.commentSingleLine)
                    {
                        state.commentStart = -1;
                    }
                    AddPhraseToken(ref state);
                }
                else if (state.commentStart != -1)
                {
                    if (state.commentSingleLine)
                    {
                        state.currentPhrase = "";
                    }
                    else if (state.commentSingleLine != true && state.currentPhrase.Length >= 2 && state.currentPhrase.Substring(state.currentPhrase.Length - 2, 2) == Static.multiLineCommentEnd)
                    {
                        state.commentStart = -1;
                        state.currentIndex--;
                        i--;
                        state.currentPhrase = "";
                    }
                    else
                    {
                        state.currentPhrase += state.currentChar;
                    }
                }
                else if (state.workingOnString)
                {
                    if ((state.currentChar == Static.startString1 && state.stringStartedWithType1) || (state.currentChar == Static.startString2 && !state.stringStartedWithType1))
                    {
                        state.workingOnString = false;
                        state.tokens.Enqueue(new Token(LexState.Action.String, state.currentPhrase));
                        state.currentPhrase = "";
                    }
                    else
                    {
                        state.currentPhrase += state.currentChar;
                    }
                }
                else if (state.currentPhrase == Static.singleLineComment)
                {
                    state.commentStart = state.currentLine;
                    state.commentSingleLine = true;
                    AddPhraseToken(ref state);
                }
                else if (state.currentPhrase == Static.multiLineCommentStart)
                {
                    state.commentStart = state.currentLine;
                    state.commentSingleLine = false;
                    AddPhraseToken(ref state);
                }
                else if (state.currentChar == Static.startString1 || state.currentChar == Static.startString2)
                {
                    state.workingOnString = true;
                    state.stringStartedWithType1 = (state.currentChar == Static.startString1);
                    AddPhraseToken(ref state);
                }
                else if (state.currentChar == Static.property)
                {
                    AddPhraseToken(ref state);
                    state.tokens.Enqueue(new Token(LexState.Action.Property, ""));
                }
                else if (state.currentChar == Static.endOfStatement)
                {
                    AddPhraseToken(ref state);
                    state.tokens.Enqueue(new Token(LexState.Action.EndStatement, ""));
                }
                else if (state.currentChar == Static.beginParameters)
                {
                    AddPhraseToken(ref state);
                    state.tokens.Enqueue(new Token(LexState.Action.ParametersOpen, ""));
                }
                else if (state.currentChar == Static.endParameters)
                {
                    AddPhraseToken(ref state);
                    state.tokens.Enqueue(new Token(LexState.Action.ParametersClose, ""));
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

            Interpreter.BeginInterpret(state.tokens.ToArray());
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

            isValid = Modify.IsModification(state.currentPhrase);
            if (isValid)
            {
                state.tokens.Enqueue(new Token(LexState.Action.Modification, state.currentPhrase));
                state.currentPhrase = "";
                return;
            }

            state.tokens.Enqueue(new Token(LexState.Action.SpecialPhrase, state.currentPhrase));
            state.currentPhrase = "";
        }
    }
}
