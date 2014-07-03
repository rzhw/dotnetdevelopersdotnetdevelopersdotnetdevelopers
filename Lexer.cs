using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetdevelopersdotnetdevelopersdotnetdevelopers
{
    public class Lexer
    {
        string[] _source;
        int _line;
        int _column;
        const string MatchString = "Developers";

        public Lexer(string source)
        {
            _source = source.Split('\n').Select(x => x.Trim()).ToArray();
            _line = 0;
            _column = 0;
        }

        public Token NextToken()
        {
            if (_line >= _source.Length)
                return null;

            string line = _source[_line];
            string tokenValue = "";

            // Get start of token
            int tokenStart = line.IndexOf(MatchString, _column);
            if (tokenStart == -1)
            {
                _line++;
                _column = 0;
                return NextToken();
            }

            // Add start of token
            tokenValue += MatchString;
            _column = tokenStart + MatchString.Length;

            // Get rest of token if connected
            while (true)
            {
                int tokenContinued = line.IndexOf(MatchString, _column);
                if (tokenContinued == -1 || tokenContinued > _column)
                    break;
                _column = tokenContinued + MatchString.Length;
                tokenValue += MatchString;
            }

            // Token is done
            var token = new Token(tokenValue, _line, tokenStart);

            // Past the line?
            if (_column >= _source[_line].Length)
            {
                _line++;
                _column = 0;
            }

            return token;
        }
    }
}
