using Zion.API.Lexer.AST.Scopes;
using Zion.API.Parser;

namespace Zion.API.Lexer.AST.Statements.Functions;

public record FunctionCallStatement(
    Token Identifier,
    ICollection<Token> Parameters,
    ScopeAst Parent
) : Statement;