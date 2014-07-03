using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetdevelopersdotnetdevelopersdotnetdevelopers
{
    public class TreeNode<T> : IEnumerable<TreeNode<T>>
    {
        public TreeNode(T value)
        {
            Children = new List<TreeNode<T>>();
            Value = value;
        }

        public List<TreeNode<T>> Children { get; set; }
        public T Value { get; set; }

        public IEnumerator<TreeNode<T>> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    // expression → function | nonzero-loop
    // expression-list → expression | expression expression-list
    // function → > | < | + | - | . | ,
    // nonzero-loop → [ expression-list ]

    public class Parser
    {
        Lexer _lexer;
        Token _current;
        Token _next;

        public Parser(string source)
        {
            _lexer = new Lexer(source);
            _current = _lexer.NextToken();
            if (_current != null)
                _next = _lexer.NextToken();
        }

        public TreeNode<TokenKind> Parse()
        {
            return ParseExpressionList();
        }

        TreeNode<TokenKind> ParseExpressionList()
        {
            var parent = new TreeNode<TokenKind>(TokenKind.None);
            while (Current != null)
                parent.Children.Add(ParseExpression());
            return parent;
        }

        TreeNode<TokenKind> ParseExpression()
        {
            if (Current.IsFunction())
                return ParseFunction();
            else
                return ParseNonZeroLoop();
        }

        TreeNode<TokenKind> ParseFunction()
        {
            if (Current.IsFunction())
                return new TreeNode<TokenKind>(Consume().Kind);
            throw new MismatchParseException("a function", Current);
        }

        TreeNode<TokenKind> ParseNonZeroLoop()
        {
            if (Current.Kind == TokenKind.NonZeroLoopOpen)
            {
                var node = new TreeNode<TokenKind>(TokenKind.NonZeroLoopOpen);
                Consume();
                while (Current.Kind != TokenKind.NonZeroLoopClose)
                    node.Children.Add(ParseExpression());
                Expect(TokenKind.NonZeroLoopClose);
                return node;
            }
            throw new MismatchParseException("a loop", Current);
        }

        Token Current { get { return _current; } }
        Token Peek { get { return _next; } }

        Token MoveNext()
        {
            var next = _next;
            _current = _next;
            _next = _lexer.NextToken();
            return _next;
        }

        Token Consume()
        {
            var current = _current;
            MoveNext();
            return current;
        }

        void Expect(TokenKind tokenKind)
        {
            var current = Consume();
            if (current == null)
                throw new EndOfFileParseException(tokenKind);
            if (current.Kind != tokenKind)
                throw new MismatchParseException(tokenKind, _next);
        }
    }

    public class ParseException : Exception
    {
        public ParseException(string message) : base(message) { }
    }

    public class EndOfFileParseException : ParseException
    {
        public EndOfFileParseException(TokenKind expecting)
            : base(String.Format("Expecting {0} but got end of file", expecting)) { }
    }

    public class MismatchParseException : ParseException
    {
        public MismatchParseException(TokenKind expecting, Token got)
            : this(expecting.ToString(), got) { }

        public MismatchParseException(string expecting, Token got)
            : base(String.Format(
                "Expecting {0} but got {1} instead at line {2} column {3}",
                expecting,
                got.Kind,
                got.Line,
                got.Column)) { }
    }
}
