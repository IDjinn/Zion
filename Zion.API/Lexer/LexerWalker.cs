using System.Diagnostics;
using Zion.API.Parser;

namespace Zion.API.Lexer;

public record struct LexerWalker
{
    private readonly IList<Lexeme> _lexemes = new List<Lexeme>();

    public LexerWalker(IReadOnlyList<Token> tokens)
    {
        Tokens = tokens;

        foreach (var token in tokens)
        {
            _lexemes.Add(new Lexeme(token));
        }
    }

    public readonly IReadOnlyList<Token> Tokens { get; init; }

    public IReadOnlyCollection<Lexeme> Lexemes => _lexemes.AsReadOnly();
    public bool EOF => Tokens.Count == Index;
    public int Index { get; private set; }

    public Token CurrentToken => Tokens[Index];
    public Lexeme CurrentLexeme => _lexemes[Index];

    public SyntaxKind CurrentSyntaxKind =>
        LexerTypeUtilities.KeywordToLexerType(LexerTypeUtilities.IdentifierKeyword(CurrentToken));

    public Token AdvanceIf(TokenType tokenType)
    {
        Debug.Assert(CurrentToken.TokenType == tokenType);
        return Advance();
    }

    public bool Peek(TokenType tokenType, int count = 1)
    {
        Debug.Assert(Index + count < Tokens.Count);
        return Tokens[Index + count].TokenType == tokenType;
    }

    public Token Advance(int count = 1)
    {
        var aux = CurrentToken;
        Index += count;
        return aux;
    }

    public ICollection<Token> TakeWhile(TokenType tokenType)
    {
        var ret = new List<Token>();
        do
        {
            if (CurrentToken.TokenType == tokenType)
                break;

            ret.Add(Advance());
        } while (!EOF);

        return ret;
    }
}