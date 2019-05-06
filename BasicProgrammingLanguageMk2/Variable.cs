using System;
using System.Collections.Generic;
using System.Text;

namespace BasicProgrammingLanguageMk2
{
    public class Variable
    {
        public enum Type
        {
            Int, String
        }

        public Type type;
        public object var;
        public bool isTemp;
        public string varName;
    }
}
