namespace Zion.API.Parser;

public interface IParser
{
    IEnumerable<Token> ParseTokens(string source);
}