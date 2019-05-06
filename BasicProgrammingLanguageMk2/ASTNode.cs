using System;
using System.Collections.Generic;
using System.Text;

namespace BasicProgrammingLanguageMk2
{
    public class ASTNode
    {
        public List<ASTNode> nodes = new List<ASTNode>();
        public List<Variable> variables = new List<Variable>();
        public List<UsedNamespace> usedNamespaces = new List<UsedNamespace>();

        public enum NodeType
        {
            Root,
            Id,
            MethodDefine,
            MethodCall,
            Int,
            String,
            Operation
        }

        public NodeType nodeType;
        public ASTNode parentNode;
        
        public Variable FindVariable(string name)
        {
            for (int i = 0; i < variables.Count; i++)
            {
                if (variables[i].varName == name)
                {
                    return variables[i];
                }
            }
            return null;
        }
    }

    public class ASTIdNode : ASTNode
    {
        public string indentifier;
    }

    public class ASTMethodDefineNode : ASTNode
    {
        public string name;
        public MethodDefine defineMethod;
    }

    public class ASTMethodCallNode : ASTNode
    {
        public string name;
        public Variable[] parameters;
    }

    public class ASTVarNode : ASTNode
    {
        public Variable var;
    }

    public class ASTOperationNode : ASTNode
    {
        public enum Operation
        {
            Add, Sub, Mul, Div, Exp, Mod
        }
        public Operation operation;
    }

    public class ASTReturnNode : ASTNode
    {
        public enum ReturnType
        {
            Int, String, Void
        }

        public ReturnType returnType;
    }
}
