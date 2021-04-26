using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLang
{
    public class Expression
    {
        public Program Program { get; set; }
        public Expression Parent { get; set; }
        public OperatorType? Operator { get; set; }
        public Operand LeftOperand { get; set; }
        public Operand RightOperand { get; set; }

        public Expression(Program program)
        {
            Program = program;
        }

        public int Evaluate()
        {
            switch (Operator.Value)
            {
                case OperatorType.Multiply: return LeftOperand.Value * RightOperand.Value;
                    break;
                case OperatorType.Divide: return LeftOperand.Value / RightOperand.Value;
                    break;
                case OperatorType.Add: return LeftOperand.Value + RightOperand.Value;
                    break;
                case OperatorType.Subtract: return LeftOperand.Value - RightOperand.Value;
                    break;
                case OperatorType.Power: return (int) Math.Pow(LeftOperand.Value,RightOperand.Value);
                    break;
                default:
                    throw new InvalidOperationException("Could not determine operation to perform");
            }
        }

        public bool IsValid()
        {
            if (Operator.Value == OperatorType.Divide && RightOperand.Value == 0)
            {
                throw new DivideByZeroException("WHADJA DO?!");
            }
            return true;
        }

        public override string ToString()
        {
            return "({OperatorToString(Operator.Value)} {LeftOperand} {RightOperand})";
        }

        public static string OperatorToString(OperatorType op)
        {
            switch (op)
            {
                case OperatorType.Multiply:
                    return "*";
                case OperatorType.Divide:
                    return "/";
                case OperatorType.Add:
                    return "+";
                case OperatorType.Subtract:
                    return "-";
                case OperatorType.Power:
                    return "^";
                default:
                    throw new InvalidOperationException("Could not determine operation to perform");
            }
        }

        public static OperatorType StringToOperator(string op)
        {
            switch (op)
            {
                case "*":
                    return OperatorType.Multiply;
                case "/":
                    return OperatorType.Divide;
                case "+":
                    return OperatorType.Add;
                case "-":
                    return OperatorType.Subtract;
                case "^":
                    return OperatorType.Power;
                default:
                    throw new InvalidOperationException("Could not determine operation to perform");
            }
        }
    }
}
