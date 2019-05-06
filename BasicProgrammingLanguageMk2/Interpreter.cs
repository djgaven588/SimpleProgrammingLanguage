using System;
using System.Collections.Generic;
using System.Text;

namespace BasicProgrammingLanguageMk2
{
    public static class Interpreter
    {

        private static int currentIndex;
        private static Token[] tokens;
        //private static Dictionary<string, MethodDefine> definedMethods = new Dictionary<string, MethodDefine>();
        private static ASTNode rootNode = new ASTNode() { nodeType = ASTNode.NodeType.Root };
        private static ASTNode currentNode;

        public static void BeginInterpret(Token[] tokenIn)
        {
            tokens = tokenIn;
            currentNode = rootNode;
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
                        SpecialPhraseProcess();
                        break;
                    case LexState.Action.Modification:
                        ModificationProcess(tokens[currentIndex].data);
                        break;
                    default:
                        break;
                }
            }

            Output.WriteDebug("Node setup:");
            WriteOutNode(rootNode, 1);
        }

        private static void WriteOutNode(ASTNode node, int depth)
        {
            string before = new string('-', depth * 2);

            foreach (UsedNamespace item in node.usedNamespaces)
            {
                Output.WriteDebug(before + "Included: " + item.namespaceName);
            }

            foreach (Variable item in node.variables)
            {
                Output.WriteDebug(before + "Declared Variables: " + item.varName + ", Type: " + item.type);
            }

            foreach (ASTNode item in node.nodes)
            {
                string parametersForDeclare = "";
                if (item.nodeType == ASTNode.NodeType.MethodDefine)
                {
                    ASTMethodDefineNode methodDefineNode = (ASTMethodDefineNode)item;
                    parametersForDeclare = " Name: " + methodDefineNode.name + " Required Input: ";
                    for (int i = 0; i < methodDefineNode.defineMethod.paremeters.Length; i++)
                    {
                        parametersForDeclare += "Type: " + methodDefineNode.defineMethod.paremeters[i].type + " ";
                    }
                }
                else if (item.nodeType == ASTNode.NodeType.MethodCall)
                {
                    ASTMethodCallNode methodDefineNode = (ASTMethodCallNode)item;
                    parametersForDeclare = " Name: " + methodDefineNode.name + " " + " Given Input: ";
                    for (int i = 0; i < methodDefineNode.parameters.Length; i++)
                    {
                        parametersForDeclare += "Type: " + methodDefineNode.parameters[i].type + " ";
                    }
                }
                Output.WriteDebug(before + item.nodeType.ToString() + ((item.nodeType == ASTNode.NodeType.MethodDefine || item.nodeType == ASTNode.NodeType.MethodCall) ? parametersForDeclare : ""));
                if(item.nodes != null && item.nodes.Count > 0)
                    WriteOutNode(item, depth + 1);
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
                        currentNode.usedNamespaces.Add(new UsedNamespace() { namespaceName = find });
                        Output.WriteDebug($"Included {find}, it is now available in current and lower nodes.");
                    }
                    break;
                case "string":
                    if (HasRequestedTokens(1))
                    {
                        if (tokens[currentIndex + 1].action == LexState.Action.SpecialPhrase)
                        {
                            string variableName = tokens[currentIndex + 1].data;
                            Output.WriteDebug($"Variable of type 'string' with name '{variableName}' was declared! It is now available in current and lower nodes. (Empty contents)");
                            currentNode.variables.Add(new Variable() { type = Variable.Type.String, varName = variableName });
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
            Output.WriteDebug("Operation " + operation + ". No node added.");
        }

        private static void ComparisonProcess(string comparison)
        {
            Output.WriteDebug("Comparison " + comparison + ". No node added.");
        }

        private static void EndStatementProcess()
        {
            Output.WriteDebug("End statement" + ". No node change.");
        }

        private static void SpecialPhraseProcess()
        {
            string find = GetNamespaceOrClass(false);
            Variable[] parameters = GetAllParameters();
            if (parameters == null)
            {
                //Not a method, but a variable of something.
                Variable node = currentNode.FindVariable(find);
                currentNode.nodes.Add(new ASTVarNode() { var = node, nodeType = ASTNode.NodeType.Id });
                Output.WriteDebug($"The phrase '{find}' is being treated as a name. Continuing... Added var node.");
            }
            else
            {
                if (IsMethodDeclare())
                {
                    MethodDefine method = new MethodDefine() { methodContents = GetMethodTokens(), paremeters = parameters };
                    //definedMethods.Add(find, method);
                    currentNode.nodes.Add(new ASTMethodDefineNode() { defineMethod = method, nodeType = ASTNode.NodeType.MethodDefine, name = find });
                    Output.WriteDebug($"The phrase '{find}' is being treated as declaring a method. Added method node.");
                }
                else
                {
                    Output.WriteDebug($"The phrase '{find}' is being treated as a method. Continuing...");
                    ASTMethodCallNode call = new ASTMethodCallNode() { name = find, nodeType = ASTNode.NodeType.MethodCall, parameters = parameters };
                    currentNode.nodes.Add(call);
                    /*
                    if (find == "Console.Write")
                    {
                        Output.ProgramOut(parameters[0].data);
                    }
                    else if (definedMethods.ContainsKey(find))
                    {
                        MethodDefine method = definedMethods[find];

                        bool lookingForType = true;
                        for (int i = 0; i < method.paremeters.Length; i++)
                        {
                            if (lookingForType)
                            {
                                if (parameters[i / 2].action.ToString().ToLower() != method.paremeters[i].data.ToLower())
                                {
                                    Output.WriteError($"Parameter #'{i / 2}' is not the type specified by the called method. Was expecting '{method.paremeters[i].data.ToLower()}'");
                                    return;
                                }
                            }
                            else
                            {
                                //Define variable in scope of method
                                Output.WriteDebug("NOT IMPLEMENTED: Method call needs to define variables in method scope.");
                            }
                            lookingForType = !lookingForType;
                        }

                        int previousIndex = currentIndex;
                        Token[] previousTokens = tokens;

                        BeginInterpret(method.methodContents);

                        currentIndex = previousIndex;
                        tokens = previousTokens;
                    }
                    else
                    {
                        Output.WriteError($"The method '{find}' is not defined! (Method defined after called?)");
                    }
                    */
                    
                }
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

        private static Variable[] GetAllParameters()
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
                    Queue<Variable> variableQueue = new Queue<Variable>();

                    while (HasRequestedTokens(1))
                    {
                        currentOffset++;
                        Token nextToken = tokens[currentIndex + currentOffset];
                        if (nextToken.action != LexState.Action.ParametersClose)
                        {
                            Output.WriteDebug(nextToken.action + ", " + nextToken.data);
                            if (nextToken.action == LexState.Action.SpecialPhrase)
                            {
                                Variable foundVariable = currentNode.FindVariable(nextToken.data);
                                if (foundVariable != null)
                                {
                                    variableQueue.Enqueue(new Variable() { isReference = true, type = foundVariable.type, varName = foundVariable.varName });
                                }
                                else
                                {
                                    Output.WriteError($"The variable '{nextToken.data}' does not exist! (Did you define it?)");
                                }
                            }
                            else
                            {
                                Output.WriteError($"GetAllParameters, not setup to handle anything other than special phrase!");
                            }

                            if (nextToken.action == LexState.Action.ParametersOpen)
                            {
                                levelsDeep++;
                            }
                        }
                        else
                        {
                            if (levelsDeep == 0)
                            {
                                currentIndex += currentOffset;
                                return variableQueue.ToArray();
                            }
                            else
                            {
                                variableQueue.Enqueue(new Variable() {
                                    varName = (nextToken.action == LexState.Action.SpecialPhrase) ? null : nextToken.data });
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

        private static bool IsMethodDeclare()
        {
            if (HasRequestedTokens(1))
            {
                if (tokens[currentIndex + 1].action == LexState.Action.MethodOpen)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Output.WriteError($"Code ended before check for method open could be found. (End of file?)");
                return false;
            }
        }

        private static Token[] GetMethodTokens()
        {
            if (HasRequestedTokens(1))
            {
                if (tokens[currentIndex + 1].action != LexState.Action.MethodOpen)
                {
                    Output.WriteError($"After a function declaration, it is expected that the block is defined using '{Static.beginBlock}', and closed using '{Static.endBlock}'.");
                    return null;
                }
                else
                {
                    int levelsDeep = 0;
                    int currentOffset = 1;

                    Queue<Token> tokenQueue = new Queue<Token>();

                    while (HasRequestedTokens(1))
                    {
                        currentOffset++;
                        Token nextToken = tokens[currentIndex + currentOffset];
                        if (nextToken.action != LexState.Action.MethodClose)
                        {
                            tokenQueue.Enqueue(nextToken);
                            if (nextToken.action == LexState.Action.MethodOpen)
                            {
                                levelsDeep++;
                            }
                        }
                        else
                        {
                            if (levelsDeep == 0)
                            {
                                currentIndex += currentOffset;
                                return tokenQueue.ToArray();
                            }
                            else
                            {
                                tokenQueue.Enqueue(nextToken);
                                levelsDeep--;
                            }
                        }
                    }

                    Output.WriteError($"Code ended before the end of the function block. (End of file?)");
                    return null;
                }
            }
            else
            {
                Output.WriteError($"Code ended before the end of the function block. (End of file?)");
                return null;
            }
        }
    }
}
