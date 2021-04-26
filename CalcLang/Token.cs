using System;
using System.Collections.Generic;
using System.Text;

namespace CalcLang
{
    public class Token
    {
        public TokenType Type { get; set; }
        public dynamic Value { get; set; }
        public int Position { get; set; }
    }

    public enum TokenType
    {
        Let,
        Identifier,
        Number,
        Operator
    }
}
