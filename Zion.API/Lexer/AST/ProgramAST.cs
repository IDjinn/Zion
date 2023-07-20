using Zion.API.Lexer.AST.Scopes;

namespace Zion.API.Lexer.AST;

public record ProgramAST(
    IEnumerable<ScopeAst> Scopes
) : AbstractSyntaxTree;