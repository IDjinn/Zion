using Zion.API.Lexer;
using Zion.API.Lexer.AST;
using Zion.API.Lexer.AST.Scopes;
using Zion.API.Parser;

namespace Zion.Lexer;

public class ZionLexerV2 : ILexer
{
    public AbstractSyntaxTree GetSyntaxTree(ICollection<Token> parsedTokens)
    {
        var scopes = new List<ScopeAst>();


        return new ProgramAST(scopes);
    }
}