using Zion.API.Lexer.AST.Scopes;

namespace Zion.API.Lexer.AST.Expressions;

public abstract record Expression(ScopeAst Parent) : AbstractSyntaxTree;