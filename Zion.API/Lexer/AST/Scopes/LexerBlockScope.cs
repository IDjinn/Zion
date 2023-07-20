using Zion.API.Parser;

namespace Zion.API.Lexer.AST.Scopes;

public record LexerBlockScope(ICollection<Token> Tokens, AbstractSyntaxTree ParentAST) : ScopeAst;