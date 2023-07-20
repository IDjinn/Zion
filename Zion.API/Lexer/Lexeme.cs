using Zion.API.Parser;

namespace Zion.API.Lexer;

public record Lexeme
{
    public Lexeme(Token token)
    {
        Token = token;

        Initialize();
    }

    public Token Token { get; init; }
    public SyntaxKind ContextualKind { get; set; }

    private void Initialize()
    {
        var type = Token.TokenType;
        if (type == TokenType.Identifier && Token.Value is not null)
        {
            ContextualKind = LexerTypeUtilities.KeywordToKind.SafeIndexOf(Token.Value);
        }
    }
}