using System;
using System.Collections.Generic;
using System.Text;

namespace BasicProgrammingLanguageMk2
{
    public static class Interpreter
    {

        private static int currentIndex;
        private static Token[] tokens;

        public static void BeginInterpret(Token[] tokenIn)
        {
            tokens = tokenIn;
            for (currentIndex = 0; currentIndex < tokens.Length; currentIndex++)
            {
                LexState.Action tokenAction = tokens[currentIndex].action;
                switch (tokenAction)
                {
                    case LexState.Action.Keyword:
                        KeywordProcess(tokens[currentIndex].data);
                        break;
                    case LexState.Action.Operation:
                        OperationProcess(tokens[currentIndex].data);
                        break;
                    case LexState.Action.Comparison:
                        ComparisonProcess(tokens[currentIndex].data);
                        break;
                    case LexState.Action.EndStatement:
                        EndStatementProcess();
                        break;
                    case LexState.Action.SpecialPhrase:
                        SpecialPhraseProcess(tokens[currentIndex].data);
                        break;
                    case LexState.Action.Modification:
                        ModificationProcess(tokens[currentIndex].data);
                        break;
                    default:
                        break;
                }
            }
        }

        private static bool HasRequestedTokens(int tokenCount)
        {
            if (tokens.Length > currentIndex + tokenCount)
            {
                return true;
            }
            return false;
        }

        private static void KeywordProcess(string keyword)
        {
            switch (keyword)
            {
                case "using":
                    string find = GetNamespaceOrClass();
                    if (find == "")
                    {
                        Output.WriteError("The parameter reguarding the namespace to be included is missing! A previous error indicated this.");
                    }
                    else
                    {
                        Output.WriteDebug($"Included {find}, this has no functionality yet. System is used by default until a proper solution is created.");
                    }
                    break;
                case "string":
                    if (HasRequestedTokens(1))
                    {
                        if (tokens[currentIndex + 1].action == LexState.Action.SpecialPhrase)
                        {
                            string variableName = tokens[currentIndex + 1].data;
                            Output.WriteDebug($"Variable of type 'string' with name '{variableName}' was declared!");
                            currentIndex++;
                        }
                        else
                        {
                            Output.WriteError("After keyword 'string', it is expected that a variable name follows.");
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private static void OperationProcess(string operation)
        {
            Output.WriteDebug("Operation " + operation);
        }

        private static void ComparisonProcess(string comparison)
        {
            Output.WriteDebug("Comparison " + comparison);
        }

        private static void EndStatementProcess()
        {
            Output.WriteDebug("End statement");
        }

        private static void SpecialPhraseProcess(string phrase)
        {
            string find = GetNamespaceOrClass(false);
            Token[] parameters = GetAllParameters();
            if (parameters == null)
            {
                //Not a method, but a variable of something.
                Output.WriteDebug($"The phrase '{find}' is being treated as a name. Continuing...");
            }
            else
            {

                if (find == "Console.Write")
                {
                    Output.ProgramOut(parameters[0].data);
                }
                Output.WriteDebug($"The phrase '{find}' is being treated as a method. Continuing...");
            }
        }

        private static void ModificationProcess(string modification)
        {
            Output.WriteDebug("Modification " + modification);
        }

        private static string GetNamespaceOrClass(bool start1Ahead = true)
        {
            if (HasRequestedTokens(2))
            {
                Token firstPar = tokens[currentIndex + ((start1Ahead) ? 1 : 0)];
                Token secondPar = tokens[currentIndex + ((start1Ahead) ? 1 : 0) + 1];

                string toUse = "";

                if (firstPar.action != LexState.Action.SpecialPhrase)
                {
                    Output.WriteError("Special phrase is missing!");
                    return "";
                }

                toUse += firstPar.data;

                if (secondPar.action == LexState.Action.Property)
                {
                    int currentOffset = ((start1Ahead) ? 1 : 0) + 1;
                    while (HasRequestedTokens(1) && (secondPar.action == LexState.Action.Property || secondPar.action == LexState.Action.SpecialPhrase))
                    {
                        currentOffset++;
                        secondPar = tokens[currentIndex + currentOffset];
                        if (secondPar.action == LexState.Action.SpecialPhrase)
                        {
                            toUse += $".{secondPar.data}";
                        }
                    }
                    currentOffset--;
                    currentIndex += currentOffset;

                    return toUse;
                }

                currentIndex += ((start1Ahead) ? 1 : 0);

                return toUse;
            }
            else
            {
                Output.WriteError("Missing required tokens for using!");
                return "";
            }
        }

        private static Token[] GetAllParameters()
        {
            if (HasRequestedTokens(1))
            {
                if (tokens[currentIndex + 1].action != LexState.Action.ParametersOpen)
                {
                    Output.WriteError($"After a function, it is expected that the parameters start symbol '{Static.beginParameters}' be used.");
                    return null;
                }
                else
                {
                    int levelsDeep = 0;
                    int currentOffset = 1;

                    Token nextToken = tokens[currentIndex + 1];
                    Queue<Token> tokenQueue = new Queue<Token>();

                    while (HasRequestedTokens(1))
                    {
                        currentOffset++;
                        nextToken = tokens[currentIndex + currentOffset];
                        if (nextToken.action != LexState.Action.ParametersClose)
                        {
                            tokenQueue.Enqueue(nextToken);
                            if (nextToken.action == LexState.Action.ParametersOpen)
                            {
                                levelsDeep++;
                            }
                        }
                        else
                        {
                            if (levelsDeep == 0)
                            {
                                return tokenQueue.ToArray();
                            }
                            else
                            {
                                tokenQueue.Enqueue(nextToken);
                                levelsDeep--;
                            }
                        }
                    }

                    Output.WriteError($"Code ended before the end of the parameters. (End of file?)");
                    return null;
                }
            }
            else
            {
                Output.WriteError($"Code ended before parameters could be found. (End of file?)");
                return null;
            }
        }
    }
}
