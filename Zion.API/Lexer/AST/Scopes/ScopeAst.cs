using Zion.API.Lexer.AST.Statements;

namespace Zion.API.Lexer.AST.Scopes;

public record ScopeAst : AbstractSyntaxTree
{
    protected readonly IList<Statement> statements = new List<Statement>();

    public ScopeAst()
    {
    }

    protected ScopeAst(IEnumerable<Statement> statements)
    {
        this.statements = statements.ToList();
    }

    public IReadOnlyCollection<Statement> Statements => statements.AsReadOnly();

    public void AddStatement(Statement statement) => statements.Add(statement);
}