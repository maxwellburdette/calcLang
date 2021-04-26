using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CalcLang
{
    // adapted from: http://stackoverflow.com/questions/722270/is-there-an-equivalent-to-the-scanner-class-in-c-sharp-for-strings
    public class Scanner : System.IO.StringReader
    {
        string currentWord;
        int pos;
        public Scanner(string source)
            : base(source)
        {
            pos = 0;
            ReadNextWord();
        }

        private void ReadNextWord()
        {
            System.Text.StringBuilder sb = new StringBuilder();
            char nextChar;
            int next;
            do
            {
                next = this.Read();
                this.pos++;
                if (next < 0)
                    break;
                nextChar = (char)next;
                if ((nextChar == '(' || nextChar == ')'))
                {
                    if (sb.Length == 0)
                        continue;
                    else
                        break;
                }
                else
                {
                    if (char.IsWhiteSpace(nextChar))
                        break;
                    if (nextChar == '*'
                        || nextChar == '/'
                        || nextChar == '+'
                        || nextChar == '-'
                        || nextChar == '^')
                    {
                        if (sb.Length > 1)
                        {
                            break;
                        }
                        sb.Append(nextChar);
                        break;
                    }
                    sb.Append(nextChar);
                }
            } while (true);
            while ((this.Peek() >= 0) && (char.IsWhiteSpace((char)this.Peek())))
                this.Read();
            if (sb.Length > 0)
                currentWord = sb.ToString();
            else
                currentWord = null;
        }

        public int GetPosition()
        {
            return pos;
        }

        public bool HasNextInt()
        {
            if (currentWord == null)
                return false;
            int dummy;
            return int.TryParse(currentWord, out dummy);
        }

        public int NextInt()
        {
            try
            {
                return int.Parse(currentWord);
            }
            finally
            {
                ReadNextWord();
            }
        }

        public bool HasNextDouble()
        {
            if (currentWord == null)
                return false;
            double dummy;
            return double.TryParse(currentWord, out dummy);
        }

        public double NextDouble()
        {
            try
            {
                return double.Parse(currentWord);
            }
            finally
            {
                ReadNextWord();
            }
        }

        public bool HasNextOperator()
        {
            return Regex.Match(currentWord, @"\+|\-|\*|\/|\^").Success;
        }


        public OperatorType NextOperator()
        {
            try
            {
                return Expression.StringToOperator(currentWord);
            }
            finally
            {
                ReadNextWord();
            }
        }

        public bool HasNextIdentifier()
        {
            return Regex.Match(currentWord, @"[a-zA-Z][a-zA-Z\d]*").Success;
        }

        public string NextIdentifier()
        {
            try
            {
                return currentWord;
            }
            finally
            {
                ReadNextWord();
            }
        }

        public bool HasNext()
        {
            return currentWord != null;
        }
    }
}
