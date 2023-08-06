using Zion.API.Generation;
using Zion.API.Lexer;
using Zion.API.Parser;

namespace Zion.API;

public class ZionCompiler
{
    private readonly IGeneration _generation;
    private readonly ILexer _lexer;
    private readonly IParser _parser;

    public ZionCompiler(IParser parser, ILexer lexer, IGeneration generation)
    {
        _parser = parser;
        _lexer = lexer;
        _generation = generation;
    }

    public void Compile(string source)
    {
        var tokens = _parser.ParseTokens(source);
        var lexer = _lexer.GetSyntaxTree(tokens.ToList());
        _generation.Generate(lexer);
    }
}