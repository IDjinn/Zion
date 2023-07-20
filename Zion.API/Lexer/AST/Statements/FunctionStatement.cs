using Zion.API.Lexer.AST.Scopes;
using Zion.API.Parser;

namespace Zion.API.Lexer.AST.Statements;

public record FunctionStatement(
    IEnumerable<Token> Visibility,
    Token ReturnType,
    Token Identifier,
    IEnumerable<Token> Parameters,
    LexerBlockScope Body
) : Statement;