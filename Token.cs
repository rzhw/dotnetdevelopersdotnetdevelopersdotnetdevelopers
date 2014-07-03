using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetdevelopersdotnetdevelopersdotnetdevelopers
{
    public class Token
    {
        public Token(string str, int line, int column)
        {
            Kind = GetDevelopersTokenKind(str);
            Line = line;
            Column = column;
        }

        public TokenKind Kind { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }

        public bool IsFunction()
        {
            return Kind != TokenKind.NonZeroLoopOpen && Kind != TokenKind.NonZeroLoopClose;
        }

        static TokenKind GetDevelopersTokenKind(string str)
        {
            switch (str)
            {
                case "Developers": return TokenKind.IncrementPointee;
                case "DevelopersDevelopers": return TokenKind.DecrementPointee;
                case "DevelopersDevelopersDevelopers": return TokenKind.IncrementPointer;
                case "DevelopersDevelopersDevelopersDevelopers": return TokenKind.DecrementPointer;
                case "DevelopersDevelopersDevelopersDevelopersDevelopers": return TokenKind.GetChar;
                case "DevelopersDevelopersDevelopersDevelopersDevelopersDevelopers": return TokenKind.PutChar;
                case "DevelopersDevelopersDevelopersDevelopersDevelopersDevelopersDevelopers": return TokenKind.NonZeroLoopOpen;
                case "DevelopersDevelopersDevelopersDevelopersDevelopersDevelopersDevelopersDevelopers": return TokenKind.NonZeroLoopClose;
                default: throw new ArgumentException("Invalid token");
            }
        }
    }

    public enum TokenKind
    {
        None,
        IncrementPointer,
        DecrementPointer,
        IncrementPointee,
        DecrementPointee,
        PutChar,
        GetChar,
        NonZeroLoopOpen,
        NonZeroLoopClose
    }
}
