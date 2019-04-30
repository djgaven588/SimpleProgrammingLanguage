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
                    case LexState.Action.Property:
                        PropertyProcess();
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
                    if (HasRequestedTokens(2))
                    {
                        Token firstPar = tokens[currentIndex + 1];
                        Token secondPar = tokens[currentIndex + 2];

                        string toUse = "";

                        if (firstPar.action != LexState.Action.SpecialPhrase)
                        {
                            Output.WriteError("Special phrase is missing for using!");
                            return;
                        }

                        toUse += firstPar.data;

                        if (secondPar.action == LexState.Action.Property)
                        {
                            int currentOffset = 3;
                            while (HasRequestedTokens(1) && (secondPar.action == LexState.Action.Property || secondPar.action == LexState.Action.Keyword))
                            {
                                if (secondPar.action == LexState.Action.SpecialPhrase)
                                {
                                    toUse += $".{secondPar.data}";
                                }
                                secondPar = tokens[currentIndex + currentOffset];
                                currentOffset++;
                            }
                            currentOffset--;
                            currentIndex += currentOffset;
                        }
                        else if (secondPar.action == LexState.Action.EndStatement)
                        {
                            currentIndex += 2;
                        }
                        else
                        {
                            Output.WriteError("Invalid action for using statement token 2");
                            return;
                        }

                        Output.WriteDebug($"Included {toUse}");
                    }
                    else
                    {
                        Output.WriteError("Missing required tokens for using!");
                        return;
                    }
                    break;
                default:
                    break;
            }
        }

        private static void OperationProcess(string operation)
        {

        }

        private static void ComparisonProcess(string comparison)
        {

        }

        private static void EndStatementProcess()
        {

        }

        private static void SpecialPhraseProcess(string phrase)
        {

        }

        private static void ModificationProcess(string modification)
        {

        }

        private static void PropertyProcess()
        {

        }
    }
}
