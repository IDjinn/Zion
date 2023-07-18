using Zion.API.Lexer.AST.Statements;
using Zion.API.Parser;

namespace Zion.API.Lexer.AST;

public record ProgramAST(
    IEnumerable<Token> Imports,
    IEnumerable<Statement> Statements
) : AbstractSyntaxTree;