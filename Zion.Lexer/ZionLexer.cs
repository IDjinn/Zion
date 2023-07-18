using System.Diagnostics;
using Zion.API.Lexer;
using Zion.API.Lexer.AST;
using Zion.API.Lexer.AST.Statements;
using Zion.API.Parser;

namespace Zion.Lexer;

public class ZionLexer : ILexer
{
    public AbstractSyntaxTree GetSyntaxTree(ICollection<Token> parsedTokens)
    {
        var lexerWalker = new LexerWalker(parsedTokens.ToList());
        var program = new ProgramAST(new List<Token>(), new List<Statement>());
        do
        {
            var token = lexerWalker.Current;
            switch (token.TokenType)
            {
                case TokenType.Identifier:
                    TryLexerIdentifier(program, ref lexerWalker);
                    break;
            }
        } while (true);

        return program;
    }

    public bool TryLexerIdentifier(AbstractSyntaxTree ast, ref LexerWalker lexer)
    {
        var lexerType = LexerTypeUtilities.KeywordToLexerType(LexerTypeUtilities.IdentifierKeyword(lexer.Current));
        if (lexerType == LexerType.Unknown) return false;

        var walker = lexer with { };
        if (LexerTypeUtilities.MayIsFunctionDeclaration(lexerType))
        {
            var returnType = walker.Current;
            walker.Advance();
            Debug.Assert(returnType != null);
            Debug.Assert(lexer.CurrentLexerType.HasFlag(LexerType.ValueType) ||
                         lexer.CurrentLexerType.HasFlag(LexerType.ReferenceType));

            var identifier = walker.Advance();
            Debug.Assert(identifier != null);

            walker.Advance(TokenType.OpenBrace);
            // var parameters = new List<Token>();
            // Debug.Assert(walker.Current.TokenType == TokenType.OpenBracket);
            // var parameters = walker.Advance();
            // Debug.Assert(walker.Current.TokenType == TokenType.OpenBracket);
            walker.Advance(TokenType.CloseBrace);

            var body = LexerBlockCode(ast, ref walker);
        }


        return false;
    }

    public AbstractSyntaxTree LexerBlockCode(AbstractSyntaxTree ast, ref LexerWalker walker)
    {
        var blockStatement = new BlockStatement(new List<Token>(), ast);
        walker.Advance(TokenType.OpenBracket);
        // block code
        walker.Advance(TokenType.CloseBracket);

        return blockStatement;
    }
}