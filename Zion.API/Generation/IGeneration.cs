using Zion.API.Lexer;

namespace Zion.API.Generation;

public interface IGeneration
{
    public void Generate(AbstractSyntaxTree syntaxTree);
}