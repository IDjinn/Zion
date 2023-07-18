using Zion.API.Parser;

namespace Zion.API.Lexer.AST.Statements;

public record BlockStatement(IEnumerable<Token> Tokens, AbstractSyntaxTree ParentAST) : Statement;