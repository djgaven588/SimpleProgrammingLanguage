using System;
using System.Collections.Generic;
using System.Text;

namespace BasicProgrammingLanguage
{
    public class Token
    {
        public string tokenValue;
        public int line;

        public Token(string value, int l)
        {
            this.tokenValue = value;
            this.line = l;
        }
    }
}
