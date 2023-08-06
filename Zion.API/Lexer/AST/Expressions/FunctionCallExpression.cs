using Zion.API.Lexer.AST.Scopes;
using Zion.API.Parser;

namespace Zion.API.Lexer.AST.Expressions;

public record FunctionCallExpression(
    ICollection<Token> Parameters,
    ScopeAst Parent
) : Expression(Parent);