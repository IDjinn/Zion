using System.Diagnostics;
using Zion.API.Parser;

namespace Zion.API.Lexer;

public record struct LexerWalker(
    IReadOnlyList<Token> Tokens
)
{
    public int Index { get; private set; }

    public Token Current => Tokens[Index];

    public LexerType CurrentLexerType =>
        LexerTypeUtilities.KeywordToLexerType(LexerTypeUtilities.IdentifierKeyword(Current));

    public Token Advance(TokenType tokenType)
    {
        Debug.Assert(Current.TokenType == tokenType);
        return Advance();
    }

    public Token Advance(int count = 1)
    {
        var aux = Current;
        Index += count;
        return aux;
    }
}