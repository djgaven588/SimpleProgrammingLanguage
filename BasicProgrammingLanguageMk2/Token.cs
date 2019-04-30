using System;
using System.Collections.Generic;
using System.Text;

namespace BasicProgrammingLanguageMk2
{
    public class Token
    {
        public LexState.Action action;
        public string data;

        public Token(LexState.Action action, string data)
        {
            this.action = action;
            this.data = data;
        }
    }
}
