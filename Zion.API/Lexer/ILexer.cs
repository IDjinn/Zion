using Zion.API.Parser;

namespace Zion.API.Lexer;

public interface ILexer
{
    AbstractSyntaxTree GetSyntaxTree(ICollection<Token> parsedTokens);
}