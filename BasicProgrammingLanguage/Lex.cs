using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BasicProgrammingLanguage
{
    class Lex
    {
        public static async Task<Token[]> DoLex(string fileContents, bool quite = false)
        {
            fileContents += "\r\n";
            LexState lexState = new LexState(fileContents.ToCharArray());
            //Turn file into character array.

            /*char[] chars = fileContents.ToCharArray();

            List<Token> tokens = new List<Token>();

            LexCurrentState currentState = LexCurrentState.None;

            int currentLine = 1;

            string currentToken = "";
            string currentString = "";
            string currentExpression = "";
            string currentVariable = "";*/

            for (int i = 0; i < lexState.chars.Length; i++)
            {
                char currentCharacter = lexState.chars[i];
                lexState.currentToken += currentCharacter;

                if (lexState.currentState == LexCurrentState.None)
                {
                    NoneStateProcess(ref lexState);
                }
                else if (lexState.currentState == LexCurrentState.String)
                {
                    StringStateProcess(ref lexState);
                }
                else if (lexState.currentState == LexCurrentState.Number || lexState.currentState == LexCurrentState.Expression)
                {
                    NumberExpressionStateProcess(ref lexState);
                }
                else if (lexState.currentState == LexCurrentState.Variable)
                {
                    VariableStateProcess(ref lexState);
                }
                else if (lexState.currentState == LexCurrentState.Comment)
                {
                    if (lexState.commentStartLine != lexState.currentLine)
                    {
                        lexState.currentState = LexCurrentState.None;
                        lexState.index--;
                    }
                    else
                    {
                        if (lexState.currentToken == "\n" || lexState.currentToken == "\r\n")
                            lexState.currentLine++;
                        lexState.currentToken = "";
                    }
                }
            }

            if (!quite)
            {
                Output.WriteDebug("Finished converting to tokens!", ConsoleColor.Green, true);
                Output.WriteDebug(lexState.tokens.Count.ToString(), "Lex Tokens");
            }

            Token[] finalTokens = lexState.tokens.ToArray();

            if (!quite)
            {
                Output.WriteDebug("Token Array: ", ConsoleColor.Green);
                Output.WriteTokenArray(finalTokens);
            }
            await Task.Delay(0);

            return finalTokens;
        }

        private static void NoneStateProcess(ref LexState lexState)
        {
            bool done = true;
            switch (lexState.currentToken)
            {
                case " ":
                    lexState.currentToken = "";
                    break;
                case "//":
                    lexState.currentState = LexCurrentState.Comment;
                    lexState.currentToken = "";
                    lexState.commentStartLine = lexState.currentLine;
                    break;
                case "  ":
                    lexState.currentToken = "";
                    break;
                case "\n":
                    lexState.currentLine += 1;
                    lexState.currentToken = "";
                    break;
                case "\r\n":
                    lexState.currentLine += 1;
                    lexState.currentToken = "";
                    break;
                case "print":
                    AddCurrentToken(ref lexState);
                    break;
                case "input":
                    AddCurrentToken(ref lexState);
                    break;
                case "if":
                    AddCurrentToken(ref lexState);
                    break;
                case "end":
                    AddCurrentToken(ref lexState);
                    break;
                case "else":
                    AddCurrentToken(ref lexState);
                    break;
                case "\"":
                    lexState.currentState = LexCurrentState.String;
                    lexState.currentToken = "";
                    break;
                case "$":
                    lexState.currentState = LexCurrentState.Variable;
                    lexState.currentToken = "";
                    break;
                case "=":
                    string lastTokenValue = lexState.tokens[lexState.tokens.Count - 1].tokenValue;
                    if (lastTokenValue == "SETEQUAL")
                        lexState.tokens[lexState.tokens.Count - 1].tokenValue = "ISEQUAL";
                    else if (lastTokenValue == "ISGREATER")
                        lexState.tokens[lexState.tokens.Count - 1].tokenValue = "ISGREATEROREQUAL";
                    else if (lastTokenValue == "ISLESS")
                        lexState.tokens[lexState.tokens.Count - 1].tokenValue = "ISLESSOREQUAL";
                    else
                        lexState.tokens.Add(new Token("SETEQUAL", lexState.currentLine));
                    lexState.currentToken = "";
                    break;
                case ">":
                    lexState.tokens.Add(new Token("ISGREATER", lexState.currentLine));
                    lexState.currentToken = "";
                    break;
                case "<":
                    lexState.tokens.Add(new Token("ISLESS", lexState.currentLine));
                    lexState.currentToken = "";
                    break;
                case "!=":
                    lexState.tokens.Add(new Token("ISNOTEQUAL", lexState.currentLine));
                    lexState.currentToken = "";
                    break;
                case "castToNum":
                    AddCurrentToken(ref lexState);
                    break;
                case "castToString":
                    AddCurrentToken(ref lexState);
                    break;
                case "loadFile":
                    AddCurrentToken(ref lexState);
                    break;
                case "runCode":
                    AddCurrentToken(ref lexState);
                    break;
                case "null":
                    lexState.tokens.Add(new Token("NULL", lexState.currentLine));
                    lexState.currentToken = "";
                    break;
                case "+":
                case "-":
                case "*":
                case "/":
                case "%":
                    if (lexState.tokens[lexState.tokens.Count - 1].tokenValue.Contains("VARIABLE:") || lexState.tokens[lexState.tokens.Count - 1].tokenValue.Contains("NUMBER:") || lexState.tokens[lexState.tokens.Count - 1].tokenValue.Contains("STRING:\""))//lexState.chars.Length > lexState.index + 1 && lexState.chars[lexState.index + 1] == ' ')
                    {
                        if (lexState.currentToken == "/" && lexState.chars.Length > lexState.index + 1 && lexState.chars[lexState.index + 1] != '/')
                        {
                            lexState.tokens.Add(new Token("OP", lexState.currentLine));
                            AddCurrentToken(ref lexState);
                        }
                        else if (lexState.currentToken != "/")
                        {
                            lexState.tokens.Add(new Token("OP", lexState.currentLine));
                            AddCurrentToken(ref lexState);
                        }
                    }
                    break;
                default:
                    done = false;
                    break;
            }

            if (!done)
            {
                if (lexState.currentToken.IndexOfAny(Static.CharArrays.Number) != -1)
                {
                    lexState.currentExpression += lexState.currentToken;
                    lexState.currentState = LexCurrentState.Number;
                    if (lexState.index + 1 >= lexState.chars.Length)
                    {
                        lexState.tokens.Add(new Token(lexState.currentState.ToString().ToUpper() + ":" + lexState.currentExpression, lexState.currentLine));
                        lexState.currentExpression = "";
                        lexState.currentState = LexCurrentState.None;
                    }
                    lexState.currentToken = "";
                }
            }
        }

        private static void StringStateProcess(ref LexState lexState)
        {
            if (lexState.currentToken == "\"")
            {
                lexState.tokens.Add(new Token("STRING:\"" + lexState.currentString + "\"", lexState.currentLine));
                lexState.currentToken = "";
                lexState.currentString = "";
                lexState.currentState = LexCurrentState.None;
            }
            else
            {
                lexState.currentString += lexState.currentToken;
                lexState.currentToken = "";
            }
        }

        private static void NumberExpressionStateProcess(ref LexState lexState)
        {
            if (lexState.currentToken == "\n" || lexState.currentToken == "\r\n")
            {
                lexState.currentLine++;
            }
            else
            {
                if (lexState.currentState != LexCurrentState.Expression && lexState.currentToken.LastIndexOfAny(Static.CharArrays.Expression) > -1)
                {
                    lexState.currentState = LexCurrentState.Expression;
                }

                if (lexState.currentToken.IndexOfAny(Static.CharArrays.Number) == -1 && lexState.currentToken.LastIndexOfAny(Static.CharArrays.Expression) == -1)
                {
                    lexState.index--;
                    lexState.tokens.Add(new Token(lexState.currentState.ToString().ToUpper() + ":" + lexState.currentExpression, lexState.currentLine));
                    lexState.currentExpression = "";
                    lexState.currentState = LexCurrentState.None;
                }
                else
                {
                    if (lexState.currentToken.IndexOfAny(Static.CharArrays.Number) == -1)
                    {
                        lexState.currentExpression += ' ' + lexState.currentToken + ' ';
                    }
                    else
                    {
                        lexState.currentExpression += lexState.currentToken;
                    }

                    if (lexState.index + 1 >= lexState.chars.Length)
                    {
                        lexState.tokens.Add(new Token(lexState.currentState.ToString().ToUpper() + ":" + lexState.currentExpression, lexState.currentLine));
                        lexState.currentExpression = "";
                        lexState.currentState = LexCurrentState.None;
                    }
                }
            }
            lexState.currentToken = "";
        }

        private static void VariableStateProcess(ref LexState lexState)
        {
            if (lexState.currentToken == "\n" || lexState.currentToken == "\r\n")
            {
                lexState.currentLine++;
            }
            else
            {
                if (lexState.currentToken.IndexOfAny(Static.CharArrays.VariableCharacters) != -1)
                {
                    lexState.currentVariable += lexState.currentToken;
                    if (lexState.index + 1 >= lexState.chars.Length)
                    {
                        lexState.tokens.Add(new Token(lexState.currentState.ToString().ToUpper() + ":" + lexState.currentVariable, lexState.currentLine));
                        lexState.currentState = LexCurrentState.None;
                        lexState.currentVariable = "";
                    }
                }
                else
                {
                    lexState.index--;
                    lexState.tokens.Add(new Token(lexState.currentState.ToString().ToUpper() + ":" + lexState.currentVariable, lexState.currentLine));
                    lexState.currentState = LexCurrentState.None;
                    lexState.currentVariable = "";
                }
            }
            lexState.currentToken = "";
        }

        private static void AddCurrentToken(ref LexState lexState)
        {
            lexState.tokens.Add(new Token(lexState.currentToken, lexState.currentLine));
            lexState.currentToken = "";
        }
    }

    public enum LexCurrentState
    {
        String,
        Number,
        Expression,
        Variable,
        Comment,
        Operation,
        None
    }
}
