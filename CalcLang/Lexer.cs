using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CalcLang
{
    public static class Lexer
    {
        public static dynamic ConvertToLiteral(string input)
        {
            if (Regex.Match(input, @"^(\+|\-)?\d+\.\d+$").Success)
            {
                return float.Parse(input);
            } else
            {
                return int.Parse(input);
            }
        }

        public static List<Token> GetTokens(string[] linesOfCode)
        {
            List<Token> tokens = new List<Token>();
            bool definitionsDone = false;
            foreach (var line in linesOfCode)
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmedLine))
                {
                    continue;
                }
                if (!trimmedLine.StartsWith("("))
                {
                    throw new Exception("Lines must start with a '('");                
                }
                if (!trimmedLine.EndsWith(")"))
                {
                    throw new Exception("Lines must end with a '('");
                }
                if (Regex.Match(trimmedLine, @"[^\(\)a-zA-Z\d\s\-\+\^\*\/\.]+").Success)
                {
                    throw new Exception("Line contains invalid characters");
                }
                if (definitionsDone && !Regex.Match(trimmedLine, @"^\(\s*let").Success)
                {
                    throw new Exception("Definitions must come before the expressions");
                }
                var match = Regex.Match(trimmedLine, @"^\(\s*(?<let>let)\s*(?<identifier>[a-zA-Z][a-zA-Z\d]*)\s*(?<number>(\+|\-
)?(\d+\.\d+|\d+))\)$");
                if (match.Success)
                {
                    var let = match.Groups["let"];
                    var identifier = match.Groups["identifier"];
                    var number = match.Groups["number"];
                    tokens.Add(new Token()
                    {
                       Type = TokenType.Let,
                       Position = 1,
                       Value = TokenType.Let
                    });
                    tokens.Add(new Token()
                    {
                        Type = TokenType.Identifier,
                        Position = 5,
                        Value = identifier.Value
                    });
                    tokens.Add(new Token()
                    {
                        Type = TokenType.Number,
                        Position = 5 + identifier.Length,
                        Value = ConvertToLiteral(number.Value)
                    });
                    continue;
                }
                else
                {
                    definitionsDone = true;
                    var lastPosition = 0;
                    var scanner = new Scanner(trimmedLine);
                    while (scanner.HasNext())
                    {
                        if (scanner.HasNextDouble() || scanner.HasNextInt())
                        {
                            dynamic value = scanner.HasNextDouble() ? scanner.NextDouble() : scanner.NextInt();
                            tokens.Add(new Token
                            {
                                Type = TokenType.Number,
                                Position = scanner.GetPosition(),
                                Value = value
                            });
                        }
                        else if (scanner.HasNextOperator())
                        {
                            tokens.Add(new Token
                            {
                                Type = TokenType.Operator,
                                Position = scanner.GetPosition(),
                                Value = scanner.NextOperator()
                            });
                        }
                        else if (scanner.HasNextIdentifier())
                        {
                            tokens.Add(new Token
                            {
                                Type = TokenType.Identifier,
                                Position = scanner.GetPosition(),
                                Value = scanner.NextIdentifier()
                            });
                        }
                        else
                        {
                            throw new Exception("Invalid syntax, what the heck is: {scanner.NextIdentifier()}?");
                        }
                        lastPosition = scanner.GetPosition();
                    }
                }
            }
            return tokens;
        }
    }
}
