namespace Zion.API.Parser;

public record Token(TokenType TokenType, Location Location, string? Value);