using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLang
{
    public class Operand
    {
        public string Literal { get; set; }

        public int Value
        {
            get
            {
                if (Expression == null)
                {
                    return int.Parse(Literal);
                }
                else
                {
                    var possibleDef = Expression.Program.GetDefinition(Identifier);
                    if (possibleDef == null)
                    {
                        if (Literal == null)
                        {
                            return Expression.Evaluate();
                        }
                        else
                        {
                            return int.Parse(Literal);
                        }
                    }
                    else
                    {
                        return possibleDef.Value;
                    }
                }
            }
        }
        public string Identifier { get; set; }
        public Expression Expression { get; set; }
    }
}
