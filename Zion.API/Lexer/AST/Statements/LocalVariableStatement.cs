using Zion.API.Lexer.AST.Scopes;
using Zion.API.Parser;

namespace Zion.API.Lexer.AST.Statements;

public record LocalVariableStatement(
    Token ReturnType,
    Token Identifier,
    Token Expression,
    ScopeAst Parent
) : Statement;