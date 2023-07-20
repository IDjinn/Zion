using System.Diagnostics;
using System.Runtime.CompilerServices;
using Zion.API.Lexer;
using Zion.API.Lexer.AST;
using Zion.API.Lexer.AST.Scopes;
using Zion.API.Lexer.AST.Statements;
using Zion.API.Parser;

namespace Zion.Lexer;

public class ZionLexer : ILexer
{
    public const int MaxWhileDeep = 30;

    public AbstractSyntaxTree GetSyntaxTree(ICollection<Token> parsedTokens)
    {
        var lexerWalker = new LexerWalker(parsedTokens.ToList());
        var programFileScope = new FileScope();
        // TODO : SCOPES
        var program = new ProgramAST(new List<ScopeAst>() { programFileScope });
        do
        {
            var token = lexerWalker.CurrentToken;
            switch (token.TokenType)
            {
                case TokenType.Identifier:
                    TryLexerIdentifier(programFileScope, ref lexerWalker);
                    break;
            }
        } while (!lexerWalker.EOF);

        return program;
    }

    public bool TryLexerScope(ScopeAst scope, ICollection<Token> tokens)
    {
        // try 
        // imports > class > functions > garbadge
        var walker = new LexerWalker(tokens.ToList());
        do
        {
            if (LexerTypeUtilities.IsItImport(walker.CurrentToken) && TryLexerImport(scope, ref walker))
                continue;

            var lexerType =
                LexerTypeUtilities.KeywordToLexerType(LexerTypeUtilities.IdentifierKeyword(walker.CurrentToken));
            if (lexerType == SyntaxKind.Unknown) return false;
        } while (!walker.EOF);

        return scope.Statements.Any();
    }

    public bool TryLexerImport(ScopeAst scope, ref LexerWalker lexer)
    {
        // TODO: IMPORT FILES
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool is_variable(ref LexerWalker walker)
    {
        var hasVariableKeyword = walker.CurrentToken.TokenType == TokenType.Identifier
                                 && walker.CurrentToken.Value == LexerTypeUtilities.KindToKeyword[SyntaxKind.Variable];
        if (!hasVariableKeyword) return false; // TODO: REUSE OF VARIABLES

        walker.Advance(); // var
        var identifier = walker.AdvanceIf(TokenType.Identifier);

        // TODO OPERATORS
        return true;
    }


    public bool TryLexerClass(ScopeAst scope, ref LexerWalker lexer)
    {
        if (!LexerTypeUtilities.IsItClass(lexer.CurrentToken)) return false;

        var classKeyword = lexer.Advance();
        var identifier = lexer.AdvanceIf(TokenType.Identifier);

        // {
        lexer.AdvanceIf(TokenType.OpenCurlyBracket);
        var deep = 0;
        do
        {
            // we expect properties
            if (is_variable(ref lexer))
            {
                // handle properties
            }
        } while (deep++ < MaxWhileDeep);

        lexer.AdvanceIf(TokenType.CloseCurlyBracket);
        // } 

        return false;
    }


    public bool TryLexerIdentifier(ScopeAst scopeAst, ref LexerWalker lexer)
    {
        var lexerType = lexer.CurrentLexeme.ContextualKind;
        if (lexerType == SyntaxKind.Unknown) return false;

        var walker = lexer with { };
        if (LexerTypeUtilities.MayIsFunctionDeclaration(lexerType) && TryLexerFunction(scopeAst, ref lexer, ref walker))
            return true;


        return false;
    }

    // TODO: BETTER WAY TO ADVANCE FUNCTIONS TOKENS WITHOUT PASS INTO LEXER ARG
    private bool TryLexerFunction(ScopeAst scopeAst, ref LexerWalker lexer, ref LexerWalker walker)
    {
        var returnType = walker.Advance();
        Debug.Assert(returnType != null);
        Debug.Assert(lexer.CurrentLexeme.ContextualKind.HasFlag(SyntaxKind.ValueType) ||
                     lexer.CurrentLexeme.ContextualKind.HasFlag(SyntaxKind.ReferenceType));

        var identifier = walker.Advance(); // type identifier
        Debug.Assert(identifier != null);
        if (identifier.TokenType is not TokenType.Identifier)
            return false;

        walker.AdvanceIf(TokenType.OpenRoundBracket);
        walker.AdvanceIf(TokenType.CloseRoundBracket);
        var blockScope = LexerFunctionBlockScope(scopeAst, ref walker);

        var function = new FunctionStatement(
            new List<Token>(),
            returnType,
            identifier,
            new List<Token>(),
            blockScope
        );

        lexer.Advance(walker.Index - lexer.Index);
        scopeAst.AddStatement(function);
        return true;
    }

    public LexerBlockScope LexerFunctionBlockScope(ScopeAst scope, ref LexerWalker walker)
    {
        var blockStatement = new LexerBlockScope(new List<Token>(), scope);
        walker.AdvanceIf(TokenType.OpenCurlyBracket);

        // TODO: sanitize this
        while (walker.CurrentToken.TokenType != TokenType.CloseCurlyBracket)
            blockStatement.Tokens.Add(walker.Advance());

        // block code
        walker.AdvanceIf(TokenType.CloseCurlyBracket);

        return blockStatement;
    }
}