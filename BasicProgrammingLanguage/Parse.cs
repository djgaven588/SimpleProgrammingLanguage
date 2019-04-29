using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicProgrammingLanguage
{
    class Parse
    {
        public static Dictionary<string, string> variables = new Dictionary<string, string>();

        public static async Task DoParse(Token[] tokens)
        {
            if (Program.TestingMode)
            {
                Output.WriteDebug("Starting Parse in 3...");
                await Task.Delay(1000);
                Output.WriteDebug("Starting Parse in 2..");
                await Task.Delay(1000);
                Output.WriteDebug("Starting Parse in 1.");
                await Task.Delay(1000);
            }

            for (int i = 0; i < tokens.Length; i++)
            {
                if(Program.TestingMode)
                    await Task.Delay(250);

                Token currentToken = tokens[i];
                if (currentToken.tokenValue == "print")
                {
                    if (i + 1 < tokens.Length)
                    {
                        DoPrint(currentToken, tokens[i + 1]);
                        i++;
                    }
                    else
                    {
                        SyntaxError("print", currentToken.line, "Parameter missing! (End of file)");
                    }
                }
                else if (currentToken.tokenValue == "input")
                {
                    if (i + 2 < tokens.Length)
                    {
                        DoPrint(tokens[i], tokens[i + 1]);
                        WriteVariable(tokens[i + 2].tokenValue, "STRING:\"" + Console.ReadLine() + "\"");
                        i += 2;
                    }
                    else
                    {
                        SyntaxError("Input", currentToken.line, "2 Parameters are required!");
                    }
                }
                else if (currentToken.tokenValue.Length > 9 && currentToken.tokenValue.Substring(0, 9) == "VARIABLE:")
                {
                    if (i + 1 < tokens.Length)
                    {
                        string nextTokenValue = tokens[i + 1].tokenValue;
                        if (nextTokenValue == "SETEQUAL")
                        {
                            if (i + 2 < tokens.Length)
                            {
                                if (i + 3 < tokens.Length)
                                {
                                    if (tokens[i + 3].tokenValue == "OP")
                                    {
                                        if (i + 4 < tokens.Length)
                                        {
                                            string operation = tokens[i + 4].tokenValue;
                                            switch (operation)
                                            {
                                                case "+":
                                                case "-":
                                                case "*":
                                                case "/":
                                                case "%":
                                                    string firstValue = tokens[i + 2].tokenValue;

                                                    if (firstValue.Contains("VARIABLE:"))
                                                    {
                                                        if (ReadVariable(firstValue, out string varValue))
                                                        {
                                                            firstValue = varValue;
                                                        }
                                                        else
                                                        {
                                                            SyntaxError("Assign Variable OP", currentToken.line, "Variable is not assigned!");
                                                        }
                                                    }

                                                    if (i + 5 < tokens.Length)
                                                    {
                                                        string secondValue = tokens[i + 5].tokenValue;

                                                        if (secondValue.Contains("VARIABLE:"))
                                                        {
                                                            if (ReadVariable(secondValue, out string varValue))
                                                            {
                                                                secondValue = varValue;
                                                            }
                                                            else
                                                            {
                                                                SyntaxError("Assign Variable OP", currentToken.line, "Variable is not assigned!");
                                                            }
                                                        }

                                                        if (firstValue.Contains("STRING:\""))
                                                        {
                                                            if (operation != "+")
                                                            {
                                                                SyntaxError("Assign Variable OP", currentToken.line, "The operation was invalid for type string. Strings can only use the '+' operation.");
                                                            }
                                                            else
                                                            {
                                                                if (secondValue.Contains("STRING:\""))
                                                                {
                                                                    secondValue = secondValue.Substring("STRING:\"".Length, secondValue.Length - 1);
                                                                }
                                                                else if (secondValue.Contains("NUMBER:"))
                                                                {
                                                                    secondValue = secondValue.Substring("NUMBER:".Length);
                                                                }
                                                                VarSetEqual(currentToken, new Token(firstValue.Substring(0, firstValue.Length - 1) + secondValue, currentToken.line));
                                                            }
                                                        }
                                                        else if (firstValue.Contains("NUMBER:"))
                                                        {
                                                            double firstNumber = double.Parse(firstValue.Substring("NUMBER:".Length));
                                                            if (secondValue.Contains("NUMBER:"))
                                                            {
                                                                double secondNumber = double.Parse(secondValue.Substring("NUMBER:".Length));
                                                                switch (operation)
                                                                {
                                                                    case "+":
                                                                        firstNumber += secondNumber;
                                                                        break;
                                                                    case "-":
                                                                        firstNumber -= secondNumber;
                                                                        break;
                                                                    case "*":
                                                                        firstNumber *= secondNumber;
                                                                        break;
                                                                    case "/":
                                                                        firstNumber /= secondNumber;
                                                                        break;
                                                                    case "%":
                                                                        firstNumber %= secondNumber;
                                                                        break;
                                                                }
                                                                VarSetEqual(currentToken, new Token("NUMBER:" + firstNumber, currentToken.line));
                                                            }
                                                            else
                                                            {
                                                                Output.WriteDebug(secondValue);
                                                                SyntaxError("Assign Variable OP", currentToken.line, "The type of the second argument must be a number / number variable");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            SyntaxError("Assign Variable OP", currentToken.line, "The type of the first argument is not valid for any operations.");
                                                        }
                                                        i += 3;
                                                    }
                                                    else
                                                    {
                                                        SyntaxError("Assign Variable OP", currentToken.line, "The value / variable that is used during the operation is missing. (End of file?)");
                                                    }
                                                    i += 2;
                                                    break;
                                                default:
                                                    SyntaxError("Assign Variable OP", currentToken.line, "The operation specified is invalid!");
                                                    i += 1;
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            SyntaxError("Assign Variable OP", currentToken.line, "An operation was not found. (End of file?)");
                                        }
                                    }
                                    else
                                    {
                                        VarSetEqual(currentToken, tokens[i + 2]);
                                    }
                                }
                                else
                                {
                                    VarSetEqual(currentToken, tokens[i + 2]);
                                }
                            }
                            else
                            {
                                SyntaxError("Assign Variable", currentToken.line, "'=' Is missing data parameter. (End of file?)");
                            }
                            i += 2;
                        }
                        else
                        {
                            SyntaxError("Assign Variable", currentToken.line, $"{nextTokenValue}, is an invalid operation for type variable!");
                        }
                    }
                    else
                    {
                        SyntaxError("Assign Variable", tokens[i].line, "Missing operation (End of file?)");
                    }
                }
                else if (currentToken.tokenValue == "if")
                {
                    if (i + 1 < tokens.Length)
                    {
                        string toCompare = tokens[i + 1].tokenValue;

                        if (i + 2 < tokens.Length)
                        {
                            string compareMethod = tokens[i + 2].tokenValue;
                            if (compareMethod.Length > 2 && compareMethod.Substring(0, 2) == "IS")
                            {
                                if (i + 3 < tokens.Length)
                                {
                                    string secondToCompare = tokens[i + 3].tokenValue;
                                    if (!Compare(toCompare, compareMethod, secondToCompare, currentToken.line))
                                    {
                                        int tokenSearchIndex = i + 4;
                                        int ifDepth = 0;
                                        while (true)
                                        {
                                            if (tokens.Length <= tokenSearchIndex)
                                            {
                                                SyntaxError("If", currentToken.line, "Expected for if statement to end. End or else is missing! (End of file?)");
                                                i = tokenSearchIndex;
                                                break;
                                            }

                                            string possibleIfEnding = tokens[tokenSearchIndex].tokenValue;

                                            if (possibleIfEnding == "if")
                                            {
                                                ifDepth++;
                                            }

                                            if (possibleIfEnding == "end" || (possibleIfEnding == "else" && ifDepth == 0))
                                            {
                                                ifDepth--;
                                                if (ifDepth == -1)
                                                {
                                                    i = tokenSearchIndex;
                                                    break;
                                                }
                                            }

                                            tokenSearchIndex++;
                                        }
                                    }
                                    else
                                    {
                                        i += 3;
                                    }
                                }
                                else
                                {
                                    SyntaxError("If", currentToken.line, "Expected variable, expression, number, string, or other data (End of file?)");
                                }
                            }
                            else
                            {
                                SyntaxError("If", currentToken.line, "Comparison is missing!");
                            }
                        }
                        else
                        {
                            SyntaxError("If", currentToken.line, "If statement not expected to end just after comparison (End of file?)");
                        }
                    }
                    else
                    {
                        SyntaxError("If", currentToken.line, $"Expected variable or comparison (End of file?)");
                    }
                }
                else if (currentToken.tokenValue == "else")
                {
                    int tokenSearchIndex = i + 1;
                    while (true)
                    {
                        if (tokens.Length <= tokenSearchIndex)
                        {
                            SyntaxError("Else", currentToken.line, "Expected for current if / else statement to end. End or else is missing! (End of file?)");
                            break;
                        }

                        string possibleIfEnding = tokens[tokenSearchIndex].tokenValue;

                        if (possibleIfEnding == "end")
                        {
                            i = tokenSearchIndex;
                            break;
                        }

                        tokenSearchIndex++;
                    }
                }
                else if (currentToken.tokenValue == "castToNum")
                {
                    if (i + 2 < tokens.Length)
                    {
                        string toCast = tokens[i + 1].tokenValue;

                        if (toCast.Contains("VARIABLE:"))
                        {
                            if (ReadVariable(toCast, out string varValue))
                            {
                                toCast = varValue;
                            }
                            else
                            {
                                SyntaxError("castToNum", currentToken.line, "Variable is not assigned!");
                            }
                        }

                        toCast = new string(toCast.Where(c => char.IsDigit(c) || c == '-' || c == '.').ToArray());
                        double.TryParse(toCast, out double result);
                        VarSetEqual(tokens[i + 2], new Token("NUMBER:" + result, currentToken.line));
                        i += 2;
                    }
                    else
                    {
                        SyntaxError("castToNum", currentToken.line, "castToNum requires two arguments which are the thing to cast, and the place to store it. (End of file?)");
                    }
                }
                else if (currentToken.tokenValue == "castToString")
                {
                    if (i + 2 < tokens.Length)
                    {
                        string toCast = tokens[i + 1].tokenValue;

                        if (toCast.Contains("VARIABLE:"))
                        {
                            if (ReadVariable(toCast, out string varValue))
                            {
                                toCast = varValue;
                            }
                            else
                            {
                                SyntaxError("castToString", currentToken.line, "Variable is not assigned!");
                            }
                        }

                        for (int x = 0; x < Static.StringArrays.Types.Length; x++)
                        {
                            if (toCast.Contains(Static.StringArrays.Types[x]))
                            {
                                toCast = toCast.Substring(Static.StringArrays.Types[x].Length);
                                break;
                            }
                        }

                        VarSetEqual(tokens[i + 2], new Token("STRING:" + toCast, currentToken.line));
                        i += 2;
                    }
                    else
                    {
                        SyntaxError("castToString", currentToken.line, "castToString requires two arguments which are the thing to cast, and the place to store it. (End of file?)");
                    }
                }
                else if (currentToken.tokenValue == "loadFile")
                {
                    if (i + 2 < tokens.Length)
                    {
                        string path = tokens[i + 1].tokenValue;

                        if (path.Contains("VARIABLE:"))
                        {
                            if (ReadVariable(path, out string varValue))
                            {
                                path = varValue;
                            }
                            else
                            {
                                SyntaxError("loadFile", currentToken.line, "Variable is not assigned!");
                            }
                        }

                        if (!path.Contains("STRING:"))
                        {
                            SyntaxError("loadFile", currentToken.line, "The variable / value used for the file path should be a string!");
                        }
                        else
                        {
                            path = path.Substring("STRING:\"".Length);
                            path = path.Substring(0, path.Length - 1);

                            string fileContents = "";
                            if (File.Exists(path))
                            {
                                fileContents = File.ReadAllText(path);
                            }
                            else
                            {
                                SyntaxError("loadFile", currentToken.line, "The path specified for the desired file does not exist. Double check the file location.");
                            }

                            VarSetEqual(tokens[i + 2], new Token("STRING:\"" + fileContents + "\"", currentToken.line));
                        }

                        i += 2;
                    }
                    else
                    {
                        SyntaxError("loadFile", currentToken.line, "loadFile requires two arguments which are the path to the desired file, and the place to store it. (End of file?)");
                    }
                }
                else if (currentToken.tokenValue == "runCode")
                {
                    if (i + 1 < tokens.Length)
                    {
                        string code = tokens[i + 1].tokenValue;

                        if (code.Contains("VARIABLE:"))
                        {
                            if (ReadVariable(code, out string varValue))
                            {
                                code = varValue;
                            }
                            else
                            {
                                SyntaxError("runCode", currentToken.line, "Variable is not assigned!");
                            }
                        }

                        if (!code.Contains("STRING:"))
                        {
                            SyntaxError("runCode", currentToken.line, "The variable / value used for storing the code should be a string!");
                        }
                        else
                        {
                            code = code.Substring("STRING:\"".Length);
                            code = code.Substring(0, code.Length - 1);

                            Token[] tokenList = await Lex.DoLex(code, false);

                            await Parse.DoParse(tokenList);
                        }
                        i++;

                    }
                    else
                    {
                        SyntaxError("runCode", currentToken.line, "runCode requires an argument which is the variable / value containing the code to be ran. (End of file?)");
                    }
                }
            }
            await Task.Delay(1);
        }

        private static void DoPrint(Token print, Token content)
        {
            if (content.tokenValue.Contains("VARIABLE:"))
            {
                if (ReadVariable(content.tokenValue, out string varValue))
                {
                    DoPrint(print, new Token(varValue, content.line));
                }
                else
                {
                    SyntaxError("Print Variable", print.line, "Variable is not assigned!");
                }
            }
            else if (content.tokenValue.Contains("STRING:"))
            {
                Output.ProgramOut(content.tokenValue.Substring(7).Trim('"'));
            }
            else if (content.tokenValue.Contains("NUMBER:"))
            {
                Output.ProgramOut(content.tokenValue.Substring(7));
            }
            else if (content.tokenValue.Contains("EXPRESSION:"))
            {
                Output.ProgramOut(EvaluateExpression(content.tokenValue.Substring(11), print.line));
            }
            else
            {
                SyntaxError("print", print.line, "Parameter missing! (Is it a string, number, variable, or expression?)");
            }
        }

        private static void VarSetEqual(Token var, Token value)
        {
            if (value.tokenValue.Length > 9 && value.tokenValue.Substring(0, 9) == "VARIABLE:")
            {
                if (!ReadVariable(value.tokenValue, out string val))
                {
                    val = $"STRING:{"null"}";
                    SyntaxError("Set Variable Equal", var.line, $"The variable {value.tokenValue} is not set!");
                }
                WriteVariable(var.tokenValue, val);
            }
            else
            {
                WriteVariable(var.tokenValue, value.tokenValue);
            }
        }

        private static string EvaluateExpression(string expression, int line)
        {
            if (expression.Contains("EXPRESSION:"))
            {
                expression = expression.Substring(11);
            }

            string[] split = expression.Split(' ');
            double currentNumber = 0;
            string currentOperation = "";
            for (int i = 0; i < split.Length; i++)
            {
                if (double.TryParse(split[i], out double selectedNum))
                {
                    if (currentOperation != "")
                    {
                        if (currentOperation.IndexOfAny(Static.CharArrays.Expression) == -1)
                        {
                            SyntaxError("Evaluating Expression", line, "Invalid operation! Converting to '+' to fix issue.");
                            currentOperation = "+";
                        }

                        if (currentOperation == "+")
                        {
                            currentNumber += selectedNum;
                        }
                        else if (currentOperation == "-")
                        {
                            currentNumber -= selectedNum;
                        }
                        else if (currentOperation == "*")
                        {
                            currentNumber *= selectedNum;
                        }
                        else if (currentOperation == "/")
                        {
                            currentNumber /= selectedNum;
                        }
                        else if (currentOperation == "%")
                        {
                            currentNumber %= selectedNum;
                        }

                        currentOperation = "";
                    }
                    else
                    {
                        currentNumber = selectedNum;
                    }
                }
                else
                {
                    currentOperation = split[i];
                }
            }
            return currentNumber.ToString();
        }

        private static bool ReadVariable(string key, out string varValue)
        {
            if (variables.TryGetValue(key.Substring(9), out varValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void WriteVariable(string key, string value)
        {
            key = key.Substring(9);
            variables[key] = value;
        }

        private static bool Compare(string toCompare, string comparison, string comparedTo, int line)
        {
            bool validCompare = false;
            for (int i = 0; i < Static.StringArrays.Comparison.Length; i++)
            {
                if (comparison == Static.StringArrays.ComparisonParsed[i])
                {
                    validCompare = true;
                    break;
                }
            }

            if (!validCompare)
            {
                SyntaxError("Compare", line, "The comparison that was attempted is not a valid comparison!");
                return false;
            }

            if (toCompare.Contains("VARIABLE:"))
            {
                if (ReadVariable(toCompare, out string varValue))
                {
                    return Compare(varValue, comparison, comparedTo, line);
                }
                else
                {
                    return Compare("NULL", comparison, comparedTo, line);
                }
            }
            else if (comparedTo.Contains("VARIABLE:"))
            {
                if (ReadVariable(comparedTo, out string varValue))
                {
                    return Compare(toCompare, comparison, varValue, line);
                }
                else
                {
                    return Compare(toCompare, comparison, "NULL", line);
                }
            }
            else if (toCompare.Contains("EXPRESSION:"))
            {
                return Compare(EvaluateExpression(toCompare, line), comparison, comparedTo, line);
            }
            else if (comparedTo.Contains("EXPRESSION:"))
            {
                return Compare(toCompare, comparison, EvaluateExpression(comparedTo, line), line);
            }

            if (comparison == "ISEQUAL")
                return toCompare == comparedTo;
            if (comparison == "ISNOTEQUAL")
                return toCompare != comparedTo;

            if (toCompare == "NULL")
            {
                SyntaxError(toCompare, line, "First argument of comparison must not be null unless using == or !=");
                return false;
            }

            if (comparedTo == "NULL")
            {
                return false;
            }

            if (toCompare.Contains("STRING:") || comparedTo.Contains("STRING:"))
            {
                SyntaxError("Compare", line, "Attempted to do illegal compare of type " + comparison);
                return false;
            }

            double.TryParse(new string(toCompare.Where(c => char.IsDigit(c) || c == '-' || c == '.').ToArray()), out double firstValue);
            double.TryParse(new string(comparedTo.Where(c => char.IsDigit(c) || c == '-' || c == '.').ToArray()), out double secondValue);

            if (comparison == "ISLESS")
                return firstValue < secondValue;
            if (comparison == "ISGREATER")
                return firstValue > secondValue;

            if (comparison == "ISLESSOREQUAL")
                return firstValue <= secondValue;
            if (comparison == "ISGREATEROREQUAL")
                return firstValue >= secondValue;

            SyntaxError("Compare", line, "Comparison ended with no result. How did you do that? Defaulted to false");
            return false;
        }

        private static void SyntaxError(string operation, int line, string message)
        {
            Output.WriteError($"Syntax error for \"{operation}\" on line {line}. {message}");
        }
    }
}
