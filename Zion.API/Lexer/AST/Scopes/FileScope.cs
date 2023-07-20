using Zion.API.Lexer.AST.Statements;

namespace Zion.API.Lexer.AST.Scopes;

public record FileScope : ScopeAst
{
    public FileScope()
    {
    }

    public FileScope(IEnumerable<Statement> statements) : base(statements)
    {
    }
}